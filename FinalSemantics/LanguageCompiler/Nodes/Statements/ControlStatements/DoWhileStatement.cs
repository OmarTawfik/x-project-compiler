namespace LanguageCompiler.Nodes.Statements.ControlStatements
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "DoWhileStatement" rule.
    /// </summary>
    public class DoWhileStatement : BaseNode
    {
        /// <summary>
        /// The expression for this do while loop.
        /// </summary>
        private ExpressionNode expression;

        /// <summary>
        /// The body for this do while loop.
        /// </summary>
        private Block body;

        /// <summary>
        /// Gets the block statements.
        /// </summary>
        public Block Block
        {
            get { return this.body; }
        }

        /// <summary>
        /// Gets the expression of the do while statement.
        /// </summary>
        public ExpressionNode Expression
        {
            get { return this.expression; }
        }

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

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            bool foundErrors = false;

            scopeStack.AddLevel(ScopeType.Loop, this);
            foundErrors |= this.expression.CheckSemanticErrors(scopeStack);
            foundErrors |= this.body.CheckSemanticErrors(scopeStack);
            scopeStack.DeleteLevel();

            if (foundErrors)
            {
                return foundErrors;
            }

            if (this.expression.GetExpressionType(scopeStack).IsEqualTo(Literal.ConstructExpression(Literal.Bool)) == false)
            {
                this.AddError(ErrorType.ExpressionNotBoolean);
                foundErrors = true;
            }

            return foundErrors;
        }
    }
}