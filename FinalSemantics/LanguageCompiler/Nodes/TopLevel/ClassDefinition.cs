namespace LanguageCompiler.Nodes.TopLevel
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;

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
        /// Names of the backend classes.
        /// </summary>
        private static List<string> backendClasses = new List<string>();

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
        /// The name of the file this class was declared in.
        /// </summary>
        private string fileName;

        /// <summary>
        /// Indicates whether this class should be implemented in the backend.
        /// </summary>
        private bool isBackend;

        /// <summary>
        /// Members of this class.
        /// </summary>
        private List<MemberDefinition> members = new List<MemberDefinition>();

        /// <summary>
        /// Initializes static members of the ClassDefinition class.
        /// </summary>
        static ClassDefinition()
        {
            backendClasses.AddRange(new string[]
            {
                "list", Literal.Bool,
                Literal.Char, Literal.String, Literal.Double, Literal.Float,
                Literal.Byte, Literal.Short, Literal.Int, Literal.Long,
            });
        }

        /// <summary>
        /// Initializes a new instance of the ClassDefinition class.
        /// </summary>
        /// <param name="fileName">The name of the file this class was declared in.</param>
        public ClassDefinition(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Gets the members of this class.
        /// </summary>
        public List<MemberDefinition> Members
        {
            get { return this.members; }
        }

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
        /// Gets a value indicating whether this class should be implemented in the backend.
        /// </summary>
        public bool IsBackend
        {
            get { return this.isBackend; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode classNode = new TreeNode(string.Format(
                "{0} {1} {2} {3}",
                this.modifierType.ToString(),
                this.isBackend ? "Backend" : string.Empty,
                this.label.ToString(),
                this.name.Text));

            if (this.classBase != null)
            {
                classNode.Text += " extends " + this.classBase.Text;
            }

            TreeNode membersNode = new TreeNode("Members: Count = " + this.members.Count);
            foreach (MemberDefinition member in this.members)
            {
                membersNode.Nodes.Add(member.GetGUINode());
            }

            classNode.Nodes.Add(membersNode);
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

            this.isBackend = node.ChildNodes[1].ChildNodes.Count > 0;

            if (node.ChildNodes[2].Token.Text == "class")
            {
                this.label = ClassLabel.Class;
            }
            else if (node.ChildNodes[2].Token.Text == "screen")
            {
                this.label = ClassLabel.Screen;
            }

            if (this.StartLocation.Position == -1)
            {
                this.StartLocation = node.ChildNodes[2].Token.Location;
            }

            this.name = new Identifier();
            this.name.RecieveData(node.ChildNodes[3]);

            if (node.ChildNodes[4].ChildNodes.Count > 0)
            {
                this.classBase = new Identifier();
                this.classBase.RecieveData(node.ChildNodes[4].ChildNodes[1]);
            }

            foreach (ParseTreeNode memberNode in node.ChildNodes[6].ChildNodes)
            {
                MemberDefinition member = null;
                if (memberNode.Term.Name == LanguageGrammar.MethodDefinition.Name)
                {
                    member = new MethodDefinition(this);
                }
                else if (memberNode.Term.Name == LanguageGrammar.OperatorDefinition.Name)
                {
                    member = new OperatorDefinition(this);
                }
                else if (memberNode.Term.Name == LanguageGrammar.FieldDefinition.Name)
                {
                    member = new FieldDefinition(this);
                }

                member.RecieveData(memberNode);
                this.members.Add(member);
            }

            this.EndLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            if (this.name.CheckSemanticErrors(scopeStack))
            {
                return true;
            }

            bool foundErrors = false;

            if (this.isBackend && backendClasses.Contains(this.name.Text) == false)
            {
                foundErrors = true;
                this.AddError(ErrorType.UserDefinedBackendClass, this.name.Text);
            }

            if (this.classBase != null && this.classBase.CheckTypeExists())
            {
                if (this.CheckCyclicInheritence())
                {
                    return true;
                }

                foundErrors |= this.classBase.CheckSemanticErrors(scopeStack);
                ClassDefinition parent = CompilerService.Instance.ClassesList[this.classBase.Text];

                if (this.label == ClassLabel.Screen)
                {
                    this.AddError(ErrorType.ScreenCannotInherit, this.name.Text);
                    foundErrors = true;
                }
                else if (parent.modifierType == ClassModifierType.Concrete)
                {
                    this.AddError(ErrorType.ConcreteBase, this.name.Text);
                    foundErrors = true;
                }
            }

            if (this.label == ClassLabel.Screen && this.modifierType != ClassModifierType.Normal)
            {
                this.AddError(ErrorType.ScreenModifierNotNormal, this.name.Text);
                foundErrors = true;
            }

            scopeStack.AddLevel(ScopeType.Class, this);
            foreach (MemberDefinition member in this.members)
            {
                if (member is FieldDefinition)
                {
                    FieldDefinition field = member as FieldDefinition;
                    foreach (FieldAtom atom in field.Atoms)
                    {
                        scopeStack.DeclareVariable(
                            new Variable(
                                member.Type.GetExpressionType(scopeStack),
                                atom.Name.Text),
                            this);
                    }
                }
            }

            if (this.CheckMultipleMembers() == false)
            {
                foreach (MemberDefinition member in this.members)
                {
                    foundErrors |= member.CheckSemanticErrors(scopeStack);
                }
            }

            scopeStack.DeleteLevel();
            return foundErrors;
        }

        /// <summary>
        /// Checks if a certain class is a parent of this class.
        /// </summary>
        /// <param name="parent">Name of the parent class.</param>
        /// <returns>True if a certain class is a parent of this class, false otherwise.</returns>
        public bool IsMyParent(string parent)
        {
            if (this.classBase != null)
            {
                if (this.classBase.Text == parent)
                {
                    return true;
                }

                ClassDefinition parentClass = CompilerService.Instance.ClassesList[this.classBase.Text];
                return parentClass.IsMyParent(parent);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if multiple members with the same name exist.
        /// </summary>
        /// <returns>True if multiple members with the same name exist, false otherwise.</returns>
        private bool CheckMultipleMembers()
        {
            List<string> memberNames = new List<string>();
            foreach (MemberDefinition member in this.members)
            {
                if (member is FieldDefinition)
                {
                    FieldDefinition field = member as FieldDefinition;
                    foreach (FieldAtom atom in field.Atoms)
                    {
                        if (memberNames.Contains(atom.Name.Text))
                        {
                            this.AddError(ErrorType.ItemAlreadyDefined, atom.Name.Text);
                            return true;
                        }
                        else
                        {
                            memberNames.Add(atom.Name.Text);
                        }
                    }
                }
                else if (member is MethodDefinition)
                {
                    MethodDefinition method = member as MethodDefinition;
                    if (memberNames.Contains(method.Name.Text))
                    {
                        this.AddError(ErrorType.ItemAlreadyDefined, method.Name.Text);
                        return true;
                    }
                    else
                    {
                        memberNames.Add(method.Name.Text);
                    }
                }
                else if (member is OperatorDefinition)
                {
                    OperatorDefinition op = member as OperatorDefinition;
                    if (memberNames.Contains(op.OperatorDefined))
                    {
                        this.AddError(ErrorType.ItemAlreadyDefined, op.OperatorDefined);
                        return true;
                    }
                    else
                    {
                        memberNames.Add(op.OperatorDefined);
                    }
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