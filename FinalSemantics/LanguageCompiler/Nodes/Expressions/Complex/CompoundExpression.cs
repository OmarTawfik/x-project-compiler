namespace LanguageCompiler.Nodes.Expressions.Complex
{
    using System;
    using System.Windows.Forms;
    using Irony.Parsing;
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
        /// expression in LHS.
        /// </summary>
        private ExpressionNode lhs;

        /// <summary>
        /// RHS of expression.
        /// </summary>
        private Identifier rhs;

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
    }
}