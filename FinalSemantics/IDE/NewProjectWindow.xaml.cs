namespace IDE
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Media;
    using System.Windows.Media.Effects;

    /// <summary>
    /// Interaction logic for NewProjectWindow.xaml
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the NewProjectWindow class.
        /// </summary>
        public NewProjectWindow()
        {
            this.InitializeComponent();
            this.nameTextbox.Focus();
        }

        /// <summary>
        /// Gets the new project name.
        /// </summary>
        public string ProjectName { get; private set; }

        /// <summary>
        /// Gets the new project location.
        /// </summary>
        public string ProjectLocation { get; private set; }

        /// <summary>
        /// CancelButton Click event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// NameTextBox TextChanged event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Text Changed Event Arguments</param>
        private void NameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ValidateName();
        }

        /// <summary>
        /// Validates project name entered by user.
        /// </summary>
        /// <returns>True if it was valid, false otherwise.</returns>
        private bool ValidateName()
        {
            if (string.IsNullOrEmpty(this.nameTextbox.Text)
             || this.nameTextbox.Text.Any(c => !char.IsLetterOrDigit(c))
             || !char.IsLetter(this.nameTextbox.Text[0]))
            {
                (this.nameTextbox.Effect as DropShadowEffect).Color = Colors.Red;
                return false;
            }
            else
            {
                (this.nameTextbox.Effect as DropShadowEffect).Color = Colors.White;
                return true;
            }
        }

        /// <summary>
        /// Validates project location entered by user.
        /// </summary>
        /// <returns>True if it was valid, false otherwise.</returns>
        private bool ValidateLocation()
        {
            if (string.IsNullOrEmpty(this.locationTextbox.Text))
            {
                (this.locationTextbox.Effect as DropShadowEffect).Color = Colors.Red;
                return false;
            }
            else
            {
                (this.locationTextbox.Effect as DropShadowEffect).Color = Colors.White;
                return true;
            }
        }

        /// <summary>
        /// OkButton Click event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments</param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ValidateName() & this.ValidateLocation())
            {
                this.ProjectName = this.nameTextbox.Text;
                this.ProjectLocation = this.locationTextbox.Text;

                this.DialogResult = true;
                this.Close();
            }
        }

        /// <summary>
        /// LocationTextBox GotFocus event.
        /// </summary>
        /// <param name="sender">Sender Object.</param>
        /// <param name="e">Routed Event Arguments</param>
        private void LocationTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.locationTextbox.Text = dialog.SelectedPath;
            }

            this.ValidateLocation();
        }
    }
}
