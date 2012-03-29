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
        /// This item was already defined elsewhere in code.
        /// </summary>
        ItemAlreadyDefined,

        /// <summary>
        /// Class member has the same name as a type.
        /// </summary>
        MemberNameIsAType,

        /// <summary>
        /// This operator has an invalid number of parameters.
        /// </summary>
        OperatorInvalidParameters,

        /// <summary>
        /// A field cannot be marked abstract, virtual, or override.
        /// </summary>
        FieldInvalidModifier,

        /// <summary>
        /// A method or operator marked as abstract cannot have a body.
        /// </summary>
        AbstractMemberHasBody,

        /// <summary>
        /// Cannot overload this operator.
        /// </summary>
        OperatorNotOverloadable,

        /// <summary>
        /// All Non Abstract members must declare a body.
        /// </summary>
        MissingBodyOfNonAbstractMember,

        /// <summary>
        /// This operator has an invalid return type.
        /// </summary>
        OperatorInvalidReturnType,

        /// <summary>
        /// An incorrect escape sequence found in char or string literals.
        /// </summary>
        IncorrectEscapeSequence,

        /// <summary>
        /// An incorrect number found in numerical literal.
        /// </summary>
        IncorrectNumberLiteral,

        /// <summary>
        /// Expression assigned to this variable doesn't match its type.
        /// </summary>
        ExpressionDoesnotMatchType,

        /// <summary>
        /// Only assignment, call, increment, decrement, and new object expressions can be used as a statement.
        /// </summary>
        InvalidExpressionStatement,

        /// <summary>
        /// Break and Continue Statements must appear within a loop.
        /// </summary>
        StatementMustAppearInLoop,

        /// <summary>
        /// This expression must be a boolean type.
        /// </summary>
        ExpressionNotBoolean,

        /// <summary>
        /// Types of both expressions of the embedded if statement don't match.
        /// </summary>
        EmbeddedIfTypeMismatch,

        /// <summary>
        /// The expression on the LHS cannot be assigned to.
        /// </summary>
        CannotAssignTo,

        /// <summary>
        /// Cannot use operator with this LHS.
        /// </summary>
        NotAValidLHS,

        /// <summary>
        /// No suitable operator found that takes RHS of this type.
        /// </summary>
        NotAValidRHS,

        /// <summary>
        /// Cannot assign the RHS value to the RHS value.
        /// </summary>
        CannotAssignRHSToLHS,

        /// <summary>
        /// Cannot apply postfix operator on a temp value.
        /// </summary>
        PostfixOnTemp,

        /// <summary>
        /// Function return type mismatch with return statement.
        /// </summary>
        FunctionReturn,

        /// <summary>
        /// An identifier cannot contain underscores.
        /// </summary>
        UnderscoreInIdentifier,
    }
}