namespace FinalSemantics
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The main entry class for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new TranslatorForm().Show();
            Application.Run(new MainForm());
        }
    }
}