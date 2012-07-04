namespace XIDE.Main_Window.Toolbar
{
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
    using XIDE.Main_Window;

    /// <summary>
    /// Interaction logic for xToolBar.xaml
    /// </summary>
    public partial class xToolBar : UserControl
    {
        public delegate void ToolbarButtonSelectedDelegate(xToolbarButton button);

        public event ToolbarButtonSelectedDelegate ToolbarButtonSelected;

        private List<xToolbarButton> toolbarButtons;

        public xToolBar()
        {
            this.InitializeComponent();
        }

        public List<xToolbarButton> ToolbarButtons
        {
            set
            {
                if (this.toolbarButtons != null)
                {
                    foreach (xToolbarButton button in this.toolbarButtons)
                    {
                        button.MouseDown -= this.ToolbarButton_MouseDown;
                    }
                }

                this.toolbarButtons = value;
                this.buttonContainer.Children.Clear();

                foreach (xToolbarButton button in this.toolbarButtons)
	            {
                    this.buttonContainer.Children.Add(button);
                    button.MouseDown += this.ToolbarButton_MouseDown;
	            }
            }

            get { return this.toolbarButtons; }
        }

        public void ViewButtonFromIndex(int i)
        {
            this.ToolbarButton_MouseDown(this.toolbarButtons[i], null);
        }

        private void ToolbarButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (xToolbarButton button in this.toolbarButtons)
            {
                button.GlowEffect.Opacity = 0;
            }

            (sender as xToolbarButton).GlowEffect.Opacity = 1;

            if (this.ToolbarButtonSelected != null)
            {
                this.ToolbarButtonSelected(sender as xToolbarButton);
            }

            if (NavigationWindow.CurrentNavigationWindow.NumberOfViews == 2)
            {
                NavigationWindow.CurrentNavigationWindow.PopView();
            }

            if (NavigationWindow.CurrentNavigationWindow.CurrentView != (sender as xToolbarButton).SubView)
            {
                NavigationWindow.CurrentNavigationWindow.PushView((sender as xToolbarButton).SubView, false);
                NavigationWindow.CurrentNavigationWindow.CommitNavigation();
            }
        }
    }
}
