namespace WPF_UVA.Controls
{
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Holds the name and thumbnail of a project sprite.
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// Name of the sprite.
        /// </summary>
        private string name;

        /// <summary>
        /// Thumbnail of the sprite.
        /// </summary>
        private BitmapImage thumbnail;

        /// <summary>
        /// Initializes a new instance of the ImageData class.
        /// </summary>
        /// <param name="name">Name of the sprite.</param>
        /// <param name="thumbnail">Thumbnail of the sprite.</param>
        public ImageData(string name, BitmapImage thumbnail)
        {
            this.name = name;
            this.thumbnail = thumbnail;
        }

        /// <summary>
        /// Gets the name of the sprite.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the thumbnail of the sprite.
        /// </summary>
        public BitmapImage Thumbnail
        {
            get { return this.thumbnail; }
        }
    }
}
