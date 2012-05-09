namespace IDE
{
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using IDE.DataModels;

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
            this.RefreshTreeView();
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
            this.RefreshTreeView();
        }

        /// <summary>
        /// The CodeButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void CodeButton_Click(object sender, RoutedEventArgs e)
        {
            this.currentResource = ProjectResourceType.Code;
            this.RefreshTreeView();
        }

        /// <summary>
        /// The ImagesButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void ImagesButton_Click(object sender, RoutedEventArgs e)
        {
            this.currentResource = ProjectResourceType.Images;
            this.RefreshTreeView();
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
        private void RefreshTreeView()
        {
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

            folder.SubFiles.Sort();
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
            if (this.selectedItemByRightClick.Tag is ProjectFolder && (this.selectedItemByRightClick.Tag as ProjectFolder).IsRootFolder)
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
                string oldPath = file.ContainingFolder.Location + "\\" + file.ContainingFolder.Name + "\\" + file.Name;
                string newPath = file.ContainingFolder.Location + "\\" + file.ContainingFolder.Name + "\\" + textbox.Text + Path.GetExtension(oldPath);
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
            this.RefreshTreeView();
        }

        /// <summary>
        /// The DeleteContextButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
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
                    parent.SubFolders.Add(new ProjectFolder("NewFolder" + i.ToString(), location, false));
                    break;
                }
            }

            this.projectSettings.SaveProject();
            this.RefreshTreeView();
        }

        /// <summary>
        /// The AddFileContextButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void AddFileButtonClicked(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// The OpenFileContextButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void OpenFileButtonClicked(object sender, RoutedEventArgs e)
        {
        }
        #endregion
    }
}
