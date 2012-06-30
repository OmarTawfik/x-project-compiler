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
using System.Windows.Shapes;
using XIDE.Manager;
using XIDE.Main_Window.CodeView;

namespace XIDE.Main_Window.WelcomeScreen
{
    /// <summary>
    /// Interaction logic for WelcomeScreen.xaml
    /// </summary>
    public partial class WelcomeScreen : UserControl
    {
        public WelcomeScreen()
        {
            InitializeComponent();
            this.recentProjectsContainer.Children.Add(new RecentProjectField("Open a project..", null));

            foreach (ProjectData project in ProjectsManager.RecentProjects)
            {
                this.recentProjectsContainer.Children.Add(new RecentProjectField(project.ProjectName, project.ProjectLocation));
            }
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            ProjectsManager.CreateNewProject("PAAAooo", "C:\\Users\\Muhammad\\Desktop\\projectTest.xproject");

            CodePanel view = new CodePanel();
            NavigationWindow.CurrentNavigationWindow.PushView(view, false);
            NavigationWindow.CurrentNavigationWindow.CommitNavigation();
        }
    }
}
