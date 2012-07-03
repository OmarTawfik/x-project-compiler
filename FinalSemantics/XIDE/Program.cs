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
            wnd = new NavigationWindow(new XIDE.Main_Window.WelcomeScreen.WelcomeScreen(), true);
            wnd.ShowDialog();
        }
    }
}

