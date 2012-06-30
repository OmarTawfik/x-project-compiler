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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XIDE.Main_Window.Toolbar
{
    /// <summary>
    /// Interaction logic for xToolbarButton.xaml
    /// </summary>
    public partial class xToolbarButton : UserControl
    {
        private ImageSource icon;

        private FrameworkElement subView;

        public xToolbarButton()
        {
            InitializeComponent();
            this.glowEffect.Opacity = 0;
        }

        public ImageSource Icon
        {
            get { return this.icon; }
            set
            {
                this.icon = value;
                this.imgIcon.Source = value;
            }
        }

        public FrameworkElement SubView
        {
            get { return this.subView; }
            set { this.subView = value; }
        }

        public DropShadowEffect GlowEffect
        {
            get { return this.glowEffect; }
        }
    }
}
