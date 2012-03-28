namespace LanguageCompiler.Semantics.ExpressionTypes
{
    using LanguageCompiler.Nodes.ClassMembers;

    /// <summary>
    /// Represents the type of an expression that returns a function.
    /// </summary>
    public class MethodExpressionType : ExpressionType
    {
        /// <summary>
        /// The method definition of this expression.
        /// </summary>
        private MethodDefinition method;

        /// <summary>
        /// Initializes a new instance of the MethodExpressionType class.
        /// </summary>
        /// <param name="method">The method definition of this expression.</param>
        public MethodExpressionType(MethodDefinition method)
        {
            this.method = method;
        }

        /// <summary>
        /// Gets the method definition of this expression.
        /// </summary>
        public MethodDefinition Method
        {
            get { return this.method; }
        }

        /// <summary>
        /// Checks two expression types for equality.
        /// </summary>
        /// <param name="other">Other type to check.</param>
        /// <returns>True if both are equal, false otherwise.</returns>
        public override bool IsEqualTo(ExpressionType other)
        {
            if (other is MethodExpressionType)
            {
                MethodDefinition otherMethod = (other as MethodExpressionType).method;
                return this.method.Name == otherMethod.Name
                    && this.method.Parent.Name == otherMethod.Parent.Name;
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
            return this.method.Name.Text;
        }
    }
}