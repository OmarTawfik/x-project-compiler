namespace WPF_UVA.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using XIDE.Manager;
    using System.IO;
    using System.Windows;
    using NAudio.Wave;
    using Microsoft.Win32;

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
        /// Updates the contents of the panel.
        /// </summary>
        /// <param name="items">List of ImageData objects.</param>
        public void UpdateItems()
        {
            List<SoundData> items = new List<SoundData>();
            foreach (string sound in ProjectsManager.CurrentProject.SoundFiles)
            {
                string location = ProjectsManager.CurrentProject.GetFullPath(sound, ProjectResourceType.Sound);
                if (File.Exists(location))
                {
                    items.Add(new SoundData(sound, (int)Math.Ceiling(new WaveFileReader(location).TotalTime.TotalSeconds)));
                }
                else
                {
                    MessageBox.Show("Cannot Load " + sound);
                }
            }

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
            string sound = ProjectsManager.CurrentProject.SoundFiles[i];
            string location = ProjectsManager.CurrentProject.GetFullPath(sound, ProjectResourceType.Sound);

            if (File.Exists(location))
            {
                try
                {
                    File.Delete(location);
                }
                catch { }
            }

            ProjectsManager.CurrentProject.SoundFiles.RemoveAt(i);
            ProjectsManager.CurrentProject.SaveProject();
            this.UpdateItems();
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
                ProjectsManager.CurrentProject.LoadSoundFile(
                    ProjectsManager.CurrentProject.SoundFiles[i]).Play();
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
                ProjectsManager.CurrentProject.LoadSoundFile(
                    ProjectsManager.CurrentProject.SoundFiles[i]).Stop();
            }
        }

        private void AddNewItem()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".wav";
            dialog.Filter = "WAV sound files (.wav)|*.wav";

            if (dialog.ShowDialog() == true)
            {
                string remoteFileFullPath = dialog.FileName;
                string remoteFileName = Path.GetFileName(remoteFileFullPath);
                string localFileFullPath = ProjectsManager.CurrentProject.GetFullPath(remoteFileName, ProjectResourceType.Sound);
                string localFileName = Path.GetFileName(localFileFullPath);

                if (File.Exists(localFileFullPath))
                {
                    MessageBox.Show("A file with the same name already exists.");
                    return;
                }

                File.Copy(remoteFileFullPath, localFileFullPath);

                ProjectsManager.CurrentProject.SoundFiles.Add(localFileName);
                ProjectsManager.CurrentProject.SaveProject();
                this.UpdateItems();
            }
        }
    }
}
