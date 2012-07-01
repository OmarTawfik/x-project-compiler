namespace WPF_UVA.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using XIDE.Manager;
    using System.IO;
    using System.Windows;
    using Microsoft.Win32;
    using XIDE.Main_Window;
    using XIDE.Main_Window.Image_View;

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
        /// Updates the contents of the panel.
        /// </summary>
        /// <param name="items">List of ImageData objects.</param>
        public void UpdateItems()
        {
            List<ImageData> items = new List<ImageData>();
            foreach (string sprite in ProjectsManager.CurrentProject.SpriteFiles)
            {
                string location = ProjectsManager.CurrentProject.GetFullPath(sprite, ProjectResourceType.Sprite);
                if (File.Exists(location))
                {
                    items.Add(new ImageData(sprite, ProjectsManager.CurrentProject.LoadSpriteFile(sprite)));
                }
                else
                {
                    MessageBox.Show("Cannot Load " + sprite);
                }
            }

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
            string sprite = ProjectsManager.CurrentProject.SpriteFiles[i];
            string location = ProjectsManager.CurrentProject.GetFullPath(sprite, ProjectResourceType.Sprite);

            if (File.Exists(location))
            {
                try
                {
                    File.Delete(location);
                }
                catch { }
            }

            ProjectsManager.CurrentProject.SpriteFiles.RemoveAt(i);
            ProjectsManager.CurrentProject.SaveProject();
            this.UpdateItems();
        }

        /// <summary>
        /// Handles the event when user clickes an item.
        /// </summary>
        /// <param name="i">Index of item.</param>
        private void ItemClickHandler(int i)
        {
            if (i < 0)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".png";
                dialog.Filter = "PNG image files (.png)|*.png";

                if (dialog.ShowDialog() == true)
                {
                    string remoteFileFullPath = dialog.FileName;
                    string remoteFileName = Path.GetFileName(remoteFileFullPath);
                    string localFileFullPath = ProjectsManager.CurrentProject.GetFullPath(remoteFileName, ProjectResourceType.Sprite);
                    string localFileName = Path.GetFileName(localFileFullPath);

                    if (File.Exists(localFileFullPath))
                    {
                        MessageBox.Show("A file with the same name already exists.");
                        return;
                    }

                    File.Copy(remoteFileFullPath, localFileFullPath);

                    ProjectsManager.CurrentProject.SpriteFiles.Add(localFileName);
                    ProjectsManager.CurrentProject.SaveProject();
                    this.UpdateItems();
                }
            }
            else
            {
                NavigationWindow.CurrentNavigationWindow.PushView(
                    new ImageDisplay(ProjectsManager.CurrentProject.LoadSpriteFile(
                        ProjectsManager.CurrentProject.SpriteFiles[i])),
                    true);
            }
        }
    }
}
