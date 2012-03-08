namespace LanguageCompiler.Nodes.Expressions.Complex
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "ArrayExpression" rule.
    /// </summary>
    public class ArrayExpression : BaseNode
    {
        /// <summary>
        /// expression in LHS.
        /// </summary>
        private BaseNode lhs;

        /// <summary>
        /// Indexes of expression.
        /// </summary>
        private List<BaseNode> indexes = new List<BaseNode>();

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Array Expression");
            result.Nodes.Add(this.lhs.GetGUINode());

            TreeNode args = new TreeNode("Indexes: Count = " + this.indexes.Count);
            foreach (BaseNode child in this.indexes)
            {
                args.Nodes.Add(child.GetGUINode());
            }

            result.Nodes.Add(args);
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.lhs = ExpressionsFactory.GetPostfixExpr(node.ChildNodes[0]);
            foreach (ParseTreeNode child in node.ChildNodes[1].ChildNodes[1].ChildNodes)
            {
                this.indexes.Add(ExpressionsFactory.GetBaseExpr(child));
            }

            this.StartLocation = this.lhs.StartLocation;
            this.EndLocation = node.ChildNodes[1].ChildNodes[2].Token.Location;
        }
    }
}
