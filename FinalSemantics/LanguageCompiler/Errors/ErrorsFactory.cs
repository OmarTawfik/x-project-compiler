namespace LanguageCompiler.Errors
{
    using System;
    using System.Collections.Generic;
    using Irony;
    using Irony.Parsing;
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
            //// No error string for ErrorType.SyntaxError.
            //// No error string for ErrorType.InternalError.
            ErrorsFactory.errorsDic.Add(ErrorType.CyclicInheritence, "The class {0} contains a cyclic inheritence list.");
            ErrorsFactory.errorsDic.Add(ErrorType.MultipleTypesWithSameName, "There exists more than one class with the name {0}");
            ErrorsFactory.errorsDic.Add(ErrorType.IdentifierIsReservedWord, "The identifier {0} cannot have a value of a reserved word.");
            ErrorsFactory.errorsDic.Add(ErrorType.IdentifierNameTooLong, "This identifier {0}'s name is too long.");
            ErrorsFactory.errorsDic.Add(ErrorType.TypeNotFound, "The type {0} cannot be found");

            ErrorsFactory.errorsDic.Add(ErrorType.ConcreteBase, "The base of class {0} is concrete.");
            ErrorsFactory.errorsDic.Add(ErrorType.ScreenModifierNotNormal, "Screen {0} cannot be labeled abstract or concrete.");
            ErrorsFactory.errorsDic.Add(ErrorType.ScreenCannotInherit, "Screen {0} cannot inherit another type.");
            ErrorsFactory.errorsDic.Add(ErrorType.MultipleMembersWithSameName, "Multiple members with the name {0} exist within class {1}.");

            ErrorsFactory.errorsDic.Add(ErrorType.MemberNameIsAType, "Member {0} has the same name as an existing type.");
            ErrorsFactory.errorsDic.Add(ErrorType.PostfixOperatorsCannotHaveParameters, "Postfix Operator {0} cannot have parameters");
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
