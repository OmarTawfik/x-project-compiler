namespace IDE
{
    using System.Windows;
    using IDE.Classes;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        /// <param name="settings">Project settings to be used.</param>
        public MainWindow(ProjectSettingsFile settings)
        {
            this.InitializeComponent();
        }
    }
}
