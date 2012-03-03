namespace LanguageCompiler.Nodes.Expressions.Basic
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Types;

    /// <summary>
    /// Holds all data related to a "EmbeddedIf" rule.
    /// </summary>
    public class EmbeddedIf : BaseNode
    {
        /// <summary>
        /// Condition of this embedded if.
        /// </summary>
        private BaseNode condition;

        /// <summary>
        /// Expression to return if condition is true.
        /// </summary>
        private BaseNode trueChoice;

        /// <summary>
        /// Expression to return if condition is false.
        /// </summary>
        private BaseNode falseChoice;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Embedded If");
            result.Nodes.Add(this.condition.GetGUINode());
            result.Nodes.Add(this.trueChoice.GetGUINode());
            result.Nodes.Add(this.falseChoice.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.condition = ExpressionsFactory.GetBinaryExpr(node.ChildNodes[0]);
            this.trueChoice = ExpressionsFactory.GetBaseExpr(node.ChildNodes[1].ChildNodes[1]);
            this.falseChoice = ExpressionsFactory.GetBaseExpr(node.ChildNodes[1].ChildNodes[3]);

            this.StartLocation = this.condition.StartLocation;
            this.EndLocation = node.ChildNodes[1].ChildNodes[4].Token.Location;
        }
    }
}
