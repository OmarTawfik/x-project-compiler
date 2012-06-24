namespace LanguageCompiler.Semantics.ExpressionTypes
{
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.TopLevel;

    /// <summary>
    /// Represents the type of an expression that returns an list.
    /// </summary>
    public class ListExpressionType : ExpressionType
    {
        /// <summary>
        /// Initializes a new instance of the ListExpressionType class.
        /// </summary>
        public ListExpressionType()
        {
        }

        /// <summary>
        /// Checks two expression types for equality.
        /// </summary>
        /// <param name="other">Other type to check.</param>
        /// <returns>True if both are equal, false otherwise.</returns>
        public override bool IsEqualTo(ExpressionType other)
        {
            if (other is ObjectExpressionType)
            {
                ObjectExpressionType objectType = other as ObjectExpressionType;
                return objectType.DataType.Name.Text.EndsWith("_list");
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the name of this expression type.
        /// </summary>
        /// <returns>The name of this expression type.</returns>
        public override string GetName()
        {
            return "list";
        }
    }
}