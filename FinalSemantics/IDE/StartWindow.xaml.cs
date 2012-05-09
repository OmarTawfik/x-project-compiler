namespace IDE
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Xml.Serialization;
    using IDE.DataModels;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the StartWindow class.
        /// </summary>
        public StartWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The Button MouseEnter event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Mouse Event Arguments.</param>
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimationsHelper.MoveUp(sender as Button, 5, 150);
        }
        
        /// <summary>
        /// The Button MouseLeave event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Mouse Event Arguments.</param>
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimationsHelper.MoveDown(sender as Button, 5, 150);
        }

        /// <summary>
        /// The OpenButton Click event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "X-Project Files (.xproject)|*.xproject";
            dialog.DefaultExt = ".xproject";
            
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ProjectSettingsFile));
                    FileStream fileStream = new FileStream(dialog.FileName, FileMode.Open);
                    ProjectSettingsFile project = (ProjectSettingsFile)xml.Deserialize(fileStream);
                    
                    fileStream.Close();
                    new MainWindow(project).Show();
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Error loading the selected project.", "Project Loading Error");
                }
            }
        }

        /// <summary>
        /// The NewButton Click event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWindow dialog = new NewProjectWindow();
            if (dialog.ShowDialog() == true)
            {
                ProjectSettingsFile project = new ProjectSettingsFile(dialog.ProjectName, dialog.ProjectLocation);
                if (File.Exists(project.ProjectFullPath))
                {
                    MessageBox.Show("A project with the same name already exists.", "Project Creation Error");
                }
                else
                {
                    new MainWindow(project).Show();
                    this.Close();
                }
            }
        }
    }
}
