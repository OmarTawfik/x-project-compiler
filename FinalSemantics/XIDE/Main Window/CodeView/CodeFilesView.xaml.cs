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
using System.IO;
using XIDE.Manager;

namespace XIDE.Main_Window.CodeView
{
    /// <summary>
    /// Interaction logic for CodeFilesView.xaml
    /// </summary>
    public partial class CodeFilesView : UserControl
    {
        public delegate void CodeFileItemSelectedDelegate(CodeFileItem item);

        public event CodeFileItemSelectedDelegate CodeFileItemSelected;

        private List<CodeFileItem> codeFiles;

        public CodeFilesView()
        {
            InitializeComponent();
        }

        public void ReloadFiles()
        {
            if (ProjectsManager.CurrentProject == null)
            {
                return;
            }

            this.codeFiles = new List<CodeFileItem>();

            List<string> files = ProjectsManager.CurrentProject.CodeFiles;

            this.codeFiles.Clear();
            this.codeFilesContainer.Children.Clear();

            for (int i = 0; i < files.Count; i++)
            {
                string name = System.IO.Path.GetFileName(files[i]);
                CodeFileItem item = new CodeFileItem(name, files[i]);
                this.codeFiles.Add(item);
                item.MouseDown += this.CodeFileSelected;
                this.codeFilesContainer.Children.Add(item);
            }

            if (files.Count != 0)
            {
                this.CodeFileSelected(this.codeFiles[0], null);
            }
        }

        private void CodeFileSelected(object sender, MouseButtonEventArgs e)
        {
            CodeFileItem codeItem = sender as CodeFileItem;
            foreach (CodeFileItem item in this.codeFiles)
            {
                IntergalacticControls.UIHelpers.FadeOutAnimation(item.selectionRectangle, null, 0.3);
                item.labelCodeFileName.Foreground = Brushes.Black;
            }

            IntergalacticControls.UIHelpers.FadeInAnimation(codeItem.selectionRectangle, 0, 0.05);
            codeItem.labelCodeFileName.Foreground = Brushes.White;
            this.CodeFileItemSelected(codeItem);
        }

        private void plusButton1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int counter = 1;
            FileStream file;
            RetryCreatingNewFile:
            try
            {
                file = new FileStream(ProjectsManager.CurrentProject.GetFullPath("/New code file " + counter + ".x", ProjectResourceType.Code), FileMode.CreateNew);
            }
            catch (Exception)
            {
                counter++;
                goto RetryCreatingNewFile;
            }

            file.Close();
            ProjectsManager.CurrentProject.CodeFiles.Add(file.Name);
            this.ReloadFiles();
        }

        private void saveButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (CodeFileItem item in this.codeFiles)
            {
                item.CodeEditor.Save(item.CodeFileLocation);
            }

            ProjectsManager.CurrentProject.SaveProject();
        }

        private void buildButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProjectsManager.BuildAndRunCurrentProject();
        }
    }
}
