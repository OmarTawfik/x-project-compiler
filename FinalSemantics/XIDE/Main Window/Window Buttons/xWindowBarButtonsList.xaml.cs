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

namespace XIDE.Main_Window.Window_Buttons
{
    /// <summary>
    /// Interaction logic for xWindowBarButtonsList.xaml
    /// </summary>
    public partial class xWindowBarButtonsList : UserControl
    {
        private Window targetedWindow;

        public xWindowBarButtonsList()
        {
            InitializeComponent();
        }

        public Window TargetedWindow
        {
            get { return this.targetedWindow; }
            set { this.targetedWindow = value; }
        }

        private void closeButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            targetedWindow.Close();
        }

        private void maxButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.targetedWindow.WindowState == WindowState.Maximized)
            {
                this.targetedWindow.WindowState = WindowState.Normal;
            }
            else
            {
                this.targetedWindow.WindowState = WindowState.Maximized;
            }
        }

        private void minButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.targetedWindow.WindowState = WindowState.Minimized;
        }
    }
}
