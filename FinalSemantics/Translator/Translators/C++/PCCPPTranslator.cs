namespace LanguageTranslator.Translators.CPP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using LanguageCompiler;
    using LanguageCompiler.Nodes;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Nodes.Expressions.Basic;
    using LanguageCompiler.Nodes.Expressions.Complex;
    using LanguageCompiler.Nodes.Statements;
    using LanguageCompiler.Nodes.Statements.CommandStatements;
    using LanguageCompiler.Nodes.Statements.ControlStatements;
    using LanguageCompiler.Nodes.TopLevel;
    using LanguageCompiler.Nodes.Types;
    using LanguageTranslator;

    /// <summary>
    /// The PC/C++ translator
    /// </summary>
    public class PCCPPTranslator : TranslatorPlugin
    {
        /// <summary>
        /// Stores all the static constructors to be added later to the main.
        /// </summary>
        private List<string> staticClassConstructors;

        /// <summary>
        /// Controls whether to use static objects or shared_ptrs.
        /// </summary>
        private bool enableSharedPtrSupport = false;

        /// <summary>
        /// Initializes a new instance of the PCCPPTranslator class.
        /// </summary>
        public PCCPPTranslator()
        {
            this.BackendClasses = new List<BackendClass>();
            string[] backendFiles = null;

            try
            {
                backendFiles = Directory.GetFiles(this.PluginDirectory + "\\BackendClasses\\");
            }
            catch (Exception)
            {
                return;
            }

            foreach (string file in backendFiles)
            {
                try
                {
                    this.BackendClasses.Add(BackendClass.LoadBackendClassFromFile(file));
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Gets the platform ID of the translator.
        /// </summary>
        public override string PlatformID
        {
            get { return "PC-WIN32"; }
        }

        /// <summary>
        /// Gets the target language of the translator.
        /// </summary>
        public override string Language
        {
            get { return "C++"; }
        }

        /// <summary>
        /// Gets the path to the compiler used in this translator.
        /// </summary>
        public override string Compiler
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Microsoft.NET\\Framework\\v4.0.30319\\MsBuild.exe"; }
        }

        /// <summary>
        /// Gets the translator's subdirectory.
        /// </summary>
        protected override string PluginDirectory
        {
            get { return Directory.GetCurrentDirectory() + "\\Translators\\Win32CPP\\"; }
        }

        /// <summary>
        /// Translates the given classes to the targeted language and platform.
        /// </summary>
        /// <param name="classes">List of all classes.</param>
        public override void Translate(List<LanguageCompiler.Nodes.TopLevel.ClassDefinition> classes)
        {
            this.ResetGeneratedCode();
            this.staticClassConstructors = new List<string>();

            this.GenerateHeaders();
            this.GenerateBackendClasses();

            for (int i = 0; i < classes.Count; i++)
            {
                this.AddDefaultConstructor(classes[i]);
            }

            for (int i = 0; i < classes.Count; i++)
            {
                this.GenerateClassDeclaration(classes[i]);
            }

            for (int i = 0; i < classes.Count; i++)
            {
                this.GenerateClassDefinition(classes[i]);
            }

            this.AppendLine("void _INIT_STATIC_CONSTRUCTORS()");
            this.StartBlock();

            for (int i = 0; i < this.staticClassConstructors.Count; i++)
            {
                this.AppendLine(this.staticClassConstructors[i]);
            }

            this.EndBlock();
        }

        /// <summary>
        /// Calls the external compiler and builds to the targeted platform.
        /// </summary>
        public override void Build()
        {
            FileStream file = new FileStream(this.PluginDirectory + "\\BackendProject\\OpenGL_BACKEND\\GENCODE.h", FileMode.OpenOrCreate);
            file.SetLength(0);
            file.Flush();
            StreamWriter wr = new StreamWriter(file);
            wr.Write(this.GeneratedCode.ToString());
            wr.Close();
            file.Close();

            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo(this.Compiler, this.PluginDirectory + "\\BackendProject\\OpenGL_BACKEND.sln");

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                string result = proc.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
            }
            catch (Exception objException)
            {
                
            }
        }

        /// <summary>
        /// Runs the built application.
        /// </summary>
        public override void Run()
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo(this.PluginDirectory + "\\BackendProject\\Release\\OpenGL_BACKEND.exe");

                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                string result = proc.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
            }
            catch (Exception objException)
            {

            }
        }

        /// <summary>
        /// Generates the header files for all backend classes.
        /// </summary>
        private void GenerateHeaders()
        {
            List<string> headers = new List<string>();

            for (int i = 0; i < this.BackendClasses.Count; i++)
            {
                for (int j = 0; j < this.BackendClasses[i].Headers.Count; j++)
                {
                    headers.Add(this.BackendClasses[i].Headers[j]);
                }
            }

            headers.Sort();

            string previouslyIncludedHeader = string.Empty;
            for (int i = 0; i < headers.Count; i++)
            {
                if (headers[i] != previouslyIncludedHeader)
                {
                    previouslyIncludedHeader = headers[i];
                    this.AppendLine("#include \"" + headers[i] + "\"");
                }
            }
        }

        /// <summary>
        /// Generates the code for the backend classes.
        /// </summary>
        private void GenerateBackendClasses()
        {
            for (int i = 0; i < this.BackendClasses.Count; i++)
            {
                this.Append(this.BackendClasses[i].CodeDeclaration);
            }

            for (int i = 0; i < this.BackendClasses.Count; i++)
            {
                this.Append(this.BackendClasses[i].CodeDefinition);
            }
        }

        /// <summary>
        /// Generates class declaration.
        /// </summary>
        /// <param name="node">Class to generate.</param>
        private void GenerateClassDeclaration(ClassDefinition node)
        {
            if (node.IsPrimitive)
            {
                return;
            }

            if (node.IsBackend)
            {
                if (node.Name.Text.IndexOf("_list") > 0)
                {
                    string listType = node.Name.Text.Substring(0, node.Name.Text.IndexOf("_list"));
                    this.AppendLine("class " + node.Name.Text + " : public list<" + listType + "> {};");
                    return;
                }

                if (this.BackendClasses.Find(x => x.Classname == node.Name.Text) == null)
                {
                    throw new Exception("Backend class implementation not found.");
                }
            }
            else
            {
                this.StartLine();
                this.Append("class " + node.Name.Text + ((node.ClassBase != null) ? " : public " + node.ClassBase.Text : string.Empty));

                this.StartBlock();

                MemberDefinition[] members = node.Members.OrderBy(x => x.AccessorType).ToArray();

                MemberAccessorType? currentAccessorType = null;

                for (int i = 0; i < members.Length; i++)
                {
                    if (members[i].AccessorType != currentAccessorType)
                    {
                        currentAccessorType = members[i].AccessorType;
                        this.AppendLine(currentAccessorType.ToString().ToLower() + ":");
                    }

                    if ((members[i] as FieldDefinition) != null)
                    {
                        this.GenerateClassFieldDeclaration(members[i] as FieldDefinition);
                        continue;
                    }

                    if ((members[i] as MethodDefinition) != null)
                    {
                        MethodDefinition method = members[i] as MethodDefinition;
                        if (method.StaticType == MemberStaticType.Static && method.Type.Text == "constructor")
                        {
                            this.staticClassConstructors.Add(node.Name.Text + "::_static_init();");
                        }

                        this.StartLine();
                        this.GenerateClassMethodDeclaration(members[i] as MethodDefinition);
                        this.Append(";");
                        this.EndLine();
                        continue;
                    }

                    if ((members[i] as OperatorDefinition) != null)
                    {
                        this.StartLine();
                        this.GenerateClassOperatorDeclaration(members[i] as OperatorDefinition);
                        this.Append(";");
                        this.EndLine();
                        continue;
                    }
                }

                this.EndBlock();
                this.Append(";");
            }
        }

        /// <summary>
        /// Generates class definition.
        /// </summary>
        /// <param name="node">Class to generate.</param>
        private void GenerateClassDefinition(ClassDefinition node)
        {
            if (node.IsBackend)
            {
                if (node.IsPrimitive)
                {
                    return;
                }

                if (this.BackendClasses.Find(x => x.Classname == node.Name.Text) == null)
                {
                    throw new Exception("Backend class implementation not found.");
                }
            }
            else
            {
                MemberDefinition[] members = node.Members.ToArray();

                for (int i = 0; i < members.Length; i++)
                {
                    if ((members[i] as FieldDefinition) != null)
                    {
                        if (members[i].StaticType != MemberStaticType.Static)
                        {
                            continue;
                        }

                        this.StartLine();
                        this.GenerateClassFieldDefinition(members[i] as FieldDefinition);
                        this.Append(";");
                        this.EndLine();

                        continue;
                    }

                    if ((members[i] as MethodDefinition) != null)
                    {
                        this.StartLine();
                        this.GenerateClassMethodDefinition(members[i] as MethodDefinition);
                        this.EndLine();
                        continue;
                    }

                    if ((members[i] as OperatorDefinition) != null)
                    {
                        this.StartLine();
                        this.GenerateClassOperatorDefinition(members[i] as OperatorDefinition);
                        this.EndLine();
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Generates field declaration.
        /// </summary>
        /// <param name="field">Class field to generate.</param>
        private void GenerateClassFieldDeclaration(FieldDefinition field)
        {
            foreach (FieldAtom atom in field.Atoms)
            {
                this.StartLine();

                if (field.StaticType == MemberStaticType.Static)
                {
                    this.Append(field.StaticType.ToString().ToLower() + " ");
                }

                if (field.ModifierType != LanguageCompiler.Nodes.ClassMembers.MemberModifierType.Normal)
                {
                    this.Append(field.ModifierType.ToString().ToLower() + " ");
                }

                if (atom.Value != null)
                {
                    if (this.GetTypeOfNode(atom.Value).IsPrimitive || !this.enableSharedPtrSupport)
                    {
                        this.Append(atom.Value.ExpressionType.GetName() + " ");
                    }
                    else
                    {
                        this.Append("shared_ptr<" + atom.Value.ExpressionType.GetName() + "> ");
                    }
                }
                else
                {
                    if (this.GetTypeOfNode(field.Type).IsPrimitive || !this.enableSharedPtrSupport)
                    {
                        this.Append(field.Type.Text + " ");
                    }
                    else
                    {
                        this.Append("shared_ptr<" + field.Type.Text + "> ");
                    }
                }

                this.Append(atom.Name.Text);

                this.Append(";");
                this.EndLine();
            }
        }

        /// <summary>
        /// Generates field definition.
        /// </summary>
        /// <param name="field">Class field to generate.</param>
        private void GenerateClassFieldDefinition(FieldDefinition field)
        {
            ////UNUSED
        }

        /// <summary>
        /// Generates method header.
        /// </summary>
        /// <param name="node">Class method to generate.</param>
        private void GenerateClassMethodDeclaration(MethodDefinition node)
        {
            if (node.StaticType == MemberStaticType.Static)
            {
                this.Append(node.StaticType.ToString().ToLower() + " ");
            }

            if (node.ModifierType != LanguageCompiler.Nodes.ClassMembers.MemberModifierType.Normal)
            {
                this.Append("virtual" + " ");
            }

            if (node.Name.Text != "constructor")
            {
                if (node.Type.Text == "void")
                {
                    this.Append("void ");
                }
                else
                {
                    if (this.GetTypeOfNode(node.Type).IsPrimitive)
                    {
                        this.Append(node.Type.Text + " ");
                    }
                    else
                    {
                        if (this.enableSharedPtrSupport)
                        {
                            this.Append("shared_ptr<" + node.Type.Text + "> ");
                        }
                        else
                        {
                            this.Append(node.Type.Text + "& ");
                        }
                    }
                }
            }
            else
            {
                if (node.StaticType == MemberStaticType.Static)
                {
                    this.Append("void ");
                }
            }

            if (node.Name.Text == "constructor")
            {
                if (node.StaticType == MemberStaticType.Static)
                {
                    this.Append("_static_init");
                }
                else
                {
                    this.Append(node.Type.Text);
                }
            }
            else
            {
                this.Append(node.Name.Text);
            }

            this.GenerateMethodHeaderParameters(node.Parameters);
        }

        /// <summary>
        /// Generates method definition.
        /// </summary>
        /// <param name="node">Class method to generate.</param>
        private void GenerateClassMethodDefinition(MethodDefinition node)
        {
            this.AppendLine(string.Empty);

            if (node.ModifierType == MemberModifierType.Abstract)
            {
                return;
            }

            if (node.Name.Text != "constructor")
            {
                if (node.Type.Text == "void")
                {
                    this.Append("void ");
                }
                else
                {
                    if (this.GetTypeOfNode(node.Type).IsPrimitive)
                    {
                        this.Append(node.Type.Text + " ");
                    }
                    else
                    {
                        if (this.enableSharedPtrSupport)
                        {
                            this.Append("shared_ptr<" + node.Type.Text + "> ");
                        }
                        else
                        {
                            this.Append(node.Type.Text + "& ");
                        }
                    }
                }
            }
            else
            {
                if (node.StaticType == MemberStaticType.Static)
                {
                    this.Append("void ");
                }
            }

            this.Append(node.Parent.Name.Text + "::");

            if (node.Name.Text == "constructor")
            {
                if (node.StaticType == MemberStaticType.Static)
                {
                    this.Append("_static_init");
                }
                else
                {
                    this.Append(node.Type.Text);
                }
            }
            else
            {
                this.Append(node.Name.Text);
            }

            this.GenerateMethodHeaderParameters(node.Parameters);

            if (node.Name.Text == "constructor")
            {
                this.StartBlock();
                if (node.StaticType == MemberStaticType.Static)
                {
                    this.GenerateStaticMembersInitializers(node.Parent);
                }
                else
                {
                    this.GenerateNoneStaticMembersInitializers(node.Parent);
                }
            }
            
            this.GenerateBlockStatement(node.Block);

            if (node.Name.Text == "constructor")
            {
                this.EndBlock();
            }
        }

        /// <summary>
        /// Generates code for the given block statement.
        /// </summary>
        /// <param name="node">Block to generate.</param>
        private void GenerateBlockStatement(LanguageCompiler.Nodes.Statements.Block node)
        {
            this.StartBlock();

            if (node == null)
            {
                this.EndBlock();
                return;
            }

            for (int i = 0; i < node.Statements.Count; i++)
            {
                this.StartLine();

                BaseNode statement = node.Statements[i];
                if (statement as DeclarationStatement != null)
                {
                    this.GenerateDeclarationStatement(statement as DeclarationStatement);
                    this.Append(";");
                }

                if (statement as BreakStatement != null)
                {
                    this.GenerateBreakStatement(statement as BreakStatement);
                    this.Append(";");
                }

                if (statement as ContinueStatement != null)
                {
                    this.GenerateContinueStatement(statement as ContinueStatement);
                    this.Append(";");
                }

                if (statement as ReturnStatement != null)
                {
                    this.GenerateReturnStatement(statement as ReturnStatement);
                    this.Append(";");
                }

                if (statement as DoWhileStatement != null)
                {
                    this.GenerateDoWhileStatement(statement as DoWhileStatement);
                    this.Append(";");
                }

                if (statement as ForStatement != null)
                {
                    this.GenerateForStatement(statement as ForStatement);
                }

                if (statement as IfStatement != null)
                {
                    this.GenerateIfStatement(statement as IfStatement);
                }

                if (statement as WhileStatement != null)
                {
                    this.GenerateWhileStatement(statement as WhileStatement);
                }

                if (statement as ExpressionStatement != null)
                {
                    this.GenerateExpressionNode((statement as ExpressionStatement).Expresssion as ExpressionNode);
                    this.Append(";");
                }

                this.EndLine();
            }

            this.EndBlock();
        }

        /// <summary>
        /// Generates code for declaration statements.
        /// </summary>
        /// <param name="node">Declaration statement to generate.</param>
        private void GenerateDeclarationStatement(LanguageCompiler.Nodes.Statements.DeclarationStatement node)
        {
            for (int i = 0; i < node.Atoms.Count; i++)
            {
                this.StartLine();

                if (node.Atoms[i].Value != null)
                {
                    if (this.GetTypeOfNode(node.Atoms[i].Value).IsPrimitive || !this.enableSharedPtrSupport)
                    {
                        this.Append(node.Atoms[i].Value.ExpressionType.GetName() + " ");
                    }
                    else
                    {
                        this.Append("shared_ptr<" + node.Atoms[i].Value.ExpressionType.GetName() + "> ");
                    }
                }
                else
                {
                    if (this.GetTypeOfNode(node.Type).IsPrimitive || !this.enableSharedPtrSupport)
                    {
                        this.Append(node.Type.Text + " ");
                    }
                    else
                    {
                        this.Append("shared_ptr<" + node.Type.Text + "> ");
                    }
                }

                this.Append(node.Atoms[i].Name.Text);

                if (node.Atoms[i].Value != null)
                {
                    this.Append(" = ");
                    this.GenerateExpressionNode(node.Atoms[i].Value);
                }

                this.Append(";");
                this.EndLine();
            }
        }

        /// <summary>
        /// Generates code for break statement.
        /// </summary>
        /// <param name="node">Break statement to generate.</param>
        private void GenerateBreakStatement(LanguageCompiler.Nodes.Statements.CommandStatements.BreakStatement node)
        {
            this.Append("break");
        }

        /// <summary>
        /// Generates code for continue statement.
        /// </summary>
        /// <param name="node">Continue statement to generate.</param>
        private void GenerateContinueStatement(LanguageCompiler.Nodes.Statements.CommandStatements.ContinueStatement node)
        {
            this.Append("continue");
        }

        /// <summary>
        /// Generates code for return statement.
        /// </summary>
        /// <param name="node">return statement to generate.</param>
        private void GenerateReturnStatement(LanguageCompiler.Nodes.Statements.CommandStatements.ReturnStatement node)
        {
            this.Append("return");
            if (node.Expression != null)
            {
                this.Append(" ");
                this.GenerateExpressionNode(node.Expression);
            }
        }

        /// <summary>
        /// Generates code for do while statement.
        /// </summary>
        /// <param name="node">Do while statement to generate.</param>
        private void GenerateDoWhileStatement(LanguageCompiler.Nodes.Statements.ControlStatements.DoWhileStatement node)
        {
            this.Append("do");
            this.EndLine();
            this.GenerateBlockStatement(node.Block);
            this.Append(" while(");
            this.GenerateExpressionNode(node.Expression);
            this.Append(")");
        }

        /// <summary>
        /// Generates code for for statement.
        /// </summary>
        /// <param name="node">For statement to generate.</param>
        private void GenerateForStatement(LanguageCompiler.Nodes.Statements.ControlStatements.ForStatement node)
        {
            this.StartLine();
            this.Append("for (");

            if (node.FirstPartList != null)
            {
                bool firstField = true;
                for (int i = 0; i < node.FirstPartList.Count; i++)
                {
                    if (!firstField)
                    {
                        this.Append(", ");
                    }

                    firstField = false;
                    this.Append(node.FirstPartList[i].Name.Text);
                }
            }

            this.Append(";");

            if (node.SecondPart != null)
            {
                this.GenerateExpressionNode(node.SecondPart);
            }

            this.Append(";");

            if (node.ThirdPartList != null)
            {
                bool firstExpression = true;
                for (int i = 0; i < node.ThirdPartList.Count; i++)
                {
                    if (!firstExpression)
                    {
                        this.Append(", ");
                    }

                    firstExpression = false;
                    this.GenerateExpressionNode(node.ThirdPartList[i]);
                }
            }

            this.Append(")");
            this.GenerateBlockStatement(node.Block);
        }

        /// <summary>
        /// Generates code for if statement.
        /// </summary>
        /// <param name="node">If statement to generate.</param>
        private void GenerateIfStatement(LanguageCompiler.Nodes.Statements.ControlStatements.IfStatement node)
        {
            bool firstIf = true;

            for (int i = 0; i < node.Bodies.Count; i++)
            {
                if (!firstIf)
                {
                    this.StartLine();
                    this.Append(" else ");
                }

                this.Append("if (");
                this.GenerateExpressionNode(node.Bodies[i].Expression);
                this.Append(")");

                this.EndLine();

                this.GenerateBlockStatement(node.Bodies[i].Body);
                firstIf = false;
            }

            if (node.ElseBody != null)
            {
                this.Append(" else ");
                this.GenerateBlockStatement(node.ElseBody);
            }
        }

        /// <summary>
        /// Generates code for while statement.
        /// </summary>
        /// <param name="node">While statement to generate.</param>
        private void GenerateWhileStatement(LanguageCompiler.Nodes.Statements.ControlStatements.WhileStatement node)
        {
            this.Append("while (");
            this.GenerateExpressionNode(node.Expression);
            this.Append(")");
            this.StartLine();
            this.GenerateBlockStatement(node.Body);
        }

        /// <summary>
        /// Generates expression from expression node.
        /// </summary>
        /// <param name="node">Expression node to generate.</param>
        private void GenerateExpressionNode(ExpressionNode node)
        {
            if (node as Identifier != null)
            {
                if (this.GetTypeOfNode(node).IsPrimitive || !this.enableSharedPtrSupport)
                {
                    this.Append((node as Identifier).Text);
                }
                else
                {
                    this.Append("(*(" + (node as Identifier).Text + ".get()))");
                }
            }

            if (node as Literal != null)
            {
                this.Append((node as Literal).Data);
            }

            if (node as BinaryExpression != null)
            {
                this.GenerateBinaryExpression(node as BinaryExpression);
            }

            if (node as EmbeddedIf != null)
            {
                this.GenerateEmbeddedIf(node as EmbeddedIf);
            }

            if (node as PostfixExpression != null)
            {
                this.GeneratePostfixExpression(node as PostfixExpression);
            }

            if (node as UnaryExpression != null)
            {
                this.GenerateUnaryExpression(node as UnaryExpression);
            }

            if (node as CompoundExpression != null)
            {
                this.GenerateCompoundExpression(node as CompoundExpression);
            }

            if (node as InvocationExpression != null)
            {
                this.GenerateInvocationExpression(node as InvocationExpression);
            }

            if (node as ObjectCreationExpression != null)
            {
                this.GenerateObjectCreationExpression(node as ObjectCreationExpression);
            }
        }

        /// <summary>
        /// Generates binary expressions.
        /// </summary>
        /// <param name="node">Binary expression to generate.</param>
        private void GenerateBinaryExpression(LanguageCompiler.Nodes.Expressions.Basic.BinaryExpression node)
        {
            this.GenerateExpressionNode(node.LHS);
            this.Append(node.OperatorDefined);
            this.GenerateExpressionNode(node.RHS);
        }

        /// <summary>
        /// Generates embedded if expression.
        /// </summary>
        /// <param name="node">The embedded if to generate.</param>
        private void GenerateEmbeddedIf(LanguageCompiler.Nodes.Expressions.Basic.EmbeddedIf node)
        {
            this.Append("(");
            this.GenerateExpressionNode(node.Condition);
            this.Append(") ? (");
            this.GenerateExpressionNode(node.TrueChoice);
            this.Append(") : (");
            this.GenerateExpressionNode(node.FalseChoice);
            this.Append(")");
        }

        /// <summary>
        /// Generates postfix expressions.
        /// </summary>
        /// <param name="node">Postfix expression to make.</param>
        private void GeneratePostfixExpression(LanguageCompiler.Nodes.Expressions.Basic.PostfixExpression node)
        {
            this.GenerateExpressionNode(node.LHS);
            this.Append(node.OperatorDefined);
        }

        /// <summary>
        /// Generates unary expressions.
        /// </summary>
        /// <param name="node">Unary expression to generate.</param>
        private void GenerateUnaryExpression(LanguageCompiler.Nodes.Expressions.Basic.UnaryExpression node)
        {
            if (node.OperatorDefined == "not")
            {
                this.Append("!");
            }
            else
            {
                this.Append(node.OperatorDefined);
            }
            
            this.GenerateExpressionNode(node.RHS);
        }

        /// <summary>
        /// Generates compound expressions.
        /// </summary>
        /// <param name="node">Compound expression to generate.</param>
        private void GenerateCompoundExpression(LanguageCompiler.Nodes.Expressions.Complex.CompoundExpression node)
        {
            this.GenerateExpressionNode(node.LHS);

            if (node.LHS as Identifier != null)
            {
                if ((node.LHS as Identifier).Text == "this")
                {
                    this.Append("->");
                    this.Append(node.RHS.Text);
                    return;
                }
            }

            if (this.GetTypeOfNode(node.LHS).IsPrimitive || !this.enableSharedPtrSupport)
            {
                this.Append(".");
            }
            else
            {
                this.Append("->");
            }

            this.Append(node.RHS.Text);
        }

        /// <summary>
        /// Generates invocation expressions.
        /// </summary>
        /// <param name="node">Invocation expression to generate.</param>
        private void GenerateInvocationExpression(LanguageCompiler.Nodes.Expressions.Complex.InvocationExpression node)
        {
            this.GenerateExpressionNode(node.LHS);
            this.Append("(");

            bool firstExpression = true;
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                if (!firstExpression)
                {
                    this.Append(", ");
                }

                this.GenerateExpressionNode(node.Arguments[i]);
                firstExpression = false;
            }

            this.Append(")");
        }

        /// <summary>
        /// Generates object creation expressions.
        /// </summary>
        /// <param name="node">Object creation expression to create.</param>
        private void GenerateObjectCreationExpression(LanguageCompiler.Nodes.Expressions.Complex.ObjectCreationExpression node)
        {
            if (this.GetTypeOfNode(node).IsPrimitive || !this.enableSharedPtrSupport)
            {
                this.Append(node.Type.Text + "(");
            }
            else
            {
                this.Append("shared_ptr<" + node.Type.Text + ">(new " + node.Type.Text + "(");
            }

            bool firstArgument = true;
            for (int i = 0; i < node.Arguments.Count; i++)
            {
                if (!firstArgument)
                {
                    this.Append(", ");
                }

                firstArgument = false;
                this.GenerateExpressionNode(node.Arguments[i]);
            }

            if (this.GetTypeOfNode(node).IsPrimitive || !this.enableSharedPtrSupport)
            {
                this.Append(")");
            }
            else
            {
                this.Append("))");
            }
        }

        /// <summary>
        /// Generates static members initializers.
        /// </summary>
        /// <param name="node">Class to be used.</param>
        private void GenerateStaticMembersInitializers(ClassDefinition node)
        {
            for (int i = 0; i < node.Members.Count; i++)
            {
                if (node.Members[i] as FieldDefinition != null)
                {
                    FieldDefinition field = node.Members[i] as FieldDefinition;
                    
                    if (field.StaticType == MemberStaticType.Static)
                    {
                        for (int j = 0; j < field.Atoms.Count; j++)
                        {
                            if (field.Atoms[j].Value != null)
                            {
                                this.StartLine();
                                this.Append(field.Atoms[j].Name.Text + " = ");
                                this.GenerateExpressionNode(field.Atoms[j].Value);
                                this.Append(";");
                                this.EndLine();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generate none static members initializers.
        /// </summary>
        /// <param name="node">Class to be used.</param>
        private void GenerateNoneStaticMembersInitializers(ClassDefinition node)
        {
            for (int i = 0; i < node.Members.Count; i++)
            {
                if (node.Members[i] as FieldDefinition != null)
                {
                    FieldDefinition field = node.Members[i] as FieldDefinition;

                    if (field.StaticType != MemberStaticType.Static)
                    {
                        for (int j = 0; j < field.Atoms.Count; j++)
                        {
                            if (field.Atoms[j].Value != null)
                            {
                                this.StartLine();
                                this.Append(field.Atoms[j].Name.Text + " = ");
                                this.GenerateExpressionNode(field.Atoms[j].Value);
                                this.Append(";");
                                this.EndLine();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates operator header.
        /// </summary>
        /// <param name="node">Class operator to generate.</param>
        private void GenerateClassOperatorDeclaration(OperatorDefinition node)
        {
            if (node.ModifierType != LanguageCompiler.Nodes.ClassMembers.MemberModifierType.Normal)
            {
                this.Append("virtual" + " ");
            }

            if (node.Type.Text == "void")
            {
                this.Append("void ");
            }
            else
            {
                if (this.GetTypeOfNode(node.Type).IsPrimitive || !this.enableSharedPtrSupport)
                {
                    this.Append(node.Type.Text);

                    switch (node.OperatorDefined)
                    {
                        case "+=":
                        case "-=":
                        case "*=":
                        case "/=":
                        case "^=":
                            this.Append("& ");
                            break;
                        default:
                            this.Append(" ");
                            break;
                    }
                }
                else
                {
                    this.Append("shared_ptr<" + node.Type.Text + "> ");
                }
            }

            this.Append("operator");
            this.Append(node.OperatorDefined);
            this.GenerateMethodHeaderParameters(node.Parameters);
        }

        /// <summary>
        /// Generates operator definition.
        /// </summary>
        /// <param name="node">Class operator to generate.</param>
        private void GenerateClassOperatorDefinition(OperatorDefinition node)
        {
            if (node.Type.Text == "void")
            {
                this.Append("void ");
            }
            else
            {
                if (this.GetTypeOfNode(node.Type).IsPrimitive || !this.enableSharedPtrSupport)
                {
                    this.Append(node.Type.Text);

                    switch (node.OperatorDefined)
                    {
                        case "+=":
                        case "-=":
                        case "*=":
                        case "/=":
                        case "^=":
                            this.Append("& ");
                            break;
                        default:
                            this.Append(" ");
                            break;
                    }
                }
                else
                {
                    this.Append("shared_ptr<" + node.Type.Text + "> ");
                }
            }

            this.Append(node.Parent.Name.Text + "::operator");
            this.Append(node.OperatorDefined);
            this.GenerateMethodHeaderParameters(node.Parameters);
            this.GenerateBlockStatement(node.Block);

            if (node.Type.Text == "constructor")
            {
                this.EndBlock();
            }
        }

        /// <summary>
        /// Returns the class definition of the type of a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>Type's class definition.</returns>
        private ClassDefinition GetTypeOfNode(ExpressionNode node)
        {
            if (node.ExpressionType == null)
            {
                if ((node as Identifier) != null)
                {
                    return CompilerService.Instance.ClassesList[(node as Identifier).Text];
                }
                else if ((node as Literal) != null)
                {
                    node.ExpressionType = node.GetExpressionType(null);
                    return CompilerService.Instance.ClassesList[(node as Literal).Type];
                }
                else
                {
                    throw new Exception("CANNOT");
                }
            }
            
            return CompilerService.Instance.ClassesList[node.ExpressionType.GetName()];
        }

        /// <summary>
        /// Adds a default constructor if didn't exist, to call field initializers.
        /// </summary>
        /// <param name="node">The class definition</param>
        private void AddDefaultConstructor(ClassDefinition node)
        {
            MethodDefinition[] constructors = node.Members.Where(x => (x as MethodDefinition) != null).Where(x => ((MethodDefinition)x).Name.Text == "constructor" && x.StaticType == MemberStaticType.Normal).Cast<MethodDefinition>().ToArray();
            MethodDefinition[] staticConstructors = node.Members.Where(x => (x as MethodDefinition) != null).Where(x => ((MethodDefinition)x).Name.Text == "constructor" && x.StaticType == MemberStaticType.Static).Cast<MethodDefinition>().ToArray();

            if (constructors.Length != 0 && staticConstructors.Length != 0)
            {
                return;
            }

            FieldDefinition[] fields = node.Members.Where(x => (x as FieldDefinition) != null).Cast<FieldDefinition>().ToArray();

            bool constructorNeeded = false;
            bool staticConstructorNeeded = false;

            for (int i = 0; i < fields.Length; i++)
            {
                for (int j = 0; j < fields[i].Atoms.Count; j++)
                {
                    if (fields[i].Atoms[j].Value != null)
                    {
                        if (fields[i].StaticType == MemberStaticType.Normal)
                        {
                            constructorNeeded = true;
                        }
                        else
                        {
                            staticConstructorNeeded = true;
                        }
                    }
                }
            }

            if (constructorNeeded && constructors.Length == 0)
            {
                MethodDefinition constructor = new MethodDefinition(node, "constructor", node.Name.Text, MemberAccessorType.Public, MemberModifierType.Normal, MemberStaticType.Normal);
                node.Members.Add(constructor);
            }

            if (staticConstructorNeeded && staticConstructors.Length == 0)
            {
                MethodDefinition constructor = new MethodDefinition(node, "constructor", node.Name.Text, MemberAccessorType.Public, MemberModifierType.Normal, MemberStaticType.Static);
                node.Members.Add(constructor);
            }
        }

        /// <summary>
        /// Generates the parameters of a method header.
        /// </summary>
        /// <param name="parameters">The parameters</param>
        private void GenerateMethodHeaderParameters(List<Parameter> parameters)
        {
            this.Append("(");

            for (int i = 0; i < parameters.Count; i++)
            {
                if (i != 0)
                {
                    this.Append(", ");
                }

                if (this.GetTypeOfNode(parameters[i].Type).IsPrimitive)
                {
                    this.Append(parameters[i].Type.Text + " " + parameters[i].Name.Text);
                }
                else
                {
                    if (this.enableSharedPtrSupport)
                    {
                        this.Append("shared_ptr<" + parameters[i].Type.Text + "> " + parameters[i].Name.Text);
                    }
                    else
                    {
                        this.Append(parameters[i].Type.Text + "& " + parameters[i].Name.Text);
                    }
                }
            }

            this.Append(")");
        }
    }
}
