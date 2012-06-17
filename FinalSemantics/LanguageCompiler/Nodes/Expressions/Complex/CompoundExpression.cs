namespace LanguageCompiler.Nodes.Expressions.Complex
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.TopLevel;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Holds all data related to a "CompoundExpression" rule.
    /// </summary>
    public class CompoundExpression : ExpressionNode
    {
        /// <summary>
        /// Expression in LHS.
        /// </summary>
        private ExpressionNode lhs;

        /// <summary>
        /// RHS of expression.
        /// </summary>
        private Identifier rhs;

        /// <summary>
        /// Gets the left hand side of the expression.
        /// </summary>
        public ExpressionNode LHS
        {
            get { return this.lhs; }
        }

        /// <summary>
        /// Gets the right hand side of the expression.
        /// </summary>
        public Identifier RHS
        {
            get { return this.rhs; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Compound Expression");
            result.Nodes.Add(this.lhs.GetGUINode());
            result.Nodes.Add(this.rhs.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.lhs = ExpressionsFactory.GetPrimaryExpr(node.ChildNodes[0]);
            this.rhs = new Identifier();
            this.rhs.RecieveData(node.ChildNodes[2]);

            this.StartLocation = this.lhs.StartLocation;
            this.EndLocation = this.rhs.EndLocation;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            if (this.lhs.CheckSemanticErrors(scopeStack))
            {
                return true;
            }

            ExpressionType lhsType = this.lhs.GetExpressionType(scopeStack);
            ObjectExpressionType lhsObject = null;
            List<MemberDefinition> lhsMembers = null;

            if (lhsType is ObjectExpressionType)
            {
                lhsObject = lhsType as ObjectExpressionType;
                lhsMembers = lhsObject.DataType.Members;
            }
            else
            {
                ClassDefinition type = CompilerService.Instance.ClassesList[(lhsType as MethodExpressionType).Method.Type.Text];
                lhsObject = new ObjectExpressionType(type, MemberStaticType.Normal);
                lhsMembers = type.Members;
            }

            foreach (MemberDefinition member in lhsMembers)
            {
                if (member is MethodDefinition)
                {
                    MethodDefinition memberMethod = member as MethodDefinition;
                    if (memberMethod.Name.Text == this.rhs.Text)
                    {
                        return this.CheckMemberErrors(member, memberMethod.Name.Text, scopeStack, lhsObject.StaticType);
                    }
                }
                else if (member is FieldDefinition)
                {
                    FieldDefinition memberField = member as FieldDefinition;
                    foreach (FieldAtom atom in memberField.Atoms)
                    {
                        if (atom.Name.Text == this.rhs.Text)
                        {
                            return this.CheckMemberErrors(member, atom.Name.Text, scopeStack, lhsObject.StaticType);
                        }
                    }
                }
            }

            this.AddError(ErrorType.NoMemberWithThisName, lhsObject.DataType.Name.Text, this.rhs.Text);
            return true;
        }

        /// <summary>
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public override ExpressionType GetExpressionType(ScopeStack stack)
        {
            ClassDefinition lhsType = (this.lhs.GetExpressionType(stack) as ObjectExpressionType).DataType;

            foreach (MemberDefinition member in lhsType.Members)
            {
                if (member is FieldDefinition)
                {
                    FieldDefinition field = member as FieldDefinition;
                    foreach (FieldAtom atom in field.Atoms)
                    {
                        if (atom.Name.Text == this.rhs.Text)
                        {
                            return field.Type.GetExpressionType(stack);
                        }
                    }
                }
                else if (member is MethodDefinition)
                {
                    MethodDefinition method = member as MethodDefinition;
                    if (method.Name.Text == this.rhs.Text)
                    {
                        return new MethodExpressionType(method);
                    }
                }
            }

            throw new Exception("Faulty Type. Should be handled in semantics.");
        }

        /// <summary>
        /// Checks if this expression can be assigned to.
        /// </summary>
        /// <returns>True if this expression can be assigned to, false otherwise.</returns>
        public override bool IsAssignable()
        {
            return this.lhs.IsAssignable();
        }

        /// <summary>
        /// Checks if there were errors in accessing the member in this point of code.
        /// </summary>
        /// <param name="member">Member to check.</param>
        /// <param name="name">Name of this member.</param>
        /// <param name="stack">Current Scope Stack.</param>
        /// <param name="staticType">Indicates if it was called from a class.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        private bool CheckMemberErrors(MemberDefinition member, string name, ScopeStack stack, MemberStaticType staticType)
        {
            ClassDefinition currentClass = stack.GetClass();
            if (member.AccessorType == MemberAccessorType.Private)
            {
                if (member.Parent.Name.Text != currentClass.Name.Text)
                {
                    this.AddError(ErrorType.InaccessibleDueToProtectionLevel, name);
                    return true;
                }
            }
            else if (member.AccessorType == MemberAccessorType.Protected)
            {
                ClassDefinition ptr = currentClass;
                bool found = false;

                do
                {
                    if (member.Parent.Name.Text == ptr.Name.Text)
                    {
                        found = true;
                    }
                    else
                    {
                        if (CompilerService.Instance.ClassesList.ContainsKey(ptr.ClassBase.Text))
                        {
                            ptr = CompilerService.Instance.ClassesList[ptr.ClassBase.Text];
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                while (found);

                if (!found)
                {
                    this.AddError(ErrorType.InaccessibleDueToProtectionLevel, name);
                    return true;
                }
            }

            if (member.ModifierType == MemberModifierType.Abstract)
            {
                this.AddError(ErrorType.CallToAbstractFunction, name);
                return true;
            }

            if (staticType == MemberStaticType.Static && member.StaticType == MemberStaticType.Normal)
            {
                this.AddError(ErrorType.CallNormalMembersFromClass, name);
                return true;
            }
            else if (staticType == MemberStaticType.Normal && member.StaticType == MemberStaticType.Static)
            {
                this.AddError(ErrorType.CallStaticMembersFromObject, name);
                return true;
            }

            return false;
        }
    }
}