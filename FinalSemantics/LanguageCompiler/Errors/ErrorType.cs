namespace LanguageCompiler.Errors
{
    /// <summary>
    /// Enumeration for all semantic errors types.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// Syntax error returned from parser.
        /// </summary>
        SyntaxError,

        /// <summary>
        /// Internal error caused by an exception in the compiler.
        /// </summary>
        InternalError,

        /// <summary>
        /// More than one class with the same nam exists.
        /// </summary>
        MultipleTypesWithSameName,

        /// <summary>
        /// An identifier cannot have a value of a reserved word.
        /// </summary>
        IdentifierIsReservedWord,

        /// <summary>
        /// An identifier's name is too long.
        /// </summary>
        IdentifierNameTooLong,

        /// <summary>
        /// A cycle is found in the base list of a certain class.
        /// </summary>
        CyclicInheritence,

        /// <summary>
        /// Type not found error.
        /// </summary>
        TypeNotFound,

        /// <summary>
        /// A class cannot extend a concrete class.
        /// </summary>
        ConcreteBase,

        /// <summary>
        /// A screen cannot be labeled abstract or concrete.
        /// </summary>
        ScreenModifierNotNormal,

        /// <summary>
        /// A screen cannot inherit another type.
        /// </summary>
        ScreenCannotInherit,

        /// <summary>
        /// Multiple members with the same name exist within a class.
        /// </summary>
        MultipleMembersWithSameName,

        /// <summary>
        /// Class member has the same name as a type.
        /// </summary>
        MemberNameIsAType,

        /// <summary>
        /// This postfix operator cannot have parameters.
        /// </summary>
        PostfixOperatorsCannotHaveParameters,
    }
}
