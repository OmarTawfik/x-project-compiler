namespace LanguageCompiler.Nodes.Expressions.Basic
{
    using System;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.TopLevel;
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
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public override ExpressionType GetExpressionType(ScopeStack stack)
        {
            ClassDefinition lhsType = (this.lhs.GetExpressionType(stack) as ObjectExpressionType).DataType;

            foreach (MemberDefinition member in lhsType.Members)
            {
                if (member is OperatorDefinition)
                {
                    OperatorDefinition op = member as OperatorDefinition;
                    if (op.OperatorDefined == this.operatorDefined)
                    {
                        return op.Type.GetExpressionType(stack);
                    }
                }
            }

            throw new Exception("Faulty Type. Should be handled in semantics.");
        }
    }
}
