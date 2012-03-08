namespace LanguageCompiler.Nodes.Expressions.Basic
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "BinaryExpression" rule.
    /// </summary>
    public class BinaryExpression : BaseNode
    {
        /// <summary>
        /// LHS of expression.
        /// </summary>
        private BaseNode lhs;

        /// <summary>
        /// Operator of expression.
        /// </summary>
        private string operatorDefined;

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
            this.lhs = ExpressionsFactory.GetBinaryExpr(node.ChildNodes[0]);
            this.operatorDefined = node.ChildNodes[1].Token.Text;
            this.rhs = ExpressionsFactory.GetBinaryExpr(node.ChildNodes[2]);

            this.StartLocation = this.lhs.StartLocation;
            this.EndLocation = this.rhs.EndLocation;
        }
    }
}
