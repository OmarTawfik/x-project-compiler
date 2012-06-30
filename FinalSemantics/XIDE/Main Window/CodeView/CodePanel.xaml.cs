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

namespace XIDE.Main_Window.CodeView
{
    /// <summary>
    /// Interaction logic for CodePanel.xaml
    /// </summary>
    public partial class CodePanel : UserControl
    {
        public CodePanel()
        {
            InitializeComponent();
            this.codeFilesView1.CodeFileItemSelected += new CodeFilesView.CodeFileItemSelectedDelegate(codeFilesView1_CodeFileItemSelected);
            this.codeFilesView1.ReloadFiles();
            ProjectsManager.CurrentProjectChanged += this.CurrentProjectChanged;
        }

        private void CurrentProjectChanged(ProjectSettings project)
        {
            this.codeFilesView1.ReloadFiles();
        }

        void codeFilesView1_CodeFileItemSelected(CodeFileItem item)
        {
            this.codeContainer.Children.Clear();
            this.codeContainer.Children.Add(item.CodeEditor);
        }
    }
}
