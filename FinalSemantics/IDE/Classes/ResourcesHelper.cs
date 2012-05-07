namespace IDE.Classes
{
    using System;
    using System.Reflection;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Helps getting resources from assembly.
    /// </summary>
    public static class ResourcesHelper
    {
        /// <summary>
        /// Returns an image from the assembly.
        /// </summary>
        /// <param name="name">Name of the image.</param>
        /// <returns>A BitmapImage object.</returns>
        public static BitmapImage GetImage(string name)
        {
            return new BitmapImage(new Uri(
                string.Format(
                    @"pack://application:,,,/{0};component/Images/{1}",
                    Assembly.GetExecutingAssembly().GetName().Name,
                    name),
                UriKind.Absolute));
        }
    }
}
