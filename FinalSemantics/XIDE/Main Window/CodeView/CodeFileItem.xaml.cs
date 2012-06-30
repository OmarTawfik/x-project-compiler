using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XIDE.Manager;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;

namespace XIDE.Main_Window.CodeView
{
    /// <summary>
    /// Interaction logic for CodeFileItem.xaml
    /// </summary>
    public partial class CodeFileItem : UserControl
    {
        private string codeFileName, codeFileLocation;

        private TextEditor codeEditor;

        public CodeFileItem(string codeFileName, string codeFileLocation)
        {
            InitializeComponent();
            this.codeFileName = codeFileName;
            this.codeFileLocation = codeFileLocation;

            this.codeEditor = new TextEditor();

            this.codeEditor.FontFamily = new FontFamily("Consolas");
            this.codeEditor.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.codeEditor.VerticalAlignment = VerticalAlignment.Stretch;
            this.codeEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
            this.codeEditor.FontSize = 14;
            this.codeEditor.ShowLineNumbers = true;

            this.codeEditor.Load(codeFileLocation);
            this.codeEditor.Tag = codeFileLocation;

            this.codeFileName = codeFileName;
            this.codeFileLocation = codeFileLocation;
            this.labelCodeFileName.Text = codeFileName;
        }

        public string CodeFileName
        {
            get { return this.codeFileName; }
        }

        public string CodeFileLocation
        {
            get { return this.codeFileLocation; }
        }

        public TextEditor CodeEditor
        {
            get { return this.codeEditor; }
        }

        private void labelCodeFileName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.labelCodeFileName.IsReadOnly = false;
            this.labelCodeFileName.IsReadOnly = false;
            this.labelCodeFileName.SelectAll();
        }

        private void labelCodeFileName_LostFocus(object sender, RoutedEventArgs e)
        {
            RenameCodeFile(this.labelCodeFileName.Text);
        }

        private void RenameCodeFile(string filename)
        {
            this.labelCodeFileName.IsReadOnly = true;
            for (int i = 0; i < ProjectsManager.CurrentProject.CodeFiles.Count; i++)
			{
                if (ProjectsManager.CurrentProject.CodeFiles[i] == this.codeFileLocation)
                {
                    try
                    {
                        string name = System.IO.Path.GetFileName(this.codeFileLocation);
                        string path = this.codeFileLocation.Substring(0, this.codeFileLocation.IndexOf(name));
                        System.IO.File.Move(this.codeFileLocation, path + filename);

                        for (int j = 0; j < ProjectsManager.CurrentProject.CodeFiles.Count; j++)
			            {
                            if (ProjectsManager.CurrentProject.CodeFiles[j] == this.codeFileLocation)
                            {
                                ProjectsManager.CurrentProject.CodeFiles[j] = path + filename;
                                this.codeFileLocation = path + filename;
                                this.codeFileName = filename;
                                this.labelCodeFileName.Text = this.codeFileName;
                            }
			            }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error has occured while renaming the file.");
                        this.labelCodeFileName.Text = this.codeFileName;
                    }
                }
			}
        }

        private void labelCodeFileName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.RenameCodeFile(this.labelCodeFileName.Text);
            }
        }
    }
}
