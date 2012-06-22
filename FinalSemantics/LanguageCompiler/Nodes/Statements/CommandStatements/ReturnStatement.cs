namespace LanguageCompiler.Nodes.Statements.CommandStatements
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Holds all data related to a "ReturnStatement" rule.
    /// </summary>
    public class ReturnStatement : BaseNode
    {
        /// <summary>
        /// The expression returned by this statement.
        /// </summary>
        private ExpressionNode expression;

        /// <summary>
        /// Gets the expression for the return statement.
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
            TreeNode result = new TreeNode("Return Statement");
            if (this.expression != null)
            {
                result.Nodes.Add(this.expression.GetGUINode());
            }

            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            if (node.ChildNodes[1].ChildNodes.Count > 0)
            {
                this.expression = ExpressionsFactory.GetBaseExpr(node.ChildNodes[1].ChildNodes[0]);
            }

            this.StartLocation = node.ChildNodes[0].Token.Location;
            this.EndLocation = node.ChildNodes[2].Token.Location;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            string functionType = scopeStack.GetFunction().Type.Text;

            if (this.expression == null)
            {
                if (functionType == "void")
                {
                    return false;
                }
                else
                {
                    this.AddError(ErrorType.FunctionReturn, functionType, "void");
                    return true;
                }
            }
            else
            {
                if (this.expression.CheckSemanticErrors(scopeStack))
                {
                    return true;
                }

                string expressionType = (this.expression.GetExpressionType(scopeStack) as ObjectExpressionType).DataType.Name.Text;
                if (functionType == expressionType)
                {
                    return false;
                }
                else
                {
                    this.AddError(ErrorType.FunctionReturn, functionType, expressionType);
                    return true;
                }
            }
        }

        /// <summary>
        /// Checks if a statement or block of code returns a value.
        /// </summary>
        /// <returns>True if it returns a value, false otherwise.</returns>
        public override bool ReturnsAValue()
        {
            return true;
        }
    }
}