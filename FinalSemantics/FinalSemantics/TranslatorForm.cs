namespace FinalSemantics
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using LanguageCompiler;
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
            CompilerService.Instance.ParseFile(this.textBox2.Text, "main.x");
            CompilerService.Instance.CheckSemantics();

            if (CompilerService.Instance.Errors.Count == 0 || true)
            {
                List<ClassDefinition> classList = new List<ClassDefinition>();

                foreach (ClassDefinition classdef in CompilerService.Instance.ClassesList.Values)
                {
                    classList.Add(classdef);
                }

                this.cpp.Translate(classList);

                this.textBox1.Text = this.cpp.GeneratedCode.ToString();
            }
        }
    }
}
