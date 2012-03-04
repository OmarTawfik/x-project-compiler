namespace LanguageCompiler.Nodes.ClassMembers
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;

    /// <summary>
    /// Holds all data related to a "Field Definition" rule.
    /// </summary>
    public class FieldDefinition : MemberDefinition
    {
        /// <summary>
        /// Atoms of this field.
        /// </summary>
        private List<FieldAtom> atoms = new List<FieldAtom>();

        /// <summary>
        /// Gets the list of field atoms.
        /// </summary>
        public List<FieldAtom> Atoms
        {
            get { return this.atoms; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = base.GetGUINode();
            result.Text = "Field Declaration";

            TreeNode atoms = new TreeNode("Atoms: Count = " + this.atoms.Count);
            foreach (FieldAtom atom in this.atoms)
            {
                atoms.Nodes.Add(atom.GetGUINode());
            }

            result.Nodes.Add(atoms);
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            base.RecieveData(node);
            foreach (ParseTreeNode child in node.ChildNodes[4].ChildNodes)
            {
                FieldAtom atom = new FieldAtom();
                atom.RecieveData(child);
                this.atoms.Add(atom);
            }

            this.EndLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        public override void CheckSemantics()
        {
            if (this.ModifierType != MemberModifierType.Normal)
            {
                this.AddError(ErrorType.FieldInvalidModifier);
            }

            foreach (FieldAtom atom in this.atoms)
            {
                atom.CheckSemantics();
            }
        }
    }
}
