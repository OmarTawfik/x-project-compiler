namespace IntergalacticControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    /// <summary>
    /// Used to pass parameters to a thread start, to remove elements from a panel.
    /// </summary>
    public struct DelayActionParameters
    {
        /// <summary>
        /// The element to remove
        /// </summary>
        private FrameworkElement element;

        /// <summary>
        /// The container of the element
        /// </summary>
        private Panel panel;

        /// <summary>
        /// Delay before element removal
        /// </summary>
        private double delayInSeconds;

        /// <summary>
        /// The dispatcher to invoke actions to
        /// </summary>
        private Dispatcher dispatcher;

        /// <summary>
        /// The action to perform after delay
        /// </summary>
        private Delegate function;

        /// <summary>
        /// List of action parameter
        /// </summary>
        private object[] parameters;

        /// <summary>
        /// Initializes a new instance of the DelayActionParameters struct
        /// </summary>
        /// <param name="element">The element to remove</param>
        /// <param name="panel">The parent panel</param>
        /// <param name="delayInSeconds">Time before removal</param>
        public DelayActionParameters(FrameworkElement element, Panel panel, double delayInSeconds)
        {
            this.element = element;
            this.panel = panel;
            this.delayInSeconds = delayInSeconds;
            this.dispatcher = null;
            this.function = null;
            this.parameters = null;
        }

        /// <summary>
        /// Initializes a new instance of the DelayActionParameters struct
        /// </summary>
        /// <param name="element">The element to remove</param>
        /// <param name="delayInSeconds">Time before removal</param>
        public DelayActionParameters(FrameworkElement element, double delayInSeconds)
        {
            this.element = element;
            this.delayInSeconds = delayInSeconds;
            this.panel = null;
            this.dispatcher = null;
            this.function = null;
            this.parameters = null;
        }

        /// <summary>
        /// Initializes a new instance of the DelayActionParameters struct
        /// </summary>
        /// <param name="dispatcher">The dispatcher to invoke to</param>
        /// <param name="function">The action to perform</param>
        /// <param name="delayInSeconds">Time before the action call</param>
        /// <param name="parameters">Parameters list</param>
        public DelayActionParameters(Dispatcher dispatcher, Delegate function, double delayInSeconds, object[] parameters)
        {
            this.delayInSeconds = delayInSeconds;
            this.dispatcher = dispatcher;
            this.function = function;
            this.parameters = parameters;
            this.panel = null;
            this.element = null;
        }

        /// <summary>
        /// Gets the element to remove
        /// </summary>
        public FrameworkElement Element
        {
            get { return this.element; }
        }

        /// <summary>
        /// Gets the parent of the element
        /// </summary>
        public Panel Panel
        {
            get { return this.panel; }
        }

        /// <summary>
        /// Gets the delay, in seconds
        /// </summary>
        public double DelayInSeconds
        {
            get { return this.delayInSeconds; }
        }

        /// <summary>
        /// Gets the dispatcher to use for invoking actions
        /// </summary>
        public Dispatcher Dispatcher
        {
            get { return this.dispatcher; }
        }

        /// <summary>
        /// Gets the dispatcher to use for invoking actions
        /// </summary>
        public Delegate Function
        {
            get { return this.function; }
        }

        /// <summary>
        /// Gets the list of parameters to give to the action
        /// </summary>
        public object[] Parameters
        {
            get { return this.parameters; }
        }
    }

    /// <summary>
    /// Provides helper funcitons relative to the UI framwwork
    /// </summary>
    public class UIHelpers
    {
        /// <summary>
        /// List of current threads (to prevent the GC from removing them)
        /// </summary>
        private static List<Thread> threads;

        /// <summary>
        /// Initializes static members of the UIHelpers class
        /// </summary>
        static UIHelpers()
        {
            threads = new List<Thread>();
        }

        /// <summary>
        /// Delegate for element removal function
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="panel">The Panel</param>
        private delegate void RemoveElementDelegate(FrameworkElement element, Panel panel);

        /// <summary>
        /// Delegate for element hiding function
        /// </summary>
        /// <param name="element">The element</param>
        private delegate void HideElementDelegate(FrameworkElement element);

        /// <summary>
        /// Gets the parent window of a control
        /// </summary>
        /// <param name="child">The control</param>
        /// <returns>The parent window</returns>
        public static Window GetParentWindow(DependencyObject child)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
            {
                return null;
            }

            Window parent = parentObject as Window;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return GetParentWindow(parentObject);
            }
        }

        /// <summary>
        /// Applies fade in animation to the given element
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="from">Optional from value</param>
        /// <param name="timeInSeconds">Time of the animation</param>
        public static void FadeInAnimation(FrameworkElement element, double? from, double timeInSeconds)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromSeconds(timeInSeconds);
            animation.AccelerationRatio = 0.3;
            animation.DecelerationRatio = 0.3;
            animation.To = 1;
            animation.From = (from != null) ? from : element.Opacity;

            element.BeginAnimation(FrameworkElement.OpacityProperty, animation);
        }

        /// <summary>
        /// Applies fade out animation to the given element
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="to">Optional to value</param>
        /// <param name="timeInSeconds">Time of the animation</param>
        public static void FadeOutAnimation(FrameworkElement element, double? to, double timeInSeconds)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromSeconds(timeInSeconds);
            animation.AccelerationRatio = 0.3;
            animation.DecelerationRatio = 0.3;
            animation.From = element.Opacity;
            animation.To = (to != null) ? to : 0;

            element.BeginAnimation(FrameworkElement.OpacityProperty, animation);
        }

        /// <summary>
        /// Applies slide in animation to the given element
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="to">The target margin</param>
        /// <param name="timeInSeconds">Time of the animation</param>
        public static void SlideInFronBottomAnimation(FrameworkElement element, Thickness to, double timeInSeconds)
        {
            ThicknessAnimation animation = new ThicknessAnimation();
            animation.Duration = TimeSpan.FromSeconds(timeInSeconds);
            animation.DecelerationRatio = 0.6;
            animation.To = to;
            animation.From = new Thickness(element.Margin.Left, element.Margin.Top + 1000, element.Margin.Right, element.Margin.Bottom);

            element.BeginAnimation(FrameworkElement.MarginProperty, animation);
        }

        /// <summary>
        /// Applies slide out animation to the given element
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="timeInSeconds">Time of the animation</param>
        public static void SlideOutToBottomAnimation(FrameworkElement element, double timeInSeconds)
        {
            ThicknessAnimation animation = new ThicknessAnimation();
            animation.Duration = TimeSpan.FromSeconds(timeInSeconds);
            animation.AccelerationRatio = 0.6;
            animation.From = element.Margin;
            animation.To = new Thickness(element.Margin.Left, element.Margin.Top + 1000, element.Margin.Right, element.Margin.Bottom);

            element.BeginAnimation(FrameworkElement.MarginProperty, animation);
        }

        /// <summary>
        /// Applies zoom in animation to the given element
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="timeInSeconds">Time of the animation</param>
        public static void ZoomInAnimation(FrameworkElement element, double timeInSeconds)
        {
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform trasform = element.RenderTransform as ScaleTransform;

            if (trasform == null)
            {
                element.RenderTransform = trasform = new ScaleTransform();
            }

            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromSeconds(timeInSeconds);
            animation.DecelerationRatio = 0.6;
            animation.From = 1.2;
            animation.To = 1;

            trasform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
            trasform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);

            FadeInAnimation(element, null, timeInSeconds);
        }

        /// <summary>
        /// Applies zoom out animation to the given element
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="timeInSeconds">Time of the animation</param>
        public static void ZoomOutAnimation(FrameworkElement element, double timeInSeconds)
        {
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform trasform = element.RenderTransform as ScaleTransform;

            if (trasform == null)
            {
                element.RenderTransform = trasform = new ScaleTransform();
            }

            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromSeconds(timeInSeconds);
            animation.AccelerationRatio = 0.6;
            animation.From = 1;
            animation.To = 0.8;

            trasform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
            trasform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);

            FadeOutAnimation(element, null, timeInSeconds);
        }

        /// <summary>
        /// Removes element from it's container after delay
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="panel">The container</param>
        /// <param name="delayInSeconds">Delay time in seconds</param>
        public static void RemoveElementFromContainerAfterDelay(FrameworkElement element, Panel panel, double delayInSeconds)
        {
            DelayActionParameters parameters = new DelayActionParameters(element, panel, delayInSeconds);
            Thread thread = new Thread(new ParameterizedThreadStart(new ParameterizedThreadStart(RemoveElementThreadStart)));
            thread.Start(parameters);
            threads.Add(thread);
        }

        /// <summary>
        /// Hides element after delay
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="delayInSeconds">Delay time in seconds</param>
        public static void HideElementAfterDelay(FrameworkElement element, double delayInSeconds)
        {
            DelayActionParameters parameters = new DelayActionParameters(element, delayInSeconds);
            Thread thread = new Thread(new ParameterizedThreadStart(new ParameterizedThreadStart(HideElementThreadStart)));
            thread.Start(parameters);
            threads.Add(thread);
        }

        /// <summary>
        /// Call a function by using the given dispatcher object after delay.
        /// </summary>
        /// <param name="delayInSeconds">Delay time in seconds</param>
        /// <param name="dispatcher">The dispatcher object</param>
        /// <param name="function">The function to call</param>
        /// <param name="parametersList">Function parameters</param>
        public static void CallFunctionAfterDelay(double delayInSeconds, Dispatcher dispatcher, Delegate function, params object[] parametersList)
        {
            DelayActionParameters parameters = new DelayActionParameters(dispatcher, function, delayInSeconds, parametersList);
            Thread thread = new Thread(new ParameterizedThreadStart(new ParameterizedThreadStart(CallFunctionThreadStart)));
            thread.Start(parameters);
            threads.Add(thread);
        }

        /// <summary>
        /// Thread start for control removal
        /// </summary>
        /// <param name="parameters">Removal parameters</param>
        private static void RemoveElementThreadStart(object parameters)
        {
            DelayActionParameters param = (DelayActionParameters)parameters;
            Thread.Sleep((int)(param.DelayInSeconds * 1000));
            if (param.Panel.Dispatcher.Thread == Thread.CurrentThread)
            {
                RemoveElementHelper(param.Element, param.Panel);
            }
            else
            {
                param.Panel.Dispatcher.BeginInvoke(new RemoveElementDelegate(RemoveElementHelper), param.Element, param.Panel).Wait();
            }

            threads.Remove(Thread.CurrentThread);
        }

        /// <summary>
        /// Thread start for control hiding
        /// </summary>
        /// <param name="parameters">Removal parameters</param>
        private static void HideElementThreadStart(object parameters)
        {
            DelayActionParameters param = (DelayActionParameters)parameters;
            Thread.Sleep((int)(param.DelayInSeconds * 1000));
            if (param.Element.Dispatcher.Thread == Thread.CurrentThread)
            {
                HideElementHelper(param.Element);
            }
            else
            {
                param.Element.Dispatcher.BeginInvoke(new HideElementDelegate(HideElementHelper), param.Element).Wait();
            }

            threads.Remove(Thread.CurrentThread);
        }

        /// <summary>
        /// Starts a function thread.
        /// </summary>
        /// <param name="parameters">Parameters to the function.</param>
        private static void CallFunctionThreadStart(object parameters)
        {
            DelayActionParameters param = (DelayActionParameters)parameters;
            Thread.Sleep((int)(param.DelayInSeconds * 1000));
            if (param.Dispatcher.Thread == Thread.CurrentThread)
            {
                switch (param.Parameters.Length)
                {
                    case 0:
                        param.Function.DynamicInvoke();
                        break;
                    case 1:
                        param.Function.DynamicInvoke(param.Parameters[0]);
                        break;
                    case 2:
                        param.Function.DynamicInvoke(param.Parameters[0], param.Parameters[1]);
                        break;
                    case 3:
                        param.Function.DynamicInvoke(param.Parameters[0], param.Parameters[1], param.Parameters[2]);
                        break;
                    case 4:
                        param.Function.DynamicInvoke(param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3]);
                        break;
                    case 5:
                        param.Function.DynamicInvoke(param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4]);
                        break;
                    case 6:
                        param.Function.DynamicInvoke(param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4], param.Parameters[5]);
                        break;
                    case 7:
                        param.Function.DynamicInvoke(param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4], param.Parameters[5], param.Parameters[6]);
                        break;
                    case 8:
                        param.Function.DynamicInvoke(param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4], param.Parameters[5], param.Parameters[6], param.Parameters[1]);
                        break;
                    case 9:
                        param.Function.DynamicInvoke(param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4], param.Parameters[5], param.Parameters[6], param.Parameters[7], param.Parameters[8]);
                        break;
                    case 10:
                        param.Function.DynamicInvoke(param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4], param.Parameters[5], param.Parameters[6], param.Parameters[7], param.Parameters[8], param.Parameters[9]);
                        break;
                    default:
                        throw new InvalidOperationException("Parameters are too much.");
                }
            }
            else
            {
                switch (param.Parameters.Length)
                {
                    case 0:
                        param.Dispatcher.BeginInvoke(param.Function).Wait();
                        break;
                    case 1:
                        param.Dispatcher.BeginInvoke(param.Function, param.Parameters[0]).Wait();
                        break;
                    case 2:
                        param.Dispatcher.BeginInvoke(param.Function, param.Parameters[0], param.Parameters[1]).Wait();
                        break;
                    case 3:
                        param.Dispatcher.BeginInvoke(param.Function, param.Parameters[0], param.Parameters[1], param.Parameters[2]).Wait();
                        break;
                    case 4:
                        param.Dispatcher.BeginInvoke(param.Function, param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3]).Wait();
                        break;
                    case 5:
                        param.Dispatcher.BeginInvoke(param.Function, param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4]).Wait();
                        break;
                    case 6:
                        param.Dispatcher.BeginInvoke(param.Function, param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4], param.Parameters[5]).Wait();
                        break;
                    case 7:
                        param.Dispatcher.BeginInvoke(param.Function, param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4], param.Parameters[5], param.Parameters[6]).Wait();
                        break;
                    case 8:
                        param.Dispatcher.BeginInvoke(param.Function, param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4], param.Parameters[5], param.Parameters[6], param.Parameters[1]).Wait();
                        break;
                    case 9:
                        param.Dispatcher.BeginInvoke(param.Function, param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4], param.Parameters[5], param.Parameters[6], param.Parameters[7], param.Parameters[8]).Wait();
                        break;
                    case 10:
                        param.Dispatcher.BeginInvoke(param.Function, param.Parameters[0], param.Parameters[1], param.Parameters[2], param.Parameters[3], param.Parameters[4], param.Parameters[5], param.Parameters[6], param.Parameters[7], param.Parameters[8], param.Parameters[9]).Wait();
                        break;
                    default:
                        throw new InvalidOperationException("Parameters are too much.");
                }
            }

            threads.Remove(Thread.CurrentThread);
        }

        /// <summary>
        /// Helper function for removing the given element from its panel
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="panel">The panel</param>
        private static void RemoveElementHelper(FrameworkElement element, Panel panel)
        {
            if (panel.Children.Contains(element))
            {
                panel.Children.Remove(element);
            }
        }

        /// <summary>
        /// Helper function for hiding the given element
        /// </summary>
        /// <param name="element">The element</param>
        private static void HideElementHelper(FrameworkElement element)
        {
            element.Visibility = Visibility.Hidden;
        }
    }
}
