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
    /// Holds all data related to a "Method Definition" rule.
    /// </summary>
    public class MethodDefinition : MemberDefinition
    {
        /// <summary>
        /// Name of this method.
        /// </summary>
        private Identifier name;

        /// <summary>
        /// Parameters of this method.
        /// </summary>
        private List<Parameter> parameters = new List<Parameter>();

        /// <summary>
        /// Code block of this method.
        /// </summary>
        private Block block;

        /// <summary>
        /// Initializes a new instance of the MethodDefinition class.
        /// </summary>
        /// <param name="parent">The class where this member was defined in.</param>
        public MethodDefinition(ClassDefinition parent)
            : base(parent)
        {
        }

        /// <summary>
        /// Gets the name of this method.
        /// </summary>
        public Identifier Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the parameters of the method.
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
            result.Text = "Method Declaration";
            result.Nodes.Add(this.name.GetGUINode());

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
            this.name = new Identifier();
            this.name.RecieveData(node.ChildNodes[4]);

            foreach (ParseTreeNode child in node.ChildNodes[5].ChildNodes[1].ChildNodes)
            {
                Parameter p = new Parameter();
                p.RecieveData(child);
                this.parameters.Add(p);
            }

            if (node.ChildNodes[6].Term.Name == LanguageGrammar.Block.Name)
            {
                this.block = new Block();
                this.block.RecieveData(node.ChildNodes[6]);
                this.EndLocation = this.block.EndLocation;
            }
            else
            {
                this.block = null;
                this.EndLocation = node.ChildNodes[6].Token.Location;
            }
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            bool foundErrors = this.name.CheckSemanticErrors(scopeStack);
            if (this.name.CheckTypeExists(false) && this.Type.Text != "constructor")
            {
                this.AddError(ErrorType.MemberNameIsAType, this.name.Text);
                foundErrors = true;
            }

            if (this.StaticType == MemberStaticType.Static && this.Type.Text == "constructor" && this.AccessorType != MemberAccessorType.Public)
            {
                this.AddError(ErrorType.StaticConstructorsMustBePublic, this.name.Text);
                foundErrors = true;
            }

            if (this.name.Text == "constructor" && this.Type.Text != scopeStack.GetClass().Name.Text)
            {
                this.AddError(ErrorType.ConstructorMustReturnContainingType);
                foundErrors = true;
            }

            if (this.ModifierType == MemberModifierType.Abstract && this.block != null)
            {
                this.AddError(ErrorType.AbstractMemberHasBody, this.name.Text);
                foundErrors = true;
            }

            if (this.ModifierType != MemberModifierType.Abstract && this.block == null)
            {
                this.AddError(ErrorType.MissingBodyOfNonAbstractMember, this.name.Text);
                foundErrors = true;
            }

            if (this.block != null)
            {
                if (this.Type.Text != "void" && this.name.Text != "constructor" && this.block.ReturnsAValue() == false)
                {
                    this.AddError(ErrorType.NotAllControlPathsReturnAValue);
                    foundErrors = true;
                }

                scopeStack.AddLevel(ScopeType.Function, this);
                foreach (Parameter param in this.parameters)
                {
                    foundErrors |= param.CheckSemanticErrors(scopeStack)
                        || scopeStack.DeclareVariable(
                            new Variable(
                                param.Type.GetExpressionType(scopeStack),
                                param.Name.Text),
                            this);
                }

                this.block.CheckSemanticErrors(scopeStack);
                scopeStack.DeleteLevel();
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
        
        /// <summary>
        /// Checks for method signature match.
        /// </summary>
        /// <param name="method">The method to match with.</param>
        /// <returns>Is it a match.</returns>
        public bool DoesMatchSignature(MethodDefinition method)
        {
            if (this.Type.Text != method.Type.Text)
            {
                return false;
            }

            if (this.parameters.Count != method.parameters.Count)
            {
                return false;
            }

            for (int i = 0; i < this.parameters.Count; i++)
            {
                if (this.parameters[i].Type.Text != method.parameters[i].Type.Text)
                {
                    return false;
                }
            }

            return true;
        }
    }
}