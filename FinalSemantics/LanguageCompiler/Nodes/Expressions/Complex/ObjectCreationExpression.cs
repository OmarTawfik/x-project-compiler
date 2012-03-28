namespace LanguageCompiler.Nodes.Expressions.Complex
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Holds all data related to a "ObjectCreationExpression" rule.
    /// </summary>
    public class ObjectCreationExpression : ExpressionNode
    {
        /// <summary>
        /// Type of object.
        /// </summary>
        private Identifier type;

        /// <summary>
        /// Arguments of constructor.
        /// </summary>
        private List<ExpressionNode> arguments = new List<ExpressionNode>();

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode args = new TreeNode("Constructor Arguments: Count = " + this.arguments.Count);
            foreach (BaseNode child in this.arguments)
            {
                args.Nodes.Add(child.GetGUINode());
            }

            TreeNode result = new TreeNode("Object Creation Expression");
            result.Nodes.Add(this.type.GetGUINode());
            result.Nodes.Add(args);
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.type = new Identifier();
            this.type.RecieveData(node.ChildNodes[1]);

            foreach (ParseTreeNode child in node.ChildNodes[3].ChildNodes)
            {
                this.arguments.Add(ExpressionsFactory.GetBaseExpr(child));
            }

            this.StartLocation = node.ChildNodes[0].Token.Location;
            this.EndLocation = node.ChildNodes[4].Token.Location;
        }

        /// <summary>
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public override ExpressionType GetExpressionType(ScopeStack stack)
        {
            return this.type.GetExpressionType(stack);
        }

        /// <summary>
        /// Checks if this expression can be assigned to.
        /// </summary>
        /// <returns>True if this expression can be assigned to, false otherwise.</returns>
        public override bool IsAssignable()
        {
            return false;
        }
    }
}