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
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public abstract ExpressionType GetExpressionType(ScopeStack stack);
    }
}
