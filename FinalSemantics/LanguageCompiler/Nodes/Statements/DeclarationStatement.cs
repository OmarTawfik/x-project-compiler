﻿namespace LanguageCompiler.Nodes.Statements

 {
     using System.Collections.Generic;
     using System.Windows.Forms;
     using Irony.Parsing;
     using LanguageCompiler.Errors;
     using LanguageCompiler.Nodes.ClassMembers;
     using LanguageCompiler.Nodes.Expressions;
     using LanguageCompiler.Nodes.Types;
     using LanguageCompiler.Semantics;
     using LanguageCompiler.Semantics.ExpressionTypes;

     /// <summary>
     /// Holds all data related to a "DeclarationStatement" rule.
     /// </summary>
     public class DeclarationStatement : BaseNode
     {
         /// <summary>
         /// Type of this statement.
         /// </summary>
         private Identifier type;

         /// <summary>
         /// Atoms of this field.
         /// </summary>
         private List<FieldAtom> atoms = new List<FieldAtom>();

         /// <summary>
         /// Gets the type of this statement.
         /// </summary>
         public Identifier Type
         {
             get { return this.type; }
         }

         /// <summary>
         /// Gets the atoms of this field.
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
             TreeNode result = new TreeNode("Declaration Statement");
             result.Nodes.Add(this.type.GetGUINode());
             foreach (FieldAtom atom in this.atoms)
             {
                 result.Nodes.Add(atom.GetGUINode());
             }

             return result;
         }

         /// <summary>
         /// Recieves an irony ParseTreeNode and constructs its contents.
         /// </summary>
         /// <param name="node">The irony ParseTreeNode.</param>
         public override void RecieveData(ParseTreeNode node)
         {
             this.type = new Identifier();
             this.type.RecieveData(node.ChildNodes[0]);

             foreach (ParseTreeNode child in node.ChildNodes[1].ChildNodes)
             {
                 FieldAtom atom = new FieldAtom();
                 atom.RecieveData(child);
                 this.atoms.Add(atom);
             }

             this.StartLocation = this.type.StartLocation;
             this.StartLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
         }

         /// <summary>
         /// Checks for semantic errors within this node.
         /// </summary>
         /// <param name="scopeStack">The scope stack associated with this node.</param>
         /// <returns>True if errors are found, false otherwise.</returns>
         public override bool CheckSemanticErrors(ScopeStack scopeStack)
         {
             if (type.CheckSemanticErrors(scopeStack) || type.CheckTypeExists(true) == false)
             {
                 return true;
             }
             
             ExpressionType myExpressionType = this.type.GetExpressionType(scopeStack);

             foreach (FieldAtom atom in this.atoms)
             {
                 if(atom.CheckSemanticErrors(scopeStack))
                 {
                     return true;
                 }

                 if ( scopeStack.DeclareVariable(new Variable(myExpressionType, atom.Name.Text), atom) == false)
                 {
                     return true;
                 }

                 if (atom.Value != null)
                 {
                     ExpressionType atomType = atom.Value.GetExpressionType(scopeStack);
                     if ( myExpressionType.IsEqualTo(atomType) == false)
                     {
                         return true;
                     }
                 }
             }

             return false;
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