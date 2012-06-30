namespace WPF_UVA.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for SoundPanel.xaml
    /// </summary>
    public partial class SoundPanel : UserControl
    {
         /// <summary>
        /// The default image that appears on any item.
        /// </summary>
        private BitmapImage thumbnail;

        /// <summary>
        /// The image that appears when mouse is on a certain item.
        /// </summary>
        private BitmapImage hoveredIcon;

        /// <summary>
        /// The image that appears on the add button.
        /// </summary>
        private BitmapImage addIcon;

        /// <summary>
        /// The image that appears on the delete icon of every item.
        /// </summary>
        private BitmapImage deleteIcon;

        /// <summary>
        /// Initializes a new instance of the SoundPanel class.
        /// </summary>
        /// <param name="thumbnail">The image that appears when mouse is on a certain item.</param>
        /// <param name="addIcon">The image that appears on the add button.</param>
        /// <param name="deleteIcon">The image that appears on the delete icon of every item.</param>
        public SoundPanel(BitmapImage thumbnail, BitmapImage hoveredIcon, BitmapImage addIcon, BitmapImage deleteIcon)
        {
            this.InitializeComponent();
            
            this.thumbnail = thumbnail;
            this.hoveredIcon = hoveredIcon;
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
        /// Event fired when user starts a sound.
        /// </summary>
        public event Action<int> SoundStarted;

        /// <summary>
        /// Event fired when user ends a sound.
        /// </summary>
        public event Action<int> SoundEnded;

        /// <summary>
        /// Updates the contents of the panel.
        /// </summary>
        /// <param name="items">List of ImageData objects.</param>
        public void UpdateItems(List<SoundData> items)
        {
            this.mainWrapPanel.Children.Clear();

            for (int i = 0; i < items.Count; i++)
            {
                SoundItem newItem = new SoundItem(items[i], this.thumbnail, this.hoveredIcon, this.deleteIcon, i);
                newItem.SoundStart += this.SoundStartHandler;
                newItem.SoundEnd += this.SoundEndHandler;
                newItem.DeleteClicked += this.DeleteItemHandler;
                this.mainWrapPanel.Children.Add(newItem);
            }

            SoundItem addButton = new SoundItem(
                new SoundData("Add New Sound", 0), this.addIcon, this.addIcon, this.deleteIcon, -1);
            addButton.SoundStart += this.SoundStartHandler;
            addButton.SoundEnd += this.SoundEndHandler;
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
        /// Handles the event when user starts a sound.
        /// </summary>
        /// <param name="i">Index of item.</param>
        private void SoundStartHandler(int i)
        {
            if (i < 0)
            {
                this.AddNewItem();
            }
            else
            {
                this.SoundStarted(i);
            }
        }

        /// <summary>
        /// Handles the event when user ends a sound.
        /// </summary>
        /// <param name="i">Index of item.</param>
        private void SoundEndHandler(int i)
        {
            if (i < 0)
            {
                this.AddNewItem();
            }
            else
            {
                this.SoundEnded(i);
            }
        }
    }
}
