namespace LanguageCompiler.Nodes.Statements.ControlStatements
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Expressions;

    /// <summary>
    /// Holds all data related to a "DoWhileStatement" rule.
    /// </summary>
    public class DoWhileStatement : BaseNode
    {
        /// <summary>
        /// The expression for this do while loop.
        /// </summary>
        private BaseNode expression;

        /// <summary>
        /// The body for this do while loop.
        /// </summary>
        private Block body;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Do While Statement");
            result.Nodes.Add(this.expression.GetGUINode());
            result.Nodes.Add(this.body.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.body = new Block();
            this.body.RecieveData(node.ChildNodes[1]);
            this.expression = ExpressionsFactory.GetBaseExpr(node.ChildNodes[4]);

            this.StartLocation = node.ChildNodes[0].Token.Location;
            this.StartLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
        }
    }
}
