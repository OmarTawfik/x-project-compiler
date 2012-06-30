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

namespace XIDE.Main_Window.CodeView
{
    /// <summary>
    /// Interaction logic for PlusButton.xaml
    /// </summary>
    public partial class PlusButton : UserControl
    {
        public PlusButton()
        {
            InitializeComponent();
            this.pressRectangle.Opacity = 0;
        }

        public ImageSource Icon
        {
            get { return this.imgIcon.Source; }
            set
            {
                this.imgIcon.Source = value;
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IntergalacticControls.UIHelpers.FadeInAnimation(this.pressRectangle, 0, 0.1);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IntergalacticControls.UIHelpers.FadeOutAnimation(this.pressRectangle, 0, 0.3);
        }
    }
}
