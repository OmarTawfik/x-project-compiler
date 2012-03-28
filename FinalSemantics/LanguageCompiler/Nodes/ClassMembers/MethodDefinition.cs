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
            bool foundErrors = false;
            if (this.name.CheckTypeExists(false))
            {
                this.AddError(ErrorType.MemberNameIsAType, this.name.Text);
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

            return foundErrors;
        }
    }
}