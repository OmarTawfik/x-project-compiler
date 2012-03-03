namespace LanguageCompiler.Nodes.Expressions.Basic
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Types;

    /// <summary>
    /// Holds all data related to a "PostfixExpression" rule.
    /// </summary>
    public class PostfixExpression : BaseNode
    {
        /// <summary>
        /// Operator of expression.
        /// </summary>
        private string operatorDefined;

        /// <summary>
        /// LHS of expression.
        /// </summary>
        private BaseNode lhs;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode(this.operatorDefined + " Postfix Expression");
            result.Nodes.Add(this.lhs.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.lhs = ExpressionsFactory.GetPostfixExpr(node.ChildNodes[0]);
            this.operatorDefined = node.ChildNodes[1].Token.Text;

            this.StartLocation = this.lhs.StartLocation;
            this.EndLocation = node.ChildNodes[1].Token.Location;
        }
    }
}
