using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.Phone.Controls;

namespace UsedParts.UI.Controls
{
    [TemplatePart(Name = "ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "loadingIndicator", Type = typeof(PerformanceProgressBar))]
    public class InfiniteListBox : ListBox
    {
        #region Dependency Properties

        public static DependencyProperty PreloadOffsetDeltaProperty =
            DependencyProperty.Register("PreloadOffsetDelta",
                                        typeof(double),
                                        typeof(InfiniteListBox),
                                        new PropertyMetadata(10.0, OnPreloadOffsetDeltaPropertyChanged));

        public static DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register("VerticalOffset",
                                        typeof(double),
                                        typeof(InfiniteListBox),
                                        new PropertyMetadata(OnVerticallOffsetPropertyChanged));

        public static DependencyProperty IsLoadingMoreItemsProperty =
            DependencyProperty.Register("IsLoadingMoreItems",
                                        typeof(bool),
                                        typeof(InfiniteListBox),
                                        new PropertyMetadata(OnIsLoadingPropertyChanged));

        public static DependencyProperty ProgressForegroundProperty = 
            DependencyProperty.Register("ProgressForeground",
                                        typeof (Brush),
                                        typeof(InfiniteListBox),
                                            null); 
                                                                                                  

        #endregion

        public event EventHandler LoadMoreItems;

        public InfiniteListBox()
        {
            DefaultStyleKey = typeof(InfiniteListBox);
        }

        #region Dependency Property Getters & Setters

        public double PreloadOffsetDelta
        {
            get { return (double)GetValue(PreloadOffsetDeltaProperty); }
            set { SetValue(PreloadOffsetDeltaProperty, value); }
        }

        public bool IsLoadingMoreItems
        {
            get { return (bool)GetValue(IsLoadingMoreItemsProperty); }
            set { SetValue(IsLoadingMoreItemsProperty, value); }
        }

        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(IsLoadingMoreItemsProperty, value); }
        }


        public Brush ProgressForeground
        {
            get
            {
                return GetValue(ProgressForegroundProperty) as Brush;
            }
            set
            {
                SetValue(ProgressForegroundProperty, value);
            }
        }
        
        #endregion

        public ScrollViewer Scroll { get; set; }
        private PerformanceProgressBar LoadingIndicator { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            Scroll = GetTemplateChild("ScrollViewer") as ScrollViewer;

            if (Scroll != null)
            {
                Scroll.Loaded += OnScrollViewerLoaded;
            }

            LoadingIndicator = GetTemplateChild("loadingIndicator") as PerformanceProgressBar;

        }

        #region Dependency Properties Callbacks

        private static void OnPreloadOffsetDeltaPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as InfiniteListBox;

            if (ctrl == null)
            {
                return;
            }

            ctrl.OnPreloadOffsetDeltaPropertyChanged(e);
        }

        private static void OnVerticallOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as InfiniteListBox;

            if (ctrl == null)
            {
                return;
            }

            ctrl.OnVerticallOffsetPropertyChanged(e);
        }

        private static void OnIsLoadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as InfiniteListBox;

            if (ctrl == null)
            {
                return;
            }

            ctrl.OnIsLoadingPropertyChanged(e);
        }

        #endregion

        private void OnScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            var scrolViewer = sender as ScrollViewer;

            if (scrolViewer == null)
            {
                return;
            }

            var binding = new Binding
            {
                Source = scrolViewer,
                Path = new PropertyPath("VerticalOffset"),
                Mode = BindingMode.OneWay
            };

            SetBinding(VerticalOffsetProperty, binding);
        }

        protected void OnPreloadOffsetDeltaPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            // TODO: Extend logic...
        }

        protected void OnIsLoadingPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                LoadingIndicator.IsIndeterminate = true;
                LoadingIndicator.Visibility = Visibility.Visible;
            }
            else
            {
                LoadingIndicator.IsIndeterminate = false;
                LoadingIndicator.Visibility = Visibility.Collapsed;
            }
        }

        
        protected void OnVerticallOffsetPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if ((double)e.NewValue > (double)e.OldValue)
            {
                TriggerLoadMoreItems();
            }    
        }

        public void TriggerLoadMoreItems()
        {
            if (IsLoadingMoreItems || 
                Scroll.ScrollableHeight == 0 || 
                (Scroll.VerticalOffset < Scroll.ScrollableHeight - PreloadOffsetDelta))
            {
                return;
            }

            var handler = LoadMoreItems;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
