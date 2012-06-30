// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace XIDE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using XIDE.Main_Window;
    using XIDE.Main_Window.CodeView;
    using XIDE.Manager;

    /// <summary>
    /// The main entry class for the application.
    /// </summary>
    public static class Program
    {
        static NavigationWindow wnd;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Button button1 = new Button();
            button1.Click += new System.Windows.RoutedEventHandler(button1_Click);
            wnd = new NavigationWindow(new XIDE.Main_Window.WelcomeScreen.WelcomeScreen(), true);
            wnd.ShowDialog();
        }

        static void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ProjectsManager.CreateNewProject("ProjectBeta1", "C:\\Users\\Muhammad\\Desktop\\ProjectBeta1.xproject");

            CodePanel view = new CodePanel();
            NavigationWindow.CurrentNavigationWindow.PushView(view, false);
            NavigationWindow.CurrentNavigationWindow.CommitNavigation();
            //Image img = new Image();
            //img.Source = new BitmapImage(new Uri("C:\\Users\\Muhammad\\desktop\\Texture.png"));
            //img.Stretch = System.Windows.Media.Stretch.Uniform;
            //wnd.PushView(img, true);
            //wnd.CommitNavigation();
            //img.MouseDown += new System.Windows.Input.MouseButtonEventHandler(img_MouseDown);
        }

        static void img_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            wnd.PopView();
            wnd.CommitNavigation();
        }
    }
}
