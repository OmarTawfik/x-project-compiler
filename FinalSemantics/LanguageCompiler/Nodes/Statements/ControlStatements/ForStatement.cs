namespace LanguageCompiler.Nodes.Statements.ControlStatements
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "ForStatement" rule.
    /// </summary>
    public class ForStatement : BaseNode
    {
        /// <summary>
        /// First part list of assignments or expressions.
        /// </summary>
        private List<FieldAtom> firstPartList = new List<FieldAtom>();

        /// <summary>
        /// Stopping condition of the for statement.
        /// </summary>
        private ExpressionNode secondPart;

        /// <summary>
        /// Third part list of expressions.
        /// </summary>
        private List<ExpressionNode> thidPartList = new List<ExpressionNode>();

        /// <summary>
        /// Body of the for statement.
        /// </summary>
        private Block body;

        /// <summary>
        /// Gets the first part of the for loop.
        /// </summary>
        public List<FieldAtom> FirstPartList
        {
            get { return this.firstPartList; }
        }

        /// <summary>
        /// Gets the second part of the for loop.
        /// </summary>
        public ExpressionNode SecondPart
        {
            get { return this.secondPart; }
        }

        /// <summary>
        /// Gets the third part of the for loop.
        /// </summary>
        public List<ExpressionNode> ThirdPartList
        {
            get { return this.ThirdPartList; }
        }

        /// <summary>
        /// Gets the body of the for loop.
        /// </summary>
        public Block Block
        {
            get { return this.body; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("For Statement");
            TreeNode firstPartNode = new TreeNode("First Part");
            foreach (BaseNode child in this.firstPartList)
            {
                firstPartNode.Nodes.Add(child.GetGUINode());
            }

            result.Nodes.Add(firstPartNode);
            if (this.secondPart != null)
            {
                result.Nodes.Add(this.secondPart.GetGUINode());
            }
            else
            {
                result.Nodes.Add("No Second Part");
            }

            TreeNode thirdPartNode = new TreeNode("Third Part");
            foreach (BaseNode child in this.thidPartList)
            {
                thirdPartNode.Nodes.Add(child.GetGUINode());
            }

            result.Nodes.Add(thirdPartNode);

            if (this.body != null)
            {
                result.Nodes.Add(this.body.GetGUINode());
            }
            else
            {
                result.Nodes.Add(new TreeNode("No Body!"));
            }

            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            if (node.ChildNodes[2].ChildNodes.Count > 0)
            {
                foreach (ParseTreeNode child in node.ChildNodes[2].ChildNodes[0].ChildNodes)
                {
                    FieldAtom atom = new FieldAtom();
                    atom.RecieveData(child);
                    this.firstPartList.Add(atom);
                }
            }

            if (node.ChildNodes[4].ChildNodes.Count > 0)
            {
                this.secondPart = ExpressionsFactory.GetBaseExpr(node.ChildNodes[4].ChildNodes[0]);
            }
            else
            {
                this.secondPart = null;
            }

            foreach (ParseTreeNode child in node.ChildNodes[6].ChildNodes)
            {
                this.thidPartList.Add(ExpressionsFactory.GetBaseExpr(child));
            }

            if (node.ChildNodes[8].Term.Name == LanguageGrammar.Block.Name)
            {
                this.body = new Block();
                this.body.RecieveData(node.ChildNodes[8]);
            }
            else
            {
                this.body = null;
            }

            this.StartLocation = node.ChildNodes[0].Token.Location;
            this.EndLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            bool foundErrors = false;

            foreach (FieldAtom atom in this.firstPartList)
            {
                foundErrors |= atom.CheckSemanticErrors(scopeStack);
            }

            foundErrors |= this.secondPart.CheckSemanticErrors(scopeStack);

            foreach (BaseNode node in this.thidPartList)
            {
                foundErrors |= node.CheckSemanticErrors(scopeStack);
            }

            scopeStack.AddLevel(ScopeType.Loop, this);
            this.body.CheckSemanticErrors(scopeStack);
            scopeStack.DeleteLevel();

            if (foundErrors)
            {
                return foundErrors;
            }

            if (this.secondPart.GetExpressionType(scopeStack).IsEqualTo(Literal.ConstructExpression(Literal.Bool)) == false)
            {
                this.AddError(ErrorType.ExpressionNotBoolean);
                foundErrors = true;
            }

            return foundErrors;
        }
    }
}