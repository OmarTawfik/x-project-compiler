namespace IDE.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Helps animating UI Elements.
    /// </summary>
    public static class AnimationsHelper
    {
        /// <summary>
        /// Moves Up a certain UI Element.
        /// </summary>
        /// <param name="element">Element to be moved.</param>
        /// <param name="ammount">Ammount of pixels to move.</param>
        /// <param name="milliSeconds">Animation Duration.</param>
        public static void MoveUp(UIElement element, double ammount, double milliSeconds)
        {
            element.RenderTransform = new TranslateTransform();
            DoubleAnimation anim = new DoubleAnimation(0, -ammount, TimeSpan.FromMilliseconds(milliSeconds));
            element.RenderTransform.BeginAnimation(TranslateTransform.YProperty, anim);
        }

        /// <summary>
        /// Moves Down a certain UI Element.
        /// </summary>
        /// <param name="element">Element to be moved.</param>
        /// <param name="ammount">Ammount of pixels to move.</param>
        /// <param name="milliSeconds">Animation Duration.</param>
        public static void MoveDown(UIElement element, double ammount, double milliSeconds)
        {
            element.RenderTransform = new TranslateTransform();
            DoubleAnimation anim = new DoubleAnimation(-ammount, 0, TimeSpan.FromMilliseconds(milliSeconds));
            element.RenderTransform.BeginAnimation(TranslateTransform.YProperty, anim);
        }
    }
}
