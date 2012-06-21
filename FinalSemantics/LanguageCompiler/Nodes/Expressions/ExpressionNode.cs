namespace LanguageCompiler.Nodes.Expressions
{
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Acts a base for all expression nodes.
    /// </summary>
    public abstract class ExpressionNode : BaseNode
    {
        /// <summary>
        /// The expression type of this node.
        /// </summary>
        private ExpressionType expressionType;

        /// <summary>
        /// Gets or sets the expression type of this node.
        /// </summary>
        public ExpressionType ExpressionType
        {
            get { return this.expressionType; }
            protected set { this.expressionType = value; }
        }

        /// <summary>
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public abstract ExpressionType GetExpressionType(ScopeStack stack);

        /// <summary>
        /// Checks if this expression can be assigned to.
        /// </summary>
        /// <returns>True if this expression can be assigned to, false otherwise.</returns>
        public abstract bool IsAssignable();

        /// <summary>
        /// Checks if a statement or block of code returns a value.
        /// </summary>
        /// <returns>True if it returns a value, false otherwise.</returns>
        public override bool ReturnsAValue()
        {
            return false;
        }
    }
}