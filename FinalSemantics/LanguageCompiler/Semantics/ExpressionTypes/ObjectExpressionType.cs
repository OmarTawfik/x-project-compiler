namespace LanguageCompiler.Semantics.ExpressionTypes
{
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.TopLevel;

    /// <summary>
    /// Represents the type of an expression that returns an object.
    /// </summary>
    public class ObjectExpressionType : ExpressionType
    {
        /// <summary>
        /// The data type of this expression.
        /// </summary>
        private ClassDefinition dataType;

        /// <summary>
        /// Indicates whether this expression is static.
        /// </summary>
        private MemberStaticType staticType;

        /// <summary>
        /// Initializes a new instance of the ObjectExpressionType class.
        /// </summary>
        /// <param name="dataType">The data type of this expression.</param>
        /// <param name="staticType">Indicates whether this expression is static.</param>
        public ObjectExpressionType(ClassDefinition dataType, MemberStaticType staticType)
        {
            this.dataType = dataType;
            this.staticType = staticType;
        }

        /// <summary>
        /// Gets a value indicating whether this expression is static.
        /// </summary>
        public MemberStaticType StaticType
        {
            get { return this.staticType; }
        }

        /// <summary>
        /// Gets the data type of this expression.
        /// </summary>
        public ClassDefinition DataType
        {
            get { return this.dataType; }
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
                return this.dataType.Name == (other as ObjectExpressionType).dataType.Name;
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
            return this.dataType.Name.Text;
        }
    }
}