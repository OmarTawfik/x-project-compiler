namespace LanguageCompiler.Errors
{
    using System.Collections.Generic;
    using Irony;
    using LanguageCompiler.Nodes;

    /// <summary>
    /// Produces correct error strings for different compiler errors.
    /// </summary>
    public static class ErrorsFactory
    {
        /// <summary>
        /// Holds all string representations for error types.
        /// </summary>
        private static Dictionary<ErrorType, string> errorsDic = new Dictionary<ErrorType, string>();

        /// <summary>
        /// Initializes static members of the ErrorsFactory class.
        /// </summary>
        static ErrorsFactory()
        {
            //// Level One: Parser Errors and Others
            {
                ErrorsFactory.errorsDic.Add(ErrorType.SyntaxError, "The parser failed to produce a valid parse tree.");
                ErrorsFactory.errorsDic.Add(ErrorType.InternalError, "An internal error happened in the compiler. Cannot recover");
                ErrorsFactory.errorsDic.Add(ErrorType.MultipleTypesWithSameName, "There exists more than one class with the name {0}");
                ErrorsFactory.errorsDic.Add(ErrorType.TypeNotFound, "The type {0} cannot be found");
                ErrorsFactory.errorsDic.Add(ErrorType.ItemAlreadyDefined, "An item with the name {0} was already defined.");
            }

            //// Level Two: Class Definitions
            {
                ErrorsFactory.errorsDic.Add(ErrorType.CyclicInheritence, "The class {0} contains a cyclic inheritence list.");
                ErrorsFactory.errorsDic.Add(ErrorType.ConcreteBase, "The base of class {0} is concrete.");
                ErrorsFactory.errorsDic.Add(ErrorType.ScreenModifierNotNormal, "Screen {0} cannot be labeled abstract or concrete.");
                ErrorsFactory.errorsDic.Add(ErrorType.ScreenCannotInherit, "Screen {0} cannot inherit another type.");
                ErrorsFactory.errorsDic.Add(ErrorType.UserDefinedBackendClass, "User defined a class or screen {0} with the keyword \"Backend\".");
            }

            //// Level Three: Member Definitions
            {
                ErrorsFactory.errorsDic.Add(ErrorType.FieldInvalidModifier, "Field cannot be marked abstract, virtual, or override.");
                ErrorsFactory.errorsDic.Add(ErrorType.AbstractMemberHasBody, "Method or Operator {0} is marked as abstract and cannot have a body.");
                ErrorsFactory.errorsDic.Add(ErrorType.MissingBodyOfNonAbstractMember, "Non Abstract member {0} must declare a body.");
                ErrorsFactory.errorsDic.Add(ErrorType.MemberNameIsAType, "Member {0} has the same name as an existing type.");
                ErrorsFactory.errorsDic.Add(ErrorType.OperatorInvalidParameters, "This Operator {0} has an invalid number of parameters");
                ErrorsFactory.errorsDic.Add(ErrorType.OperatorNotOverloadable, "Cannot overload the operator {0}.");
                ErrorsFactory.errorsDic.Add(ErrorType.OperatorInvalidReturnType, "Operator {0} has an invalid return type.");
                ErrorsFactory.errorsDic.Add(ErrorType.StaticConstructorsMustBePublic, "Static constructor {0} must be public.");
            }

            //// Level Four: Members With Scope
            {
                ErrorsFactory.errorsDic.Add(ErrorType.ConstructorMustReturnContainingType, "A constructor must return the containing type.");
                ErrorsFactory.errorsDic.Add(ErrorType.UnreachableCodeDetected, "Unreachable code detected.");
                ErrorsFactory.errorsDic.Add(ErrorType.NotAllControlPathsReturnAValue, "Not all control paths return a value.");
                ErrorsFactory.errorsDic.Add(ErrorType.OperatorMustReturnContainingType, "An assignment or a postfix operator must return the containing type.");
            }

            //// Level Five: Statements
            {
                ErrorsFactory.errorsDic.Add(ErrorType.ExpressionDoesnotMatchType, "Expression assigned to this variable doesn't match its type.");
                ErrorsFactory.errorsDic.Add(ErrorType.StatementMustAppearInLoop, "Break and Continue Statements must appear within a loop.");
                ErrorsFactory.errorsDic.Add(ErrorType.InvalidExpressionStatement, "Only assignment, call, increment, decrement, and new object expressions can be used as a statement.");
                ErrorsFactory.errorsDic.Add(ErrorType.ExpressionNotBoolean, "This expression must be a boolean type.");
                ErrorsFactory.errorsDic.Add(ErrorType.FunctionReturn, "Function return type {0} doesn't match return statement type {1}.");
                ErrorsFactory.errorsDic.Add(ErrorType.VariableSameFunction, "The Variable ({0}) and The Function names are the same");
            }

            //// Level Six: Expressions
            {
                ErrorsFactory.errorsDic.Add(ErrorType.IdentifierIsReservedWord, "The identifier {0} cannot have a value of a reserved word.");
                ErrorsFactory.errorsDic.Add(ErrorType.IdentifierNameTooLong, "This identifier {0}'s name is too long.");
                ErrorsFactory.errorsDic.Add(ErrorType.IncorrectEscapeSequence, "Incorrect escape sequence {0} found in char or string literal.");
                ErrorsFactory.errorsDic.Add(ErrorType.UnderscoreInIdentifier, "The identifier {0} cannot contain underscores.");
                ErrorsFactory.errorsDic.Add(ErrorType.IncorrectNumberLiteral, "Incorrect number {0} found in numerical literal.");
                ErrorsFactory.errorsDic.Add(ErrorType.EmbeddedIfTypeMismatch, "Types of both expressions of the embedded if statement don't match.");
                ErrorsFactory.errorsDic.Add(ErrorType.CannotAssignTo, "The expression on the LHS cannot be assigned to.");
                ErrorsFactory.errorsDic.Add(ErrorType.NotAValidLHS, "Cannot use operator {0} with this LHS.");
                ErrorsFactory.errorsDic.Add(ErrorType.NotAValidRHS, "No suitable operator found that takes RHS of type {0}.");
                ErrorsFactory.errorsDic.Add(ErrorType.CannotAssignRHSToLHS, "Cannot assign the RHS value to the LHS value.");
                ErrorsFactory.errorsDic.Add(ErrorType.PostfixOnTemp, "Cannot apply postfix operator on a temp value.");
                ErrorsFactory.errorsDic.Add(ErrorType.NoMemberWithThisName, "Class {0} doesn't contain a member with the name {1}.");
                ErrorsFactory.errorsDic.Add(ErrorType.InaccessibleDueToProtectionLevel, "Member {0} is inaccessible due to its protection level.");
                ErrorsFactory.errorsDic.Add(ErrorType.CallToAbstractFunction, "Cannot call method {0} because it is abstract.");
                ErrorsFactory.errorsDic.Add(ErrorType.CallNormalMembersFromClass, "Cannot call member {0} from a class quantifier.");
                ErrorsFactory.errorsDic.Add(ErrorType.CallStaticMembersFromObject, "Cannot call member {0} from an object quantifier.");
                ErrorsFactory.errorsDic.Add(ErrorType.InvalidParameterType, "Function {0} doesn't accept the argument of type {1}.");
                ErrorsFactory.errorsDic.Add(ErrorType.WrongNumberOfParameters, "Function {0} doesn't accept {1} arguments.");
                ErrorsFactory.errorsDic.Add(ErrorType.LHSNotAFunction, "LHS of this expression isn't a function.");
                ErrorsFactory.errorsDic.Add(ErrorType.NoSuitableConstructor, "Cannot find a suitable constructor for type {0} that takes these arguments.");
                ErrorsFactory.errorsDic.Add(ErrorType.VariableDoesnotExist, "A variable with the name {0} doesn't exist in the current scope.");
            }
        }

        /// <summary>
        /// Gets a string representing a syntax error.
        /// </summary>
        /// <param name="message">Log Message returned from parser.</param>
        /// <returns>The formed string.</returns>
        public static CompilerError SyntaxError(LogMessage message)
        {
            return new CompilerError(
                message.Location,
                message.Location,
                message.Message);
        }

        /// <summary>
        /// Gets a string representing a semantic error.
        /// </summary>
        /// <param name="type">Type of semantic error.</param>
        /// <param name="node">Node where error was found.</param>
        /// <param name="parameters">Parameters of string description.</param>
        /// <returns>The formed string.</returns>
        public static CompilerError SemanticError(ErrorType type, BaseNode node, params string[] parameters)
        {
            string error = string.Empty;
            if (parameters.Length == 0)
            {
                error = ErrorsFactory.errorsDic[type];
            }
            else
            {
                error = string.Format(ErrorsFactory.errorsDic[type], parameters);
            }

            return new CompilerError(node.StartLocation, node.EndLocation, error);
        }
    }
}