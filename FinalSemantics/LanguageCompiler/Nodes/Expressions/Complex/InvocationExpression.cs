namespace LanguageCompiler.Nodes.Expressions.Complex
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "InvocationExpression" rule.
    /// </summary>
    public class InvocationExpression : BaseNode
    {
        /// <summary>
        /// expression in LHS.
        /// </summary>
        private BaseNode lhs;

        /// <summary>
        /// Function name of this expression.
        /// </summary>
        private Identifier functionName;

        /// <summary>
        /// arguments of expression.
        /// </summary>
        private List<BaseNode> arguments = new List<BaseNode>();

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Invocation Expression");
            result.Nodes.Add(this.lhs.GetGUINode());
            result.Nodes.Add(this.functionName.GetGUINode());
            TreeNode args = new TreeNode("Arguments: Count = " + this.arguments.Count);
            foreach (BaseNode child in this.arguments)
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
            this.lhs = ExpressionsFactory.GetPrimaryExpr(node.ChildNodes[0]);
            this.functionName = new Identifier();
            this.functionName.RecieveData(node.ChildNodes[1].ChildNodes[0]);

            foreach (ParseTreeNode child in node.ChildNodes[1].ChildNodes[2].ChildNodes)
            {
                this.arguments.Add(ExpressionsFactory.GetBaseExpr(child));
            }

            this.StartLocation = this.lhs.StartLocation;
            this.EndLocation = node.ChildNodes[1].ChildNodes[3].Token.Location;
        }
    }
}
