using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LanguageCompiler;
using LanguageCompiler.Nodes.TopLevel;

namespace FinalSemantics
{
    public partial class TranslatorForm : Form
    {
        public TranslatorForm()
        {
            InitializeComponent();
            cpp = new LanguageTranslator.Translators.CPP.PCCPPTranslator();
        }

        LanguageTranslator.Translators.CPP.PCCPPTranslator cpp;
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
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

                    cpp.Translate(classList);

                    this.textBox1.Text = cpp.GeneratedCode.ToString();
                }
            }
            catch (Exception ex)
            {
                this.textBox1.Text = "EXCEPTION!!" + Environment.NewLine;
                this.textBox1.Text += ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }
    }
}
