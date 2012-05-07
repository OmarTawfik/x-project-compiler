namespace IDE
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using IDE.Classes;

    /// <summary>
    /// Project Resource type.
    /// </summary>
    public enum ProjectResourceType
    {
        /// <summary>
        /// Sound resource.
        /// </summary>
        Sounds,

        /// <summary>
        /// Image resource.
        /// </summary>
        Images,

        /// <summary>
        /// Code resource.
        /// </summary>
        Code,
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The settings file used in the project.
        /// </summary>
        private ProjectSettingsFile projectSettings;

        /// <summary>
        /// User-selected resource type (upper buttons).
        /// </summary>
        private ProjectResourceType currentResource = ProjectResourceType.Code;

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

        /// <summary>
        /// Folder Context Menu used in the Tree View.
        /// </summary>
        private ContextMenu folderContextMenu = new ContextMenu();

        /// <summary>
        /// File Context Menu used in the Tree View.
        /// </summary>
        private ContextMenu fileContextMenu = new ContextMenu();

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
        /// Constructs a folder for the TreeView.
        /// </summary>
        /// <param name="folder">Folder to use.</param>
        /// <param name="icon">Icon of files inside this folder.</param>
        /// <returns>A TreeViewItem object.</returns>
        private TreeViewItem ConstructFolder(ProjectFolder folder, BitmapImage icon)
        {
            TreeViewItem root = this.ConstructTreeViewItem(folder.Name, this.folderIcon);
            root.ContextMenu = this.folderContextMenu;
            root.Tag = folder;

            foreach (ProjectFolder subFolder in folder.SubFolders)
            {
                root.Items.Add(this.ConstructFolder(subFolder, icon));
            }

            foreach (string subFile in folder.SubFiles)
            {
                root.Items.Add(this.ConstructTreeViewItem(subFile, icon));
            }

            return root;
        }

        /// <summary>
        /// Constructs a TreeViewItem for the TreeView.
        /// </summary>
        /// <param name="name">Name of this item.</param>
        /// <param name="icon">Icon of this item.</param>
        /// <returns>A TreeViewItem object.</returns>
        private TreeViewItem ConstructTreeViewItem(string name, BitmapImage icon)
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
            item.Tag = name;
            return item;
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

        /// <summary>
        /// Creates context menus to be used in the TreeView.
        /// </summary>
        /// <param name="fileIcon">Current File Icon.</param>
        private void CreateContextMenus(BitmapImage fileIcon)
        {
            this.folderContextMenu.Items.Clear();
            this.fileContextMenu.Items.Clear();

            this.folderContextMenu.Items.Add(this.ConstructItem("Add New Folder", this.folderIcon, this.AddFolderButtonClicked));
            this.folderContextMenu.Items.Add(this.ConstructItem("Add New File", fileIcon, this.AddFileButtonClicked));
            this.folderContextMenu.Items.Add(new Separator());
            this.folderContextMenu.Items.Add(this.ConstructItem("Rename Folder", ResourcesHelper.GetImage("RenameIcon.png"), this.RenameButtonClicked));
            this.folderContextMenu.Items.Add(this.ConstructItem("Delete Folder", ResourcesHelper.GetImage("DeleteIcon.png"), this.DeleteButtonClicked));

            this.fileContextMenu.Items.Add(this.ConstructItem("Open File", ResourcesHelper.GetImage("OpenIcon.png"), this.OpenFileButtonClicked));
            this.fileContextMenu.Items.Add(this.ConstructItem("Rename File", ResourcesHelper.GetImage("RenameIcon.png"), this.RenameButtonClicked));
            this.fileContextMenu.Items.Add(this.ConstructItem("Delete File", ResourcesHelper.GetImage("DeleteIcon.png"), this.DeleteButtonClicked));
        }

        /// <summary>
        /// Makes a MenuItem out of data provided.
        /// </summary>
        /// <param name="name">Name of item.</param>
        /// <param name="icon">Icon of item.</param>
        /// <param name="clickHandler">ClickHandler to call when user clickes the item.</param>
        /// <returns>A MenuItem object.</returns>
        private MenuItem ConstructItem(string name, BitmapImage icon, RoutedEventHandler clickHandler)
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
        /// The RenameContextButton Click Event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments.</param>
        private void RenameButtonClicked(object sender, RoutedEventArgs e)
        {
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
    }
}
