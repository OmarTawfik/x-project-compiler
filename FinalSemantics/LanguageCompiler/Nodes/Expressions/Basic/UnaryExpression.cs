namespace LanguageCompiler.Nodes.Expressions.Basic
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Types;

    /// <summary>
    /// Holds all data related to a "UnaryExpression" rule.
    /// </summary>
    public class UnaryExpression : BaseNode
    {
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
            TreeNode result = new TreeNode(this.operatorDefined + " Unary Expression");
            result.Nodes.Add(this.rhs.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.operatorDefined = node.ChildNodes[0].Token.Text;
            this.rhs = ExpressionsFactory.GetUnaryExpr(node.ChildNodes[1]);

            this.StartLocation = node.ChildNodes[0].Token.Location;
            this.EndLocation = this.rhs.EndLocation;
        }
    }
}
