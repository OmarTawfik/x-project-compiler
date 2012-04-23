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
        /// Cannot assign the RHS value to the LHS value.
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

        /// <summary>
        /// Variable's Name same as Function's Name.
        /// </summary>
        VariableSameFunction,

        /// <summary>
        /// User defined backend class.
        /// </summary>
        UserDefinedBackendClass,

        /// <summary>
        /// Static constructors must be public.
        /// </summary>
        StaticConstructorsMustBePublic,

        /// <summary>
        /// Class {0} doesn't contain a member with the name {1}.
        /// </summary>
        NoMemberWithThisName,

        /// <summary>
        /// Member {0} is inaccessible due to its protection level.
        /// </summary>
        InaccessibleDueToProtectionLevel,

        /// <summary>
        /// Cannot call method {0} because it is abstract.
        /// </summary>
        CallToAbstractFunction,

        /// <summary>
        /// Cannot call member {0} from a class quantifier.
        /// </summary>
        CallNormalMembersFromClass,

        /// <summary>
        /// Cannot call member {0} from an object quantifier.
        /// </summary>
        CallStaticMembersFromObject,

        /// <summary>
        /// Function {0} doesn't accept the argument of type {1}.
        /// </summary>
        InvalidParameterType,

        /// <summary>
        /// Function {0} doesn't accept {1} arguments.
        /// </summary>
        WrongNumberOfParameters,

        /// <summary>
        /// LHS of this expression isn't a function.
        /// </summary>
        LHSNotAFunction,

        /// <summary>
        /// Cannot find a suitable constructor for type {0} that takes these arguments.
        /// </summary>
        NoSuitableConstructor,

        /// <summary>
        /// A constructor must return the containing type.
        /// </summary>
        ConstructorMustReturnContainingType,

        /// <summary>
        /// Not all control paths return a value.
        /// </summary>
        NotAllControlPathsReturnAValue,

        /// <summary>
        /// Unreachable code detected.
        /// </summary>
        UnreachableCodeDetected,

        /// <summary>
        /// An assignment or a postfix operator must return the containing type.
        /// </summary>
        OperatorMustReturnContainingType,

        /// <summary>
        /// A variable with the name {0} doesn't exist in the current scope.
        /// </summary>
        VariableDoesnotExist,
    }
}