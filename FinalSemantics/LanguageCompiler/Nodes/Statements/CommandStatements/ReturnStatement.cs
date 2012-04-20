namespace LanguageCompiler.Nodes.Statements.CommandStatements
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Semantics;

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
            MemberDefinition node = scopeStack.GetFunction();

            string retStatment, functionRetStatment;

            try
            {
                functionRetStatment = node.Type.GetExpressionType(scopeStack).GetName();
            }
            catch (System.Exception)
            {
                if (this.expression == null)
                {
                    return true;
                }

                if (this.expression != null)
                {
                    this.AddError(Errors.ErrorType.FunctionReturn, "void", string.Empty);
                    return false;
                }
            }

            retStatment = this.expression.GetExpressionType(scopeStack).GetName();

            if (node != null && this.expression == null)
            {
                functionRetStatment = node.Type.GetExpressionType(scopeStack).GetName();
                this.AddError(Errors.ErrorType.FunctionReturn, functionRetStatment, string.Empty);
                return false;
            }

            if (node.Type.GetExpressionType(scopeStack).GetName()
                == this.expression.GetExpressionType(scopeStack).GetName())
            {
                return true;
            }

            functionRetStatment = node.Type.GetExpressionType(scopeStack).GetName();
            this.AddError(Errors.ErrorType.FunctionReturn, functionRetStatment, retStatment);
            return false;
        }
    }
}