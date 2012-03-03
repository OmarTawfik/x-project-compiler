namespace LanguageCompiler.Nodes.Statements.CommandStatements
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Expressions;

    /// <summary>
    /// Holds all data related to a "ReturnStatement" rule.
    /// </summary>
    public class ReturnStatement : BaseNode
    {
        /// <summary>
        /// The expression returned by this statement.
        /// </summary>
        private BaseNode expression;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Return Statement");
            if (this.expression != null)
            {
                result.Nodes.Add(this.expression.GetGUINode());
            }

            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            if (node.ChildNodes[1].ChildNodes.Count > 0)
            {
                this.expression = ExpressionsFactory.GetBaseExpr(node.ChildNodes[1].ChildNodes[0]);
            }

            this.StartLocation = node.ChildNodes[0].Token.Location;
            this.EndLocation = node.ChildNodes[2].Token.Location;
        }
    }
}
