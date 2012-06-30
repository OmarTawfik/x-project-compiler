namespace WPF_UVA.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for ImagePanel.xaml
    /// </summary>
    public partial class ImagePanel : UserControl
    {
        /// <summary>
        /// The image that appears when mouse is on a certain item.
        /// </summary>
        private BitmapImage hoverIcon;

        /// <summary>
        /// The image that appears on the add button.
        /// </summary>
        private BitmapImage addIcon;

        /// <summary>
        /// The image that appears on the delete icon of every item.
        /// </summary>
        private BitmapImage deleteIcon;

        /// <summary>
        /// Initializes a new instance of the ImagePanel class.
        /// </summary>
        /// <param name="hoverIcon">The image that appears when mouse is on a certain item.</param>
        /// <param name="addIcon">The image that appears on the add button.</param>
        /// <param name="deleteIcon">The image that appears on the delete icon of every item.</param>
        public ImagePanel(BitmapImage hoverIcon, BitmapImage addIcon, BitmapImage deleteIcon)
        {
            this.InitializeComponent();
            
            this.hoverIcon = hoverIcon;
            this.addIcon = addIcon;
            this.deleteIcon = deleteIcon;
        }

        /// <summary>
        /// Event fired when user deletes an item.
        /// </summary>
        public event Action<int> DeleteItem;

        /// <summary>
        /// Event fired when user adds an item.
        /// </summary>
        public event Action AddNewItem;

        /// <summary>
        /// Event fired when user opens an item.
        /// </summary>
        public event Action<int> ItemClicked;

        /// <summary>
        /// Updates the contents of the panel.
        /// </summary>
        /// <param name="items">List of ImageData objects.</param>
        public void UpdateItems(List<ImageData> items)
        {
            this.mainWrapPanel.Children.Clear();

            for (int i = 0; i < items.Count; i++)
            {
                ImageItem newItem = new ImageItem(items[i], this.hoverIcon, this.deleteIcon, i);
                newItem.ImageClicked += this.ItemClickHandler;
                newItem.DeleteClicked += this.DeleteItemHandler;
                this.mainWrapPanel.Children.Add(newItem);
            }

            ImageItem addButton = new ImageItem(
                new ImageData("Add New Image", this.addIcon), this.addIcon, this.deleteIcon, -1);
            addButton.ImageClicked += this.ItemClickHandler;
            addButton.DeleteClicked += this.DeleteItemHandler;
            this.mainWrapPanel.Children.Add(addButton);
        }

        /// <summary>
        /// Handles the event when user deletes an item.
        /// </summary>
        /// <param name="i">Index of item.</param>
        private void DeleteItemHandler(int i)
        {
            this.DeleteItem(i);
        }

        /// <summary>
        /// Handles the event when user clickes an item.
        /// </summary>
        /// <param name="i">Index of item.</param>
        private void ItemClickHandler(int i)
        {
            if (i < 0)
            {
                this.AddNewItem();
            }
            else
            {
                this.ItemClicked(i);
            }
        }
    }
}
