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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XIDE.Main_Window.Toolbar;
using XIDE.Main_Window.CodeView;

namespace XIDE.Main_Window
{
    struct NavigationState
    {
        private FrameworkElement view;
        private bool hideToolbar;

        public NavigationState(FrameworkElement view, bool hideToolbar)
        {
            this.view = view;
            this.hideToolbar = hideToolbar;
        }

        public FrameworkElement View
        {
            get { return this.view; }
        }

        public bool HideToolbar
        {
            get { return this.hideToolbar; }
        }
    }

    /// <summary>
    /// Interaction logic for NavigationWindow.xaml
    /// </summary>
    public partial class NavigationWindow : Window
    {
        private static NavigationWindow currentNavigationWindow;

        private Stack<NavigationState> navigationStack;

        public NavigationWindow(FrameworkElement rootView, bool hideToolbar)
        {
            InitializeComponent();
            this.xWindowBarButtonsList1.TargetedWindow = this;
            this.navigationStack = new Stack<NavigationState>();
            this.navigationStack.Push(new NavigationState(rootView, hideToolbar));
            this.mainGrid.ClipToBounds = true;
            currentNavigationWindow = this;

            if (hideToolbar)
            {
                this.xToolBar1.Margin = new Thickness(0, -100, 0, 0);
            }

            this.ViewCurrentView();
            xToolbarButton codeViewButton = new xToolbarButton();
            codeViewButton.Icon = new BitmapImage(new Uri("C:/Users/Muhammad/Desktop/icon.png"));
            codeViewButton.SubView = new CodePanel();

            xToolbarButton codeViewButton2 = new xToolbarButton();
            codeViewButton2.Icon = new BitmapImage(new Uri("C:/Users/Muhammad/Desktop/icon.png"));
            codeViewButton2.SubView = new Image();
            (codeViewButton2.SubView as Image).Source = new BitmapImage(new Uri("C:/Users/Muhammad/Desktop/Texture.png"));

            List<xToolbarButton> buttonList = new List<xToolbarButton>();
            buttonList.Add(codeViewButton);
            buttonList.Add(codeViewButton2);
            this.xToolBar1.ToolbarButtons = buttonList;
        }

        public bool IsViewingRoot
        {
            get { return this.navigationStack.Count == 1; }
        }

        public int NumberOfViews
        {
            get { return this.navigationStack.Count; }
        }

        public FrameworkElement CurrentView
        {
            get { return this.navigationStack.Peek().View; }
        }

        public static NavigationWindow CurrentNavigationWindow
        {
            get { return currentNavigationWindow; }
        }

        public void PushView(FrameworkElement view, bool hideToolbar)
        {
            this.navigationStack.Push(new NavigationState(view, hideToolbar));
        }

        public FrameworkElement PopView()
        {
            if (this.navigationStack.Count == 1)
            {
                return null;
            }

            return this.navigationStack.Pop().View;
        }

        public void CommitNavigation()
        {
            this.ViewCurrentView();
        }

        private void ViewCurrentView()
        {
            ThicknessAnimation marginAnimation;
            if (this.navigationStack.Peek().HideToolbar)
            {
                marginAnimation = new ThicknessAnimation(new Thickness(0, -100, 0, 0), TimeSpan.FromSeconds(0.6));
            }
            else
            {
                marginAnimation = new ThicknessAnimation(new Thickness(0), TimeSpan.FromSeconds(0.6));
            }

            marginAnimation.AccelerationRatio = 0.4;
            marginAnimation.DecelerationRatio = 0.4;

            this.xToolBar1.BeginAnimation(FrameworkElement.MarginProperty, marginAnimation);

            IntergalacticControls.UIHelpers.FadeOutAnimation(this.mainGrid, 0, 0.3);
            IntergalacticControls.UIHelpers.CallFunctionAfterDelay(0.3, this.Dispatcher, new Action(this.FinalizeViewingCurrentView));
        }

        private void FinalizeViewingCurrentView()
        {
            this.mainGrid.Children.Clear();
            this.mainGrid.Children.Add(this.navigationStack.Peek().View);
            Grid.SetRow(this.navigationStack.Peek().View, 1);
            IntergalacticControls.UIHelpers.FadeInAnimation(this.mainGrid, 0, 0.3);
        }
    }
}
