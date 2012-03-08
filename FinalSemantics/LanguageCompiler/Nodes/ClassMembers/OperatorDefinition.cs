namespace LanguageCompiler.Nodes.ClassMembers
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.Statements;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "Operator Definition" rule.
    /// </summary>
    public class OperatorDefinition : MemberDefinition
    {
        /// <summary>
        /// Operators that are non overloadable.
        /// </summary>
        private static List<string> nonOverloadableOperators = new List<string>();

        /// <summary>
        /// Operators that don't take parameters.
        /// </summary>
        private static List<string> noParameterOperators = new List<string>();

        /// <summary>
        /// Operators that take only one parameter.
        /// </summary>
        private static List<string> oneParameterOperators = new List<string>();

        /// <summary>
        /// Assignment operators that return the containing type.
        /// </summary>
        private static List<string> assignmentOperators = new List<string>();

        /// <summary>
        /// Operator of this method.
        /// </summary>
        private string operatorDefined;

        /// <summary>
        /// Parameters of this method.
        /// </summary>
        private List<Parameter> parameters = new List<Parameter>();

        /// <summary>
        /// Code block of this method.
        /// </summary>
        private Block block;

        /// <summary>
        /// Initializes static members of the OperatorDefinition class.
        /// </summary>
        static OperatorDefinition()
        {
            OperatorDefinition.nonOverloadableOperators.AddRange(new string[] { "==", "!=", "not", "and", "or" });
            OperatorDefinition.noParameterOperators.AddRange(new string[] { "++", "--" });
            OperatorDefinition.oneParameterOperators.AddRange(new string[] { "<", ">", "<=", ">=", "+", "-", "*", "/", "%" });
            OperatorDefinition.assignmentOperators.AddRange(new string[] { "=", "+=", "-=", "*=", "/=", "%=" });
        }

        /// <summary>
        /// Gets the operator of this method.
        /// </summary>
        public string OperatorDefined
        {
            get { return this.operatorDefined; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = base.GetGUINode();
            result.Text = "Operator Declaration";
            result.Nodes.Add(new TreeNode(this.operatorDefined));

            TreeNode parameters = new TreeNode("Parameters: Count = " + this.parameters.Count);
            foreach (Parameter p in this.parameters)
            {
                parameters.Nodes.Add(p.GetGUINode());
            }

            result.Nodes.Add(parameters);
            if (this.block == null)
            {
                result.Nodes.Add("No Body!");
            }
            else
            {
                result.Nodes.Add(this.block.GetGUINode());
            }

            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            base.RecieveData(node);
            this.operatorDefined = node.ChildNodes[5].Token.Text;
            foreach (ParseTreeNode child in node.ChildNodes[6].ChildNodes[1].ChildNodes)
            {
                Parameter p = new Parameter();
                p.RecieveData(child);
                this.parameters.Add(p);
            }

            if (node.ChildNodes[7].Term.Name == LanguageGrammar.Block.Name)
            {
                this.block = new Block();
                this.block.RecieveData(node.ChildNodes[7]);
                this.EndLocation = this.block.EndLocation;
            }
            else
            {
                this.block = null;
                this.EndLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
            }
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool HaveSemanticErrors(ScopeStack scopeStack)
        {
            bool foundErrors = false;
            if (OperatorDefinition.nonOverloadableOperators.Contains(this.operatorDefined))
            {
                this.AddError(ErrorType.OperatorNotOverloadable, this.operatorDefined);
                foundErrors = true;
            }
            
            if (OperatorDefinition.noParameterOperators.Contains(this.operatorDefined) && this.parameters.Count != 0)
            {
                this.AddError(ErrorType.OperatorInvalidParameters, this.operatorDefined);
                foundErrors = true;
            }

            if (OperatorDefinition.oneParameterOperators.Contains(this.operatorDefined) && this.parameters.Count != 1)
            {
                this.AddError(ErrorType.OperatorInvalidParameters, this.operatorDefined);
                foundErrors = true;
            }

            if (this.operatorDefined == "<" || this.operatorDefined == ">" || this.operatorDefined == "<=" || this.operatorDefined == ">=")
            {
                if ((this.Type is Identifier) == false || (this.Type as Identifier).Text != Literal.Bool)
                {
                    this.AddError(ErrorType.OperatorInvalidReturnType, this.operatorDefined);
                    foundErrors = true;
                }
            }

            if (this.ModifierType == MemberModifierType.Abstract && this.block != null)
            {
                this.AddError(ErrorType.AbstractMemberHasBody, this.operatorDefined);
                foundErrors = true;
            }

            if (this.ModifierType != MemberModifierType.Abstract && this.block == null)
            {
                this.AddError(ErrorType.MissingBodyOfNonAbstractMember, this.operatorDefined);
                foundErrors = true;
            }

            return foundErrors;
        }
    }
}
