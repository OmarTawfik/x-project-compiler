namespace LanguageCompiler.Nodes.TopLevel
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.Types;

    /// <summary>
    /// The class modifier type.
    /// </summary>
    public enum ClassModifierType
    {
        /// <summary>
        /// Abstract: User cannot make an object from this class.
        /// </summary>
        Abstract,

        /// <summary>
        /// Concrete: User cannot inherit from this class.
        /// </summary>
        Concrete,

        /// <summary>
        /// Normal: User can inherit and make an object from this class.
        /// </summary>
        Normal,
    }

    /// <summary>
    /// The class label.
    /// </summary>
    public enum ClassLabel
    {
        /// <summary>
        /// Class: A normal class.
        /// </summary>
        Class,

        /// <summary>
        /// Screen: A screen class.
        /// </summary>
        Screen,
    }

    /// <summary>
    /// Holds all data related to a "Class Definition" rule.
    /// </summary>
    public class ClassDefinition : BaseNode
    {
        /// <summary>
        /// The modifier type for this class.
        /// </summary>
        private ClassModifierType modifierType = ClassModifierType.Normal;

        /// <summary>
        /// The label for this class.
        /// </summary>
        private ClassLabel label;

        /// <summary>
        /// Name of this class.
        /// </summary>
        private Identifier name;

        /// <summary>
        /// Base type of this class.
        /// </summary>
        private Identifier classBase;

        /// <summary>
        /// Methods of this class.
        /// </summary>
        private List<MethodDefinition> methods = new List<MethodDefinition>();

        /// <summary>
        /// Operators of this class.
        /// </summary>
        private List<OperatorDefinition> operators = new List<OperatorDefinition>();

        /// <summary>
        /// Fields of this class.
        /// </summary>
        private List<FieldDefinition> fields = new List<FieldDefinition>();

        /// <summary>
        /// Gets the name of this class.
        /// </summary>
        public Identifier Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the base type of this class.
        /// </summary>
        public Identifier ClassBase
        {
            get { return this.classBase; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode classNode = new TreeNode(string.Format(
                "{0} {1} {2}",
                this.modifierType.ToString(),
                this.label.ToString(),
                this.name.Text));

            if (this.classBase != null)
            {
                classNode.Text += " extends " + this.classBase.Text;
            }

            TreeNode methods = new TreeNode("Methods: Count = " + this.methods.Count);
            foreach (MethodDefinition method in this.methods)
            {
                methods.Nodes.Add(method.GetGUINode());
            }

            TreeNode operators = new TreeNode("Operators: Count = " + this.operators.Count);
            foreach (OperatorDefinition op in this.operators)
            {
                operators.Nodes.Add(op.GetGUINode());
            }

            TreeNode fields = new TreeNode("Fields: Count = " + this.fields.Count);
            foreach (FieldDefinition field in this.fields)
            {
                fields.Nodes.Add(field.GetGUINode());
            }

            classNode.Nodes.Add(methods);
            classNode.Nodes.Add(operators);
            classNode.Nodes.Add(fields);
            return classNode;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.StartLocation = new SourceLocation(-1, -1, -1);
            if (node.ChildNodes[0].ChildNodes.Count > 0)
            {
                if (node.ChildNodes[0].ChildNodes[0].Token.Text == "abstract")
                {
                    this.modifierType = ClassModifierType.Abstract;
                }
                else if (node.ChildNodes[0].ChildNodes[0].Token.Text == "concrete")
                {
                    this.modifierType = ClassModifierType.Concrete;
                }

                if (this.StartLocation.Position == -1)
                {
                    this.StartLocation = node.ChildNodes[0].ChildNodes[0].Token.Location;
                }
            }

            if (node.ChildNodes[1].Token.Text == "class")
            {
                this.label = ClassLabel.Class;
            }
            else if (node.ChildNodes[1].Token.Text == "screen")
            {
                this.label = ClassLabel.Screen;
            }

            if (this.StartLocation.Position == -1)
            {
                this.StartLocation = node.ChildNodes[1].Token.Location;
            }

            this.name = new Identifier();
            this.name.RecieveData(node.ChildNodes[2]);

            if (node.ChildNodes[3].ChildNodes.Count > 0)
            {
                this.classBase = new Identifier();
                this.classBase.RecieveData(node.ChildNodes[3].ChildNodes[1]);
            }

            foreach (ParseTreeNode member in node.ChildNodes[5].ChildNodes)
            {
                if (member.Term.Name == LanguageGrammar.MethodDefinition.Name)
                {
                    MethodDefinition method = new MethodDefinition();
                    method.RecieveData(member);
                    this.methods.Add(method);
                }
                else if (member.Term.Name == LanguageGrammar.OperatorDefinition.Name)
                {
                    OperatorDefinition op = new OperatorDefinition();
                    op.RecieveData(member);
                    this.operators.Add(op);
                }
                else if (member.Term.Name == LanguageGrammar.FieldDefinition.Name)
                {
                    FieldDefinition field = new FieldDefinition();
                    field.RecieveData(member);
                    this.fields.Add(field);
                }
            }

            this.EndLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        public override void CheckSemantics()
        {
            this.name.CheckSemantics();
            if (this.classBase != null && this.classBase.CheckTypeExists())
            {
                this.classBase.CheckSemantics();
                if (this.CheckCyclicInheritence())
                {
                    return;
                }

                ClassDefinition parent = CompilerService.Instance.ClassesList[this.classBase.Text];
                if (this.label == ClassLabel.Screen)
                {
                    this.AddError(ErrorType.ScreenCannotInherit, this.name.Text);
                }
                else if (parent.modifierType == ClassModifierType.Concrete)
                {
                    this.AddError(ErrorType.ConcreteBase, this.name.Text);
                }
            }

            if (this.label == ClassLabel.Screen && this.modifierType != ClassModifierType.Normal)
            {
                this.AddError(ErrorType.ScreenModifierNotNormal, this.name.Text);
            }

            if (this.MultipleMembersExist() == false)
            {
                foreach (FieldDefinition field in this.fields)
                {
                    field.CheckSemantics();
                }

                foreach (MethodDefinition method in this.methods)
                {
                    method.CheckSemantics();
                }

                foreach (OperatorDefinition op in this.operators)
                {
                    op.CheckSemantics();
                }
            }
        }

        /// <summary>
        /// Checks if multiple members with the same name exist.
        /// </summary>
        /// <returns>True if multiple members with the same name exist, false otherwise.</returns>
        private bool MultipleMembersExist()
        {
            List<string> members = new List<string>();
            foreach (FieldDefinition field in this.fields)
            {
                foreach (FieldAtom atom in field.Atoms)
                {
                    if (members.Contains(atom.Name.Text))
                    {
                        this.AddError(ErrorType.MultipleMembersWithSameName, atom.Name.Text, this.name.Text);
                        return true;
                    }
                    else
                    {
                        members.Add(atom.Name.Text);
                    }
                }
            }

            foreach (MethodDefinition method in this.methods)
            {
                if (members.Contains(method.Name.Text))
                {
                    this.AddError(ErrorType.MultipleMembersWithSameName, method.Name.Text, this.name.Text);
                    return true;
                }
                else
                {
                    members.Add(method.Name.Text);
                }
            }

            List<string> operators = new List<string>();
            foreach (OperatorDefinition op in this.operators)
            {
                if (operators.Contains(op.OperatorDefined))
                {
                    this.AddError(ErrorType.MultipleMembersWithSameName, op.OperatorDefined, this.name.Text);
                    return true;
                }
                else
                {
                    operators.Add(op.OperatorDefined);
                }
            }

            return false;
        }

        /// <summary>
        /// Checks for cycles in the inherience list of this class.
        /// </summary>
        /// <returns>True if there is a cyclic inheritence, false otherwise.</returns>
        private bool CheckCyclicInheritence()
        {
            List<string> cycle = new List<string>();
            ClassDefinition current = this;

            while (true)
            {
                if (cycle.Contains(current.name.Text))
                {
                    this.AddError(ErrorType.CyclicInheritence, this.name.Text);
                    return true;
                }
                else
                {
                    cycle.Add(current.name.Text);
                }

                if (current.classBase != null && current.classBase.CheckTypeExists(false))
                {
                    current = CompilerService.Instance.ClassesList[current.classBase.Text];
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
