namespace LanguageCompiler.Nodes.Statements
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Nodes.Expressions.Basic;
    using LanguageCompiler.Nodes.Expressions.Complex;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "ExpressionStatement" rule.
    /// </summary>
    public class ExpressionStatement : BaseNode
    {
        /// <summary>
        /// Expression included in this statement.
        /// </summary>
        private BaseNode expression;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Expression Statement");
            result.Nodes.Add(this.expression.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.expression = ExpressionsFactory.GetBaseExpr(node.ChildNodes[0]);

            this.StartLocation = this.expression.StartLocation;
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
            foundErrors |= this.expression.CheckSemanticErrors(scopeStack);

            if (this.expression is AssignmentExpression == false
                && this.expression is InvocationExpression == false
                && this.expression is PostfixExpression == false
                && this.expression is ObjectCreationExpression == false)
            {
                this.AddError(ErrorType.InvalidExpressionStatement);
                foundErrors = true;
            }

            return foundErrors;
        }
    }
}
