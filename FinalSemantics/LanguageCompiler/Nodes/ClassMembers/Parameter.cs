namespace LanguageCompiler.Nodes.ClassMembers
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "Parameter" rule.
    /// </summary>
    public class Parameter : BaseNode
    {
        /// <summary>
        /// Type of this parameter.
        /// </summary>
        private ExpressionNode type;

        /// <summary>
        /// Name of this parameter.
        /// </summary>
        private Identifier name;

        /// <summary>
        /// Gets the type of this parameter.
        /// </summary>
        public ExpressionNode Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the name of this parameter.
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
            TreeNode result = new TreeNode("Parameter");
            result.Nodes.Add(this.name.GetGUINode());
            result.Nodes.Add(this.type.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            if (node.ChildNodes[0].Term.Name == LanguageGrammar.ID.Name)
            {
                this.type = new Identifier();
                this.type.RecieveData(node.ChildNodes[0]);
            }
            else if (node.ChildNodes[0].Term.Name == LanguageGrammar.ArrayType.Name)
            {
                this.type = new ArrayType();
                this.type.RecieveData(node.ChildNodes[0]);
            }

            this.name = new Identifier();
            this.name.RecieveData(node.ChildNodes[1]);

            this.StartLocation = this.type.StartLocation;
            this.EndLocation = this.name.EndLocation;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            return this.type.CheckSemanticErrors(scopeStack) || this.name.CheckSemanticErrors(scopeStack);
        }
    }
}