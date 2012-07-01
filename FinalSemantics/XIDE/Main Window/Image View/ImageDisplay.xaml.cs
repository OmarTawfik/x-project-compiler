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

namespace XIDE.Main_Window.Image_View
{
    /// <summary>
    /// Interaction logic for ImageDisplay.xaml
    /// </summary>
    public partial class ImageDisplay : UserControl
    {
        public ImageDisplay(BitmapImage image)
        {
            InitializeComponent();
            this.mainImage.Source = image;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationWindow.CurrentNavigationWindow.PopView();
        }
    }
}
