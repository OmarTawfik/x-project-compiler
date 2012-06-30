namespace FinalSemantics
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using LanguageCompiler;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.TopLevel;
    using LanguageTranslator.Translators.CPP;

    /// <summary>
    /// Holds all events of the TranslatorForm.
    /// </summary>
    public partial class TranslatorForm : Form
    {
        /// <summary>
        /// The c++ translator object.
        /// </summary>
        private PCCPPTranslator cpp = new PCCPPTranslator();

        /// <summary>
        /// Initializes a new instance of the TranslatorForm class.
        /// </summary>
        public TranslatorForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// This event fires when button1 is clicked.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Event Arguments.</param>
        private void Button1_Click(object sender, EventArgs e)
        {
            CompilerService.Instance.Clear();
            CompilerService.Instance.ParseFile(this.richTextBox1.Text, "main.x");

            for (int i = 0; i < this.cpp.BackendClasses.Count; i++)
            {
                CompilerService.Instance.ParseFile(this.cpp.BackendClasses[i].XlangCode, this.cpp.BackendClasses[i].Classname);
                if (CompilerService.Instance.Errors.Count != 0)
                {
                    Console.Write("BLA");
                }
            }

            CompilerService.Instance.CheckSemantics();

            if (CompilerService.Instance.Errors.Count == 0)
            {
                List<ClassDefinition> classList = new List<ClassDefinition>();

                foreach (ClassDefinition classdef in CompilerService.Instance.ClassesList.Values)
                {
                    classList.Add(classdef);
                }

                this.cpp.Translate(classList);

                this.richTextBox2.Text = this.cpp.GeneratedCode.ToString();

                this.cpp.Build();
                this.cpp.Run();
            } else {
                this.richTextBox2.Text = "Errors Found!" + Environment.NewLine + Environment.NewLine;

                foreach (CompilerError error in CompilerService.Instance.Errors)
	            {
                    this.richTextBox2.Text += Environment.NewLine + "class " + error.ClassName + ":";
                    this.richTextBox2.Text += error.Message + ". At line(" + error.StartingLocation.Line + "), column(" + error.StartingLocation.Column + ".";
	            }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
