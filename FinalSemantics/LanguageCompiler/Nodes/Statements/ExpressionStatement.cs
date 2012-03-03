namespace LanguageCompiler.Nodes.Statements
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Expressions;

    /// <summary>
    /// Holds all data related to a "ExpressionStatement" rule.
    /// </summary>
    public class ExpressionStatement : BaseNode
    {
        /// <summary>
        /// Expression included in this statement.
        /// </summary>
        private BaseNode expression;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Expression Statement");
            result.Nodes.Add(this.expression.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.expression = ExpressionsFactory.GetBaseExpr(node.ChildNodes[0]);

            this.StartLocation = this.expression.StartLocation;
            this.StartLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
        }
    }
}
