namespace IDE
{
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Effects;
    using System.Windows.Media.Imaging;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Highlighting;
    using IDE.DataModels;
    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region TreeView Icons
        /// <summary>
        /// Folder Icon BitmapImage object.
        /// </summary>
        private BitmapImage folderIcon = ResourcesHelper.GetImage("FolderIcon.png");

        /// <summary>
        /// Sounds Icon BitmapImage object.
        /// </summary>
        private BitmapImage soundsIcon = ResourcesHelper.GetImage("SoundsIcon.png");

        /// <summary>
        /// Code Icon BitmapImage object.
        /// </summary>
        private BitmapImage codeIcon = ResourcesHelper.GetImage("CodeIcon.png");

        /// <summary>
        /// Images Icon BitmapImage object.
        /// </summary>
        private BitmapImage imagesIcon = ResourcesHelper.GetImage("ImagesIcon.png");
        #endregion

        #region Context Menus
        /// <summary>
        /// Folder Context Menu used in the Tree View.
        /// </summary>
        private ContextMenu folderContextMenu = new ContextMenu();

        /// <summary>
        /// File Context Menu used in the Tree View.
        /// </summary>
        private ContextMenu fileContextMenu = new ContextMenu();
        #endregion

        #region Other Properties
        /// <summary>
        /// The settings file used in the project.
        /// </summary>
        private ProjectSettingsFile projectSettings;

        /// <summary>
        /// User-selected resource type (upper buttons).
        /// </summary>
        private ProjectResourceType currentResource = ProjectResourceType.Code;

        /// <summary>
        /// Currently selected item in the TreeView.
        /// </summary>
        private TreeViewItem selectedItemByRightClick;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        /// <param name="settings">Project settings to be used.</param>
        public MainWindow(ProjectSettingsFile settings)
        {
            this.InitializeComponent();

            this.projectSettings = settings;
            this.projectSettings.SaveProject();

            this.Closing += this.FinishedEditingCode;
            this.RefreshScreen();
        }
        #endregion

