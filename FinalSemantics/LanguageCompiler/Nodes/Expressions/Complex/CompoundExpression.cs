namespace LanguageCompiler.Nodes.Expressions.Complex
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;

    /// <summary>
    /// Holds all data related to a "CompoundExpression" rule.
    /// </summary>
    public class CompoundExpression : BaseNode
    {
        /// <summary>
        /// expression in LHS.
        /// </summary>
        private BaseNode lhs;

        /// <summary>
        /// RHS of expression.
        /// </summary>
        private BaseNode rhs;

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
            this.lhs = ExpressionsFactory.GetPostfixExpr(node.ChildNodes[0]);
            this.rhs = ExpressionsFactory.GetPostfixExpr(node.ChildNodes[1].ChildNodes[1]);

            this.StartLocation = this.lhs.StartLocation;
            this.EndLocation = this.rhs.EndLocation;
        }
    }
}
