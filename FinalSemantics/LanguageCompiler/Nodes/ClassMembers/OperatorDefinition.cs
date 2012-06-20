namespace LanguageCompiler.Nodes.ClassMembers
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.Statements;
    using LanguageCompiler.Nodes.TopLevel;
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
        public static readonly List<string> NonOverloadableOperators = new List<string>();

        /// <summary>
        /// Operators that don't take parameters.
        /// </summary>
        public static readonly List<string> PostfixOperators = new List<string>();

        /// <summary>
        /// Operators that take only one parameter.
        /// </summary>
        public static readonly List<string> OneParameterOperators = new List<string>();

        /// <summary>
        /// Assignment operators that return the containing type.
        /// </summary>
        public static readonly List<string> AssignmentOperators = new List<string>();

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
            OperatorDefinition.NonOverloadableOperators.AddRange(new string[] { "==", "!=", "not", "and", "or" });
            OperatorDefinition.PostfixOperators.AddRange(new string[] { "++", "--" });
            OperatorDefinition.OneParameterOperators.AddRange(new string[] { "<", ">", "<=", ">=", "+", "-", "*", "/", "%" });
            OperatorDefinition.AssignmentOperators.AddRange(new string[] { "=", "+=", "-=", "*=", "/=", "%=" });
        }

        /// <summary>
        /// Initializes a new instance of the OperatorDefinition class.
        /// </summary>
        /// <param name="parent">The class where this member was defined in.</param>
        public OperatorDefinition(ClassDefinition parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Gets the operator of this method.
        /// </summary>
        public string OperatorDefined
        {
            get { return this.operatorDefined; }
        }

        /// <summary>
        /// Gets the parameters of this method.
        /// </summary>
        public List<Parameter> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Gets the block of the method.
        /// </summary>
        public Block Block
        {
            get { return this.block; }
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
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            bool foundErrors = false;
            if (OperatorDefinition.NonOverloadableOperators.Contains(this.operatorDefined))
            {
                this.AddError(ErrorType.OperatorNotOverloadable, this.operatorDefined);
                foundErrors = true;
            }

            if (this.Type.Text != scopeStack.GetClass().Name.Text
                && (OperatorDefinition.AssignmentOperators.Contains(this.operatorDefined)
                || OperatorDefinition.PostfixOperators.Contains(this.operatorDefined)))
            {
                this.AddError(ErrorType.OperatorMustReturnContainingType);
                foundErrors = true;
            }

            if (OperatorDefinition.PostfixOperators.Contains(this.operatorDefined) && this.parameters.Count != 0)
            {
                this.AddError(ErrorType.OperatorInvalidParameters, this.operatorDefined);
                foundErrors = true;
            }

            if (OperatorDefinition.OneParameterOperators.Contains(this.operatorDefined) && this.parameters.Count != 1)
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

            if (this.block != null)
            {
                if (this.Type.Text != "void" && this.block.ReturnsAValue() == false)
                {
                    this.AddError(ErrorType.NotAllControlPathsReturnAValue);
                    foundErrors = true;
                }

                if (this.ModifierType == MemberModifierType.Abstract)
                {
                    this.AddError(ErrorType.AbstractMemberHasBody, this.operatorDefined);
                    foundErrors = true;
                }

                scopeStack.AddLevel(ScopeType.Function, this);
                foreach (Parameter param in this.parameters)
                {
                    foundErrors |= param.CheckSemanticErrors(scopeStack);
                    foundErrors |= scopeStack.DeclareVariable(
                        new Variable(
                            param.Type.GetExpressionType(scopeStack),
                            param.Name.Text),
                        this);
                }

                this.block.CheckSemanticErrors(scopeStack);
                scopeStack.DeleteLevel();
            }
            else
            {
                if (this.ModifierType != MemberModifierType.Abstract)
                {
                    this.AddError(ErrorType.MissingBodyOfNonAbstractMember, this.operatorDefined);
                    foundErrors = true;
                }
            }

            return foundErrors;
        }

        /// <summary>
        /// Checks if a statement or block of code returns a value.
        /// </summary>
        /// <returns>True if it returns a value, false otherwise.</returns>
        public override bool ReturnsAValue()
        {
            return false;
        }
    }
}