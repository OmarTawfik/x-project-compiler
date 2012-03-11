namespace FinalSemantics
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using LanguageCompiler;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.TopLevel;

    /// <summary>
    /// MainForm class events.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// This event fires when the text in codeRichTextBox is changed.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Event Arguments.</param>
        private void CodeRichTextBox_TextChanged(object sender, System.EventArgs e)
        {
            CompilerService.Instance.Clear();
            CompilerService.Instance.ParseFile(this.codeRichTextBox.Text, "main.x");
            CompilerService.Instance.CheckSemantics();

            this.syntaxTreeView.Nodes.Clear();
            this.errorsGridView.Rows.Clear();

            if (CompilerService.Instance.Errors.Count == 0)
            {
                foreach (KeyValuePair<string, ClassDefinition> pair in CompilerService.Instance.ClassesList)
                {
                    this.syntaxTreeView.Nodes.Add(pair.Value.GetGUINode());
                }

                this.syntaxTreeView.ExpandAll();
            }
            else
            {
                this.errorsGridView.Rows.Add(CompilerService.Instance.Errors.Count);
                for (int i = 0; i < CompilerService.Instance.Errors.Count; i++)
                {
                    CompilerError error = CompilerService.Instance.Errors[i];
                    this.errorsGridView.Rows[i].Cells[0].Value = (i + 1).ToString();
                    this.errorsGridView.Rows[i].Cells[1].Value = error.StartingLocation.ToUiString();
                    this.errorsGridView.Rows[i].Cells[2].Value = error.EndingLocation.ToUiString();
                    this.errorsGridView.Rows[i].Cells[3].Value = error.Message;
                }
            }
        }

        /// <summary>
        /// This event fires when the user clicks in a cell in the errorsGridView.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Event Arguments.</param>
        private void ErrorsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CompilerError error = CompilerService.Instance.Errors[e.RowIndex];
            this.codeRichTextBox.Select(
                error.StartingLocation.Position,
                error.EndingLocation.Position - error.StartingLocation.Position + 1);
            this.codeRichTextBox.Focus();
        }
    }
}
