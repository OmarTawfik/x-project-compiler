using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public enum WindowButtonType
    {
        Close,
        Maximize,
        Minimize
    }

    /// <summary>
    /// Interaction logic for xWindowBarButton.xaml
    /// </summary>
    public partial class xWindowBarButton : UserControl
    {
        private static ImageSource normalCloseImage, hoverCloseImage, pressCloseImage;
        private static ImageSource normalMaxImage, hoverMaxImage, pressMaxImage;
        private static ImageSource normalMinImage, hoverMinImage, pressMinImage;

        private WindowButtonType type;

        static xWindowBarButton()
        {
            normalCloseImage = new BitmapImage(new Uri(@"pack://application:,,,/XIDE;component/Main%20Window/Window%20Buttons/Button%20Images/Close_Normal.png"));
            hoverCloseImage = new BitmapImage(new Uri(@"pack://application:,,,/XIDE;component/Main Window/Window Buttons/Button Images/Close_Hover.png"));
            pressCloseImage = new BitmapImage(new Uri(@"pack://application:,,,/XIDE;component/Main Window/Window Buttons/Button Images/Close_Press.png"));

            normalMaxImage = new BitmapImage(new Uri(@"pack://application:,,,/XIDE;component/Main Window/Window Buttons/Button Images/Max_Normal.png"));
            hoverMaxImage = new BitmapImage(new Uri(@"pack://application:,,,/XIDE;component/Main Window/Window Buttons/Button Images/Max_Hover.png"));
            pressMaxImage = new BitmapImage(new Uri(@"pack://application:,,,/XIDE;component/Main Window/Window Buttons/Button Images/Max_Press.png"));

            normalMinImage = new BitmapImage(new Uri(@"pack://application:,,,/XIDE;component/Main Window/Window Buttons/Button Images/Min_Normal.png"));
            hoverMinImage = new BitmapImage(new Uri(@"pack://application:,,,/XIDE;component/Main Window/Window Buttons/Button Images/Min_Hover.png"));
            pressMinImage = new BitmapImage(new Uri(@"pack://application:,,,/XIDE;component/Main Window/Window Buttons/Button Images/Min_Press.png"));
        }

        public xWindowBarButton()
        {
            InitializeComponent();

            switch (this.type)
            {
                case WindowButtonType.Close:
                    this.imgIcon.Source = normalCloseImage;
                    break;
                case WindowButtonType.Maximize:
                    this.imgIcon.Source = normalMaxImage;
                    break;
                case WindowButtonType.Minimize:
                    this.imgIcon.Source = normalMinImage;
                    break;
                default:
                    break;
            }
        }

        public WindowButtonType ButtonType
        {
            get { return this.type; }
            set
            {
                this.type = value;
                switch (this.type)
                {
                    case WindowButtonType.Close:
                        this.imgIcon.Source = normalCloseImage;
                        break;
                    case WindowButtonType.Maximize:
                        this.imgIcon.Source = normalMaxImage;
                        break;
                    case WindowButtonType.Minimize:
                        this.imgIcon.Source = normalMinImage;
                        break;
                    default:
                        break;
                }
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            switch (this.type)
            {
                case WindowButtonType.Close:
                    this.imgIcon.Source = hoverCloseImage;
                    break;
                case WindowButtonType.Maximize:
                    this.imgIcon.Source = hoverMaxImage;
                    break;
                case WindowButtonType.Minimize:
                    this.imgIcon.Source = hoverMinImage;
                    break;
                default:
                    break;
            }
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            switch (this.type)
            {
                case WindowButtonType.Close:
                    this.imgIcon.Source = normalCloseImage;
                    break;
                case WindowButtonType.Maximize:
                    this.imgIcon.Source = normalMaxImage;
                    break;
                case WindowButtonType.Minimize:
                    this.imgIcon.Source = normalMinImage;
                    break;
                default:
                    break;
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (this.type)
            {
                case WindowButtonType.Close:
                    this.imgIcon.Source = pressCloseImage;
                    break;
                case WindowButtonType.Maximize:
                    this.imgIcon.Source = pressMaxImage;
                    break;
                case WindowButtonType.Minimize:
                    this.imgIcon.Source = pressMinImage;
                    break;
                default:
                    break;
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (this.type)
            {
                case WindowButtonType.Close:
                    this.imgIcon.Source = normalCloseImage;
                    break;
                case WindowButtonType.Maximize:
                    this.imgIcon.Source = normalMaxImage;
                    break;
                case WindowButtonType.Minimize:
                    this.imgIcon.Source = normalMinImage;
                    break;
                default:
                    break;
            }
        }
    }
}
