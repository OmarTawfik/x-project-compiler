namespace LanguageCompiler.Nodes.Statements.ControlStatements
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Expressions;

    /// <summary>
    /// Holds all data related to a "WhileStatement" rule.
    /// </summary>
    public class WhileStatement : BaseNode
    {
        /// <summary>
        /// The expression for this while loop.
        /// </summary>
        private BaseNode expression;

        /// <summary>
        /// The body for this while loop.
        /// </summary>
        private Block body;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("While Statement");
            result.Nodes.Add(this.expression.GetGUINode());
            if (this.body != null)
            {
                result.Nodes.Add(this.body.GetGUINode());
            }
            else
            {
                result.Nodes.Add("No Body!");
            }

            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.expression = ExpressionsFactory.GetBaseExpr(node.ChildNodes[2]);
            if (node.ChildNodes[4].Term.Name == LanguageGrammar.Block.Name)
            {
                this.body = new Block();
                this.body.RecieveData(node.ChildNodes[4]);
            }
            else
            {
                this.body = null;
            }

            this.StartLocation = node.ChildNodes[0].Token.Location;
            this.StartLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
        }
    }
}
