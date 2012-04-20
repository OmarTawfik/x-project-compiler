namespace LanguageCompiler.Nodes.Expressions.Complex
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Holds all data related to a "EmptyArrayExpression" rule.
    /// </summary>
    public class ArrayCreationExpression : ExpressionNode
    {
        /// <summary>
        /// Type of array.
        /// </summary>
        private Identifier type;

        /// <summary>
        /// Sizes of its indices.
        /// </summary>
        private List<ExpressionNode> sizes = new List<ExpressionNode>();

        /// <summary>
        /// Gets the type identifier.
        /// </summary>
        public Identifier Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the sizes of the created array.
        /// </summary>
        public List<ExpressionNode> Sizes
        {
            get { return this.sizes; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode args = new TreeNode("Indices Sizes: Count = " + this.sizes.Count);
            foreach (BaseNode child in this.sizes)
            {
                args.Nodes.Add(child.GetGUINode());
            }

            TreeNode result = new TreeNode("Empty Array Expression");
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
                this.sizes.Add(ExpressionsFactory.GetBaseExpr(child));
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
            return new ArrayExpressionType(this.type.GetExpressionType(stack), this.sizes.Count);
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