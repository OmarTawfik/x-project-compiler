namespace WPF_UVA.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for ImageItem.xaml
    /// </summary>
    public partial class ImageItem : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ImageItem class.
        /// </summary>
        /// <param name="imageData">Data of the contained image.</param>
        /// <param name="hovered">Image to be displayed when mouse is on ImageItem.</param>
        /// <param name="deleteIcon">The icon of the delete button.</param>
        /// <param name="tag">Index of this item in its container.</param>
        public ImageItem(ImageData imageData, BitmapImage hovered, BitmapImage deleteIcon, int tag)
        {
            this.InitializeComponent();

            this.thumbnail.Source = imageData.Thumbnail;
            this.hoveredIcon.Source = hovered;
            this.DeleteButton.Source = deleteIcon;

            this.titleBlock.Text = imageData.Name;
            this.Tag = tag;

            if (tag < 0)
            {
                this.DeleteButton.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Event fired when the image is clicked.
        /// </summary>
        public event Action<int> ImageClicked;

        /// <summary>
        /// Event fired when the delete button is clicked.
        /// </summary>
        public event Action<int> DeleteClicked;

        /// <summary>
        /// Handles the event when the mouse enters the image.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Mouse event arguments.</param>
        private void HoveredIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            this.hoveredIcon.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Handles the event when the mouse leaves the image.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Mouse event arguments.</param>
        private void HoveredIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            this.hoveredIcon.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Handles the event when the mouse is clicked on the image.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Mouse button event arguments.</param>
        private void HoveredIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ImageClicked((int)this.Tag);
        }

        /// <summary>
        /// Handles the event when the mouse is clicked on the delete button..
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Routed event arguments.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteClicked((int)this.Tag);
        }
    }
}
