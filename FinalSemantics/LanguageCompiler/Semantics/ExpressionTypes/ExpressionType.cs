namespace LanguageCompiler.Semantics.ExpressionTypes
{
    /// <summary>
    /// Acts as a base for all expression types.
    /// </summary>
    public abstract class ExpressionType
    {
        /// <summary>
        /// Checks two expression types for equality.
        /// </summary>
        /// <param name="other">Other type to check.</param>
        /// <returns>True if both are equal, false otherwise.</returns>
        public abstract bool IsEqualTo(ExpressionType other);
    }
}
