namespace LanguageCompiler.Nodes.Expressions.Basic
{
    using System;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.TopLevel;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Holds all data related to a "BinaryExpression" rule.
    /// </summary>
    public class BinaryExpression : ExpressionNode
    {
        /// <summary>
        /// LHS of expression.
        /// </summary>
        private ExpressionNode lhs;

        /// <summary>
        /// Operator of expression.
        /// </summary>
        private string operatorDefined;

        /// <summary>
        /// RHS of expression.
        /// </summary>
        private ExpressionNode rhs;

        /// <summary>
        /// Gets the operator of expression.
        /// </summary>
        public string OperatorDefined
        {
            get { return this.operatorDefined; }
        }

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
        public ExpressionNode RHS
        {
            get { return this.rhs; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode(this.operatorDefined + " Binary Expression");
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
            this.lhs = ExpressionsFactory.GetBaseExpr(node.ChildNodes[0]);
            this.operatorDefined = node.ChildNodes[1].Token.Text;
            this.rhs = ExpressionsFactory.GetBaseExpr(node.ChildNodes[2]);

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
            bool foundErrors = false;

            foundErrors |= this.rhs.CheckSemanticErrors(scopeStack);
            foundErrors |= this.lhs.CheckSemanticErrors(scopeStack);

            if (foundErrors)
            {
                return foundErrors;
            }

            ExpressionType lhsType = this.lhs.GetExpressionType(scopeStack);
            ExpressionType rhsType = this.rhs.GetExpressionType(scopeStack);

            if (OperatorDefinition.NonOverloadableOperators.Contains(this.operatorDefined) == false)
            {
                if (OperatorDefinition.AssignmentOperators.Contains(this.operatorDefined) && this.lhs.IsAssignable() == false)
                {
                    this.AddError(ErrorType.CannotAssignTo);
                    foundErrors = true;
                }

                if (lhsType is ObjectExpressionType == false)
                {
                    this.AddError(ErrorType.NotAValidLHS, this.operatorDefined);
                    return true;
                }

                ////if (lhsType is ObjectExpressionType && rhsType is ObjectExpressionType)
                ////{
                ////    ObjectExpressionType rhsObject = rhsType as ObjectExpressionType;
                ////    ObjectExpressionType lhsObject = lhsType as ObjectExpressionType;

                ////    if (rhsObject.DataType.IsMyParent(lhsObject.DataType.Name.Text) == false
                ////        && rhsObject.DataType.Name.Text != lhsObject.DataType.Name.Text)
                ////    {
                ////        this.AddError(ErrorType.CannotAssignRHSToLHS);
                ////        return true;
                ////    }
                ////}

                if (!foundErrors)
                {
                    ClassDefinition lhsClass = (lhsType as ObjectExpressionType).DataType;
                    if (this.operatorDefined == "=" && lhsType.IsEqualTo(rhsType))
                    {
                        return false;
                    }

                    bool foundOperator = false;

                    foreach (MemberDefinition member in lhsClass.Members)
                    {
                        if (member is OperatorDefinition)
                        {
                            OperatorDefinition op = member as OperatorDefinition;
                            if (op.OperatorDefined == this.operatorDefined
                                && rhsType.IsEqualTo(op.Parameters[0].Type.GetExpressionType(scopeStack)))
                            {
                                foundOperator = true;
                            }
                        }
                    }

                    if (foundOperator == false)
                    {
                        this.AddError(ErrorType.NotAValidRHS, rhsType.GetName());
                        foundErrors = true;
                    }
                }
            }

            return foundErrors;
        }

        /// <summary>
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public override ExpressionType GetExpressionType(ScopeStack stack)
        {
            if (OperatorDefinition.NonOverloadableOperators.Contains(this.operatorDefined))
            {
                return new ObjectExpressionType(
                    CompilerService.Instance.ClassesList[Literal.Bool],
                    MemberStaticType.Normal);
            }
            else
            {
                ClassDefinition lhsType = (this.lhs.GetExpressionType(stack) as ObjectExpressionType).DataType;

                foreach (MemberDefinition member in lhsType.Members)
                {
                    if (member is OperatorDefinition)
                    {
                        OperatorDefinition op = member as OperatorDefinition;
                        if (op.OperatorDefined == this.operatorDefined)
                        {
                            return this.ExpressionType = op.Type.GetExpressionType(stack);
                        }
                    }
                }

                throw new Exception("Faulty Type. Should be handled in semantics.");
            }
        }

        /// <summary>
        /// Checks if this expression can be assigned to.
        /// </summary>
        /// <returns>True if this expression can be assigned to, false otherwise.</returns>
        public override bool IsAssignable()
        {
            return false;
        }
    }
}