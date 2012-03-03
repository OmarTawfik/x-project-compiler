namespace LanguageCompiler.Nodes.ClassMembers
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Nodes.Types;

    /// <summary>
    /// Holds all data related to a "Field Atom" rule.
    /// </summary>
    public class FieldAtom : MemberDefinition
    {
        /// <summary>
        /// Name of this atom.
        /// </summary>
        private Identifier name;

        /// <summary>
        /// Value of this atom.
        /// </summary>
        private BaseNode value;

        /// <summary>
        /// Gets the name of the field atom.
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
            TreeNode result = new TreeNode("Atom");
            result.Nodes.Add(this.name.GetGUINode());
            if (this.value != null)
            {
                result.Nodes.Add(this.value.GetGUINode());
            }

            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.name = new Identifier();
            this.name.RecieveData(node.ChildNodes[0]);
            this.StartLocation = this.name.StartLocation;

            if (node.ChildNodes[1].ChildNodes.Count > 0)
            {
                this.value = ExpressionsFactory.GetBaseExpr(node.ChildNodes[1].ChildNodes[1]);
                this.EndLocation = this.value.EndLocation;
            }
            else
            {
                this.value = null;
                this.EndLocation = this.name.EndLocation;
            }
        }
        
        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        public override void CheckSemantics()
        {
            if (this.name.CheckTypeExists(false))
            {
                this.AddError(ErrorType.MemberNameIsAType, this.name.Text);
            }
        }
    }
}