        #region Selection Buttons Events
        /// <summary>
        /// The MouseEnter Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Mouse Event Arguments.</param>
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            AnimationsHelper.MoveDown(sender as Button, 5, 100);
        }

        /// <summary>
        /// The MouseLeave Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Mouse Event Arguments.</param>
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            AnimationsHelper.MoveUp(sender as Button, 5, 100);
        }

        /// <summary>
        /// The SoundsButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void SoundsButton_Click(object sender, RoutedEventArgs e)
        {
            this.currentResource = ProjectResourceType.Sounds;
            this.RefreshScreen();
        }

        /// <summary>
        /// The CodeButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void CodeButton_Click(object sender, RoutedEventArgs e)
        {
            this.currentResource = ProjectResourceType.Code;
            this.RefreshScreen();
        }

        /// <summary>
        /// The ImagesButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void ImagesButton_Click(object sender, RoutedEventArgs e)
        {
            this.currentResource = ProjectResourceType.Images;
            this.RefreshScreen();
        }
        #endregion

        #region TreeView Helpers
        /// <summary>
        /// Creates context menus to be used in the TreeView.
        /// </summary>
        /// <param name="fileIcon">Current File Icon.</param>
        private void CreateContextMenus(BitmapImage fileIcon)
        {
            this.folderContextMenu.Items.Clear();
            this.fileContextMenu.Items.Clear();

            this.folderContextMenu.Items.Add(this.ConstructMenuItem("Add New Folder", this.folderIcon, this.AddFolderButtonClicked));
            this.folderContextMenu.Items.Add(this.ConstructMenuItem("Add New File", fileIcon, this.AddFileButtonClicked));
            this.folderContextMenu.Items.Add(new Separator());
            this.folderContextMenu.Items.Add(this.ConstructMenuItem("Rename Folder", ResourcesHelper.GetImage("RenameIcon.png"), this.RenameButtonClicked));
            this.folderContextMenu.Items.Add(this.ConstructMenuItem("Delete Folder", ResourcesHelper.GetImage("DeleteIcon.png"), this.DeleteButtonClicked));

            this.fileContextMenu.Items.Add(this.ConstructMenuItem("Open File", ResourcesHelper.GetImage("OpenIcon.png"), this.OpenFileButtonClicked));
            this.fileContextMenu.Items.Add(this.ConstructMenuItem("Rename File", ResourcesHelper.GetImage("RenameIcon.png"), this.RenameButtonClicked));
            this.fileContextMenu.Items.Add(this.ConstructMenuItem("Delete File", ResourcesHelper.GetImage("DeleteIcon.png"), this.DeleteButtonClicked));
        }

        /// <summary>
        /// Refreshes the TreeView and adds proper children.
        /// </summary>
        private void RefreshScreen()
        {
            this.FinishedEditingCode(null, null);

            BitmapImage fileIcon = null;
            ProjectFolder currentFolder = null;

            if (this.currentResource == ProjectResourceType.Code)
            {
                fileIcon = this.codeIcon;
                currentFolder = this.projectSettings.CodeFolder;
            }
            else if (this.currentResource == ProjectResourceType.Images)
            {
                fileIcon = this.imagesIcon;
                currentFolder = this.projectSettings.ImagesFolder;
            }
            else if (this.currentResource == ProjectResourceType.Sounds)
            {
                fileIcon = this.soundsIcon;
                currentFolder = this.projectSettings.SoundsFolder;
            }

            this.CreateContextMenus(fileIcon);
            this.filesTreeView.Items.Clear();
            this.filesTreeView.Items.Add(this.ConstructFolder(currentFolder, fileIcon));
        }

        /// <summary>
        /// Makes a MenuItem out of data provided.
        /// </summary>
        /// <param name="name">Name of item.</param>
        /// <param name="icon">Icon of item.</param>
        /// <param name="clickHandler">ClickHandler to call when user clickes the item.</param>
        /// <returns>A MenuItem object.</returns>
        private MenuItem ConstructMenuItem(string name, BitmapImage icon, RoutedEventHandler clickHandler)
        {
            Image image = new Image();
            image.Source = icon;
            image.Height = image.Width = 16;

            MenuItem item = new MenuItem();
            item.Header = name;
            item.Icon = image;
            item.Click += clickHandler;

            return item;
        }

        /// <summary>
        /// Constructs a folder for the TreeView.
        /// </summary>
        /// <param name="folder">Folder to use.</param>
        /// <param name="icon">Icon of files inside this folder.</param>
        /// <returns>A TreeViewItem object.</returns>
        private TreeViewItem ConstructFolder(ProjectFolder folder, BitmapImage icon)
        {
            TreeViewItem root = this.ConstructTreeViewItem(folder.Name, folder, this.folderIcon);
            root.ContextMenu = this.folderContextMenu;

            folder.SubFiles = folder.SubFiles.OrderBy(x => x.Name).ToList();
            folder.SubFolders = folder.SubFolders.OrderBy(f => f.Name).ToList();

            foreach (ProjectFolder subFolder in folder.SubFolders)
            {
                root.Items.Add(this.ConstructFolder(subFolder, icon));
            }

            foreach (ProjectFile subFile in folder.SubFiles)
            {
                root.Items.Add(this.ConstructTreeViewItem(subFile.Name, subFile, icon));
            }

            return root;
        }

        /// <summary>
        /// Constructs a TreeViewItem for the TreeView.
        /// </summary>
        /// <param name="name">Name of this item.</param>
        /// <param name="tag">Object to tag to this item.</param>
        /// <param name="icon">Icon of this item.</param>
        /// <returns>A TreeViewItem object.</returns>
        private TreeViewItem ConstructTreeViewItem(string name, object tag, BitmapImage icon)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = name;
            Canvas.SetLeft(textBlock, 22);

            Image image = new Image();
            image.Source = icon;
            image.Height = image.Width = 16;

            Canvas canvas = new Canvas();
            canvas.Height = 20;
            canvas.Width = 100;

            canvas.Children.Add(image);
            canvas.Children.Add(textBlock);

            TreeViewItem item = new TreeViewItem();
            item.Header = canvas;
            item.IsExpanded = true;
            item.ContextMenu = this.fileContextMenu;
            item.Tag = tag;

            item.PreviewMouseRightButtonDown += this.TreeViewItemRightClick;
            return item;
        }
        #endregion

        #region ContextMenu Events
        /// <summary>
        /// The TreeViewItemR Right-Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void TreeViewItemRightClick(object sender, RoutedEventArgs e)
        {
            this.selectedItemByRightClick = sender as TreeViewItem;
        }

        /// <summary>
        /// The RenameContextButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void RenameButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.selectedItemByRightClick.Tag is ProjectFolder
                && this.projectSettings.GetParentFolder(this.selectedItemByRightClick.Tag as ProjectFolder) == null)
            {
                MessageBox.Show("Cannot Rename Root Folder.", "Renaming Error");
            }
            else
            {
                TextBox textBox = new TextBox();

                textBox.Height = 20;
                textBox.Width = 100;
                textBox.LostKeyboardFocus += this.DoneRenamingHandler;
                textBox.Tag = this.selectedItemByRightClick;

                this.selectedItemByRightClick.Header = textBox;
                textBox.Focus();
            }
        }

        /// <summary>
        /// The RenameContextButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void DoneRenamingHandler(object sender, RoutedEventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (string.IsNullOrEmpty(textbox.Text)
                || textbox.Text.Any(c => !char.IsLetterOrDigit(c))
                || !char.IsLetter(textbox.Text[0]))
            {
                MessageBox.Show("Invalid Name. Please enter letters and digits only (starting with a digit).", "Renaming Error");
                return;
            }

            TreeViewItem treeViewItem = textbox.Tag as TreeViewItem;
            if (treeViewItem.Tag is ProjectFolder)
            {
                ProjectFolder folder = treeViewItem.Tag as ProjectFolder;
                string oldPath = folder.Location + "\\" + folder.Name;
                string newPath = folder.Location + "\\" + textbox.Text;

                if (Directory.Exists(newPath))
                {
                    MessageBox.Show("A folder already exists with the same name.", "Renaming Error");
                    return;
                }
                else
                {
                    Directory.Move(oldPath, newPath);
                    folder.Name = textbox.Text;
                }
            }
            else if (treeViewItem.Tag is ProjectFile)
            {
                ProjectFile file = treeViewItem.Tag as ProjectFile;
                ProjectFolder parent = this.projectSettings.GetParentFile(file);
                string oldPath = parent.Location + "\\" + parent.Name + "\\" + file.Name;
                string newPath = parent.Location + "\\" + parent.Name + "\\" + textbox.Text + Path.GetExtension(oldPath);
                if (File.Exists(newPath))
                {
                    MessageBox.Show("A file already exists with the same name.", "Renaming Error");
                    return;
                }
                else
                {
                    File.Move(oldPath, newPath);
                    file.Name = textbox.Text + Path.GetExtension(oldPath);
                }
            }

            this.projectSettings.SaveProject();
            this.RefreshScreen();
        }

        /// <summary>
        /// The DeleteContextButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            TreeViewItem treeViewItem = this.selectedItemByRightClick as TreeViewItem;
            if (treeViewItem.Tag is ProjectFolder)
            {
                ProjectFolder folder = treeViewItem.Tag as ProjectFolder;
                if (this.projectSettings.GetParentFolder(folder) == null)
                {
                    MessageBox.Show("Cannot delete Root Folder.", "Deletion Error");
                    return;
                }
                else
                {
                    this.EmptyFolder(new DirectoryInfo(folder.Location + "\\" + folder.Name));
                    Directory.Delete(folder.Location + "\\" + folder.Name);
                    this.projectSettings.GetParentFolder(folder).SubFolders.Remove(folder);
                }
            }
            else if (treeViewItem.Tag is ProjectFile)
            {
                ProjectFile file = treeViewItem.Tag as ProjectFile;
                ProjectFolder folder = this.projectSettings.GetParentFile(file);
                File.Delete(folder.Location + "\\" + folder.Name + "\\" + file.Name);
                folder.SubFiles.Remove(file);
            }

            this.projectSettings.SaveProject();
            this.RefreshScreen();
        }

        /// <summary>
        /// Empties a given folder from subfolders/subfiles.
        /// </summary>
        /// <param name="directoryInfo">Directory Info to use.</param>
        private void EmptyFolder(DirectoryInfo directoryInfo)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                this.EmptyFolder(subfolder);
            }
        }

        /// <summary>
        /// The AddFolderContextButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void AddFolderButtonClicked(object sender, RoutedEventArgs e)
        {
            ProjectFolder parent = this.selectedItemByRightClick.Tag as ProjectFolder;
            string location = parent.Location + "\\" + parent.Name;

            for (int i = 1; i < int.MaxValue; i++)
            {
                if (Directory.Exists(location + "\\NewFolder" + i.ToString()) == false)
                {
                    parent.SubFolders.Add(new ProjectFolder("NewFolder" + i.ToString(), location));
                    break;
                }
            }

            this.projectSettings.SaveProject();
            this.RefreshScreen();
        }

        /// <summary>
        /// The AddFileContextButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void AddFileButtonClicked(object sender, RoutedEventArgs e)
        {
            ProjectFolder parent = this.selectedItemByRightClick.Tag as ProjectFolder;
            string location = parent.Location + "\\" + parent.Name;

            if (this.currentResource == ProjectResourceType.Code)
            {
                for (int i = 1; i < int.MaxValue; i++)
                {
                    if (File.Exists(location + "\\NewCodeFile" + i.ToString() + ".x") == false)
                    {
                        parent.SubFiles.Add(new ProjectFile(ProjectResourceType.Code, "NewCodeFile" + i.ToString() + ".x"));
                        File.CreateText(location + "\\NewCodeFile" + i.ToString() + ".x");
                        break;
                    }
                }
            }
            else
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (this.currentResource == ProjectResourceType.Sounds)
                {
                    dialog.DefaultExt = ".wav";
                    dialog.Filter = "WAV sound files (.wav)|*.wav";
                }
                else if (this.currentResource == ProjectResourceType.Images)
                {
                    dialog.DefaultExt = ".png";
                    dialog.Filter = "PNG image files (.png)|*.png";
                }

                if (dialog.ShowDialog() == true)
                {
                    string fileName = Path.GetFileName(dialog.FileName);
                    if (File.Exists(location + "\\" + fileName))
                    {
                        MessageBox.Show("A file with the same name already exists.", "Error Adding File");
                        return;
                    }
                    else
                    {
                        File.Copy(dialog.FileName, location + "\\" + fileName);
                        parent.SubFiles.Add(new ProjectFile(this.currentResource, fileName));
                    }
                }
            }

            this.projectSettings.SaveProject();
            this.RefreshScreen();
        }
        
        /// <summary>
        /// The OpenFileContextButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void OpenFileButtonClicked(object sender, RoutedEventArgs e)
        {
            this.FinishedEditingCode(null, null);

            ProjectFile file = this.selectedItemByRightClick.Tag as ProjectFile;
            ProjectFolder folder = this.projectSettings.GetParentFile(file);
            string filePath = folder.Location + "\\" + folder.Name + "\\" + file.Name;

            if (File.Exists(filePath) == false)
            {
                MessageBox.Show("File not found on hard disk.", "Error Openning File");
                return;
            }

            if (this.currentResource == ProjectResourceType.Images)
            {
                MemoryStream memoryStream = new MemoryStream();
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                memoryStream.SetLength(fileStream.Length);
                fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);

                memoryStream.Flush();
                fileStream.Close();

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = memoryStream;
                bitmap.EndInit();

                Image image = new Image();
                image.Source = bitmap;
                image.Height = bitmap.Height;
                image.Width = bitmap.Width;

                this.mainEditingArea.Content = image;
            }
            else if (this.currentResource == ProjectResourceType.Code)
            {
                TextEditor textEditor = new TextEditor();

                textEditor.FontFamily = new FontFamily("Consolas");
                textEditor.HorizontalAlignment = HorizontalAlignment.Stretch;
                textEditor.VerticalAlignment = VerticalAlignment.Stretch;
                textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
                textEditor.FontSize = 14;
                textEditor.Margin = new Thickness(6, 6, 6, 6);
                textEditor.ShowLineNumbers = true;

                DropShadowEffect shadow = new DropShadowEffect();
                shadow.Color = Colors.Indigo;
                shadow.BlurRadius = 8;
                shadow.ShadowDepth = 0;
                textEditor.Effect = shadow;

                textEditor.Load(filePath);
                textEditor.Tag = filePath;
                this.mainEditingArea.Content = textEditor;
            }
        }

        /// <summary>
        /// The Finished Editing code Event Handler.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void FinishedEditingCode(object sender, CancelEventArgs e)
        {
            if (this.mainEditingArea.Content is TextEditor)
            {
                TextEditor textEditor = this.mainEditingArea.Content as TextEditor;
                textEditor.Save(textEditor.Tag as string);
            }
        }
        #endregion
    }
}
