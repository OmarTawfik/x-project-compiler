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

namespace XIDE.Main_Window.WelcomeScreen
{
    /// <summary>
    /// Interaction logic for RecentProjectField.xaml
    /// </summary>
    public partial class RecentProjectField : UserControl
    {
        private string projectTitle;
        private string projectLocation;

        public RecentProjectField(string projectTitle, string projectLocation)
        {
            InitializeComponent();
            this.projectTitle = projectTitle;
            this.projectLocation = projectLocation;
            this.lablelProjectName.Content = projectTitle;
        }

        public string ProjectTitle
        {
            get { return this.projectTitle; }
        }

        public string ProjectLocation
        {
            get { return this.projectLocation; }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            IntergalacticControls.UIHelpers.FadeInAnimation(this.hoverRectangle, null, 0.25);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            IntergalacticControls.UIHelpers.FadeOutAnimation(this.hoverRectangle, null, 0.25);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.projectLocation == null)
            {
                System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                dialog.Filter = "X-Project Files (.xproject)|*.xproject";
                dialog.DefaultExt = ".xproject";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        ProjectsManager.LoadProjectFromLocation(dialog.FileName);
                        NavigationWindow.CurrentNavigationWindow.PushView(new CodeView.CodePanel(), false);
                        NavigationWindow.CurrentNavigationWindow.CommitNavigation();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error has occred: " + ex.Message);
                    }
                }
            }
            else
            {
                try
                {
                    ProjectsManager.LoadProjectFromLocation(this.projectLocation);
                    NavigationWindow.CurrentNavigationWindow.PushView(new CodeView.CodePanel(), false);
                    NavigationWindow.CurrentNavigationWindow.CommitNavigation();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occred: " + ex.Message);
                }
            }
        }
    }
}
