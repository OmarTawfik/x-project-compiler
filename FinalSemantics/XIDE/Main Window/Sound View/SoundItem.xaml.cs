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

namespace WPF_UVA.Controls
{
    /// <summary>
    /// Interaction logic for SoundItem.xaml
    /// </summary>
    public partial class SoundItem : UserControl
    {
        /// <summary>
        /// Length of the sound file in seconds.
        /// </summary>
        private int seconds;

        /// <summary>
        /// Indicates if the sound is currently playing.
        /// </summary>
        private bool isPlaying;

        /// <summary>
        /// Initializes a new instance of the SoundItem class.
        /// </summary>
        /// <param name="soundData">Data of the sound file.</param>
        /// <param name="thumbnail">Thumbnail of audio files.</param>
        /// <param name="deleteIcon">Icon to be used on the delete button.</param>
        /// <param name="tag">Index of this item in its container.</param>
        public SoundItem(SoundData soundData, BitmapImage thumbnail, BitmapImage hoveredIcon, BitmapImage deleteIcon, int tag)
        {
            this.InitializeComponent();

            this.thumbnail.Source = thumbnail;
            this.hoveredIcon.Source = hoveredIcon;
            this.DeleteButton.Source = deleteIcon;

            this.seconds = soundData.Seconds;
            this.titleBlock.Text = soundData.Name;
            
            this.Tag = tag;
            this.isPlaying = false;

            if (tag < 0)
            {
                this.DeleteButton.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Event fired when the sound is started.
        /// </summary>
        public event Action<int> SoundStart;

        /// <summary>
        /// Event fired when the sound is ended.
        /// </summary>
        public event Action<int> SoundEnd;

        /// <summary>
        /// Event fired when the delete button is clicked.
        /// </summary>
        public event Action<int> DeleteClicked;

        /// <summary>
        /// Handles the event when the mouse enters the sound.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Mouse event arguments.</param>
        private void HoveredIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            this.hoveredIcon.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the event when the mouse leaves the sound.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Mouse event arguments.</param>
        private void HoveredIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            this.hoveredIcon.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Handles the event when the mouse is clicked on the sound.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Mouse button event arguments.</param>
        private void HoveredIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.isPlaying)
            {
                this.SoundEnd((int)this.Tag);
                this.animationCanvas.Visibility = Visibility.Hidden;

                if((int)this.Tag >= 0)
                {
                    // stop animation.
                }
            }
            else
            {
                this.SoundStart((int)this.Tag);
                this.animationCanvas.Visibility = Visibility.Visible;

                if ((int)this.Tag >= 0)
                {
                    // start animation.
                }
            }

            this.isPlaying = !this.isPlaying;
        }

        /// <summary>
        /// Handles the event when the mouse is clicked on the delete button.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Routed event arguments.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteClicked((int)this.Tag);
        }
    }
}
