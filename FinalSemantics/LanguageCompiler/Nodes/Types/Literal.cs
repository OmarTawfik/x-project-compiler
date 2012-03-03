namespace LanguageCompiler.Nodes.Types
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;

    /// <summary>
    /// Holds all data related to a "Literal" rule.
    /// </summary>
    public class Literal : BaseNode
    {
        /// <summary>
        /// The name of the char data type.
        /// </summary>
        public static readonly string Char = "char";

        /// <summary>
        /// The name of the string data type.
        /// </summary>
        public static readonly string String = "string";

        /// <summary>
        /// The name of the double data type.
        /// </summary>
        public static readonly string Double = "double";

        /// <summary>
        /// The name of the float data type.
        /// </summary>
        public static readonly string Float = "float";

        /// <summary>
        /// The name of the byte data type.
        /// </summary>
        public static readonly string Byte = "byte";

        /// <summary>
        /// The name of the short data type.
        /// </summary>
        public static readonly string Short = "short";

        /// <summary>
        /// The name of the int data type.
        /// </summary>
        public static readonly string Int = "int";

        /// <summary>
        /// The name of the long data type.
        /// </summary>
        public static readonly string Long = "long";

        /// <summary>
        /// The name of the bool data type.
        /// </summary>
        public static readonly string Bool = "bool";

        /// <summary>
        /// Content of this literal.
        /// </summary>
        private string data;

        /// <summary>
        /// Type of this literal.
        /// </summary>
        private string type;

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
    }
}
