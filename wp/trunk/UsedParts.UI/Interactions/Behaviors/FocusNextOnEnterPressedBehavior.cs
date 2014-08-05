using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;

namespace UsedParts.UI.Interactions.Behaviors
{
    public class FocusNextOnEnterPressedBehavior : SafeBehavior<Control>
    {
        public static readonly DependencyProperty NextControlNameProperty = DependencyProperty.Register(
            "NextControlName",
            typeof(string),
            typeof(FocusNextOnEnterPressedBehavior),
            null);

        public string NextControlName
        {
            get { return (string)GetValue(NextControlNameProperty); }
            set { SetValue(NextControlNameProperty, value); }
        }

        public FocusNextOnEnterPressedBehavior()
        {
            ListenToPageBackEvent = true;
        }

        protected override void OnSetup()
        {
            base.OnSetup();

            AssociatedObject.KeyUp += OnKeyUp;
        }

        protected override void OnCleanup()
        {
            base.OnCleanup();
            AssociatedObject.KeyUp -= OnKeyUp;
        }

        void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            var page = AssociatedObject.Ancestors<PhoneApplicationPage>().First();
            if (page == null)
                return;

            if (string.IsNullOrEmpty(NextControlName))
            {
                page.Focus();
            }
            else
            {
                var nextControl = page.FindName(NextControlName) as Control ??
                                  page.Descendants<Control>().FirstOrDefault(c => c.Name == NextControlName);
                if (nextControl == null) 
                    throw new InvalidOperationException(string.Format("Control '{0}' couldn't be found", NextControlName));

                nextControl.Focus();
                var scrollViewer = AssociatedObject.Ancestors<ScrollViewer>().FirstOrDefault();
                if (scrollViewer != null)
                {
                    var transform = AssociatedObject.TransformToVisual(scrollViewer);
                    var currentPoint = transform.Transform(new Point(0, 0));
                    transform = nextControl.TransformToVisual(scrollViewer);
                    var nextPoint = transform.Transform(new Point(0, 0));
                    var deltaVertical = nextPoint.Y - currentPoint.Y;
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + deltaVertical);
                    scrollViewer.UpdateLayout();
                }
            }
        }
    }
}
