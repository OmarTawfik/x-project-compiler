namespace LanguageCompiler.Nodes.Types
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Holds all data related to a "Literal" rule.
    /// </summary>
    public class Literal : ExpressionNode
    {
        /// <summary>
        /// The name of the char data type.
        /// </summary>
        public const string Char = "char";

        /// <summary>
        /// The name of the string data type.
        /// </summary>
        public const string String = "string";

        /// <summary>
        /// The name of the double data type.
        /// </summary>
        public const string Double = "double";

        /// <summary>
        /// The name of the float data type.
        /// </summary>
        public const string Float = "float";

        /// <summary>
        /// The name of the byte data type.
        /// </summary>
        public const string Byte = "byte";

        /// <summary>
        /// The name of the short data type.
        /// </summary>
        public const string Short = "short";

        /// <summary>
        /// The name of the int data type.
        /// </summary>
        public const string Int = "int";

        /// <summary>
        /// The name of the long data type.
        /// </summary>
        public const string Long = "long";

        /// <summary>
        /// The name of the bool data type.
        /// </summary>
        public const string Bool = "bool";

        /// <summary>
        /// Legal escape sequences permitted in a char or a string.
        /// </summary>
        private static List<char> escapeSequences = new List<char>();

        /// <summary>
        /// Content of this literal.
        /// </summary>
        private string data;

        /// <summary>
        /// Type of this literal.
        /// </summary>
        private string type;

        /// <summary>
        /// Initializes static members of the Literal class.
        /// </summary>
        static Literal()
        {
            Literal.escapeSequences.AddRange(new char[] { 'n', 't', '\'', '\"', '\\' });
        }

        /// <summary>
        /// Constructs a new expression type based on a name of a type.
        /// </summary>
        /// <param name="type">Name of type to use.</param>
        /// <returns>The constructed expression type.</returns>
        public static ExpressionType ConstructExpression(string type)
        {
            return new ObjectExpressionType(
                CompilerService.Instance.ClassesList[type],
                MemberStaticType.Normal);
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            return new TreeNode(this.type + ": " + this.data);
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.data = node.Token.Text;
            this.StartLocation = node.Token.Location;
            this.EndLocation = node.Token.Location;

            if (node.Term.Name == LanguageGrammar.NaturalLiteral.Name)
            {
                char suffix = this.data[this.data.Length - 1];
                this.type = (suffix == 'b' || suffix == 'B') ? Byte
                          : (suffix == 's' || suffix == 'S') ? Short
                          : (suffix == 'l' || suffix == 'L') ? Long : Int;
            }
            else if (node.Term.Name == LanguageGrammar.DecimalLiteral.Name)
            {
                char suffix = this.data[this.data.Length - 1];
                this.type = (suffix == 'f' || suffix == 'F') ? Float : Double;
            }
            else if (node.Term.Name == LanguageGrammar.CharLiteral.Name)
            {
                this.type = Char;
            }
            else if (node.Term.Name == LanguageGrammar.StringLiteral.Name)
            {
                this.type = String;
            }
            else if (this.data == "true" || this.data == "false")
            {
                this.type = Bool;
            }
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            if (this.type == Literal.Char)
            {
                if (this.data[1] == '\\' && Literal.escapeSequences.Contains(this.data[2]) == false)
                {
                    this.AddError(ErrorType.IncorrectEscapeSequence, this.data[2].ToString());
                    return true;
                }
            }
            else if (this.type == Literal.String)
            {
                for (int i = 1; i < this.data.Length - 1; i++)
                {
                    if (this.data[i] == '\\')
                    {
                        if (Literal.escapeSequences.Contains(this.data[i + 1]) == false)
                        {
                            this.AddError(ErrorType.IncorrectEscapeSequence, this.data[i + 1].ToString());
                            return true;
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
            }
            else
            {
                string value = char.IsLetter(this.data[this.data.Length - 1])
                    ? this.data.Substring(0, this.data.Length - 1)
                    : this.data;

                try
                {
                    switch (this.type)
                    {
                        case Literal.Byte:
                            byte.Parse(value);
                            break;
                        case Literal.Short:
                            short.Parse(value);
                            break;
                        case Literal.Int:
                            int.Parse(value);
                            break;
                        case Literal.Long:
                            long.Parse(value);
                            break;
                        case Literal.Float:
                            float.Parse(value);
                            break;
                        case Literal.Double:
                            double.Parse(value);
                            break;
                    }
                }
                catch
                {
                    this.AddError(ErrorType.IncorrectNumberLiteral, value);
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public override ExpressionType GetExpressionType(ScopeStack stack)
        {
            return new ObjectExpressionType(
                CompilerService.Instance.ClassesList[this.type],
                MemberStaticType.Normal);
        }
    }
}
