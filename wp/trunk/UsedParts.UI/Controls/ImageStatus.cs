using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Microsoft.Phone;
using UsedParts.Common;

namespace UsedParts.UI.Controls
{
    public class ImageStatus : ContentControl
    {
        private TextBlock _txt;
        private Image _image;
        private Image _additionalImage;
        private bool _isPrimaryFailed;
        private bool _isDefaultFailed;

        private readonly IFileStorageManager _fileManager = IoC.Get<IFileStorageManager>();

        public static readonly DependencyProperty DefaultImageSourceProperty =
           DependencyProperty.Register("DefaultImageSource",
                                       typeof(string),
                                       typeof(ImageStatus),
                                       null);
        public string DefaultImageSource
        {
            get { return GetValue(DefaultImageSourceProperty) as string; }
            set { SetValue(DefaultImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
           DependencyProperty.Register("ImageSource",
                                       typeof(string),
                                       typeof(ImageStatus),
                                       null);
        public string ImageSource
        {
            get { return GetValue(ImageSourceProperty) as string; }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageLocalSourceProperty =
           DependencyProperty.Register("ImageLocalSource",
                                       typeof(string),
                                       typeof(ImageStatus),
                                       null);
        public string ImageLocalSource
        {
            get { return GetValue(ImageLocalSourceProperty) as string; }
            set { SetValue(ImageLocalSourceProperty, value); }
        }


        public static readonly DependencyProperty AdditionalImageSourceProperty =
           DependencyProperty.Register("AdditionalImageSource",
                                       typeof(string),
                                       typeof(ImageStatus),
                                       null);
        public string AdditionalImageSource
        {
            get { return GetValue(AdditionalImageSourceProperty) as string; }
            set { SetValue(AdditionalImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageStyleProperty =
            DependencyProperty.Register("ImageStyle",
                                        typeof(Style),
                                        typeof(ImageStatus),
                                        null);

        public Style ImageStyle
        {
            get { return (Style)GetValue(ImageStyleProperty); }
            set { SetValue(ImageStyleProperty, value); }
        }

        public static readonly DependencyProperty SourceTextProperty =
            DependencyProperty.Register("SourceText",
                                        typeof (string), 
                                        typeof (ImageStatus), 
                                        null);
        public string SourceText
        {
            get { return (string) GetValue(SourceTextProperty); }
            set { SetValue(SourceTextProperty, value);}
        }

        public static readonly DependencyProperty HideLoadingProperty =
           DependencyProperty.Register("HideLoading",
                                       typeof(bool),
                                       typeof(ImageStatus),
                                       new PropertyMetadata(false));
        public bool HideLoading
        {
            get { return (bool)GetValue(HideLoadingProperty); }
            set { SetValue(HideLoadingProperty, value); }
        }

        public ImageStatus()
        {
            DefaultStyleKey = typeof(ImageStatus);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _txt = (TextBlock)GetTemplateChild("LoadingText");
            _image = (Image)GetTemplateChild("RemoteImage");
            if (ImageSource == null || ImageSource == DefaultImageSource)
                return;
            if (!ImageSource.StartsWith("http"))
                _image.Source = null;
            _additionalImage = (Image)GetTemplateChild("AdditionalRemoteImage");
            _image.ImageOpened += OnImageOpened;
            _image.ImageFailed += OnImageFailed;
            _image.Loaded += OnImageLoaded;
            _additionalImage.ImageFailed += OnImageFailed;
        }

        private void OnImageLoaded(object sender, RoutedEventArgs e)
        {
            if (ImageSource == null)
                return;
            _txt.Visibility = Visibility.Visible;
            if (ImageSource.StartsWith("http") || ImageSource == DefaultImageSource)
                return;

            var image = (Image)sender;

            using (var imageStream = _fileManager.OpenFile(ImageSource))
            {
                const int maxImageSize = 100 * 1024;
                var imageSource = PictureDecoder.DecodeJpeg(imageStream);
                if (imageStream.Length > maxImageSize) // over 100k
                {
                    using (var ms = new MemoryStream())
                    {
                        var reduceRatio = 1f * maxImageSize / imageStream.Length;
                        var reduceQuality = Convert.ToInt32(100 * reduceRatio);
                        // TODO: find correct int value to keep correct proportions
                        //var reduceWidth = Convert.ToInt32(imageSource.PixelWidth * reduceRatio);
                        //var reduceHeight = Convert.ToInt32(imageSource.PixelHeight * reduceRatio);
                        imageSource.SaveJpeg(ms, imageSource.PixelWidth, imageSource.PixelHeight, 
                            0, reduceQuality);
                        imageSource.SetSource(ms);
                        image.Source = imageSource;
                    }
                }
                else
                    image.Source = imageSource;
                image.UpdateLayout();
            }

            if (HideLoading)
                _txt.Visibility = Visibility.Collapsed;


        //    var image = (Image)sender;
        //    var url = ImageSource;

        //    var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
        //    request.BeginGetResponse(ar =>
        //    {
        //        var requestState = (HttpWebRequest)ar.AsyncState;
        //        try
        //        {
        //            var response = (HttpWebResponse)requestState.EndGetResponse(ar);
        //            var responseStream = response.GetResponseStream();
        //            if (responseStream == null)
        //            {
        //                Dispatcher.BeginInvoke(() => OnImageFailed(null, null));
        //                return;
        //            }
        //            Dispatcher.BeginInvoke(() =>
        //            {
        //                //var writableBitmap = PictureDecoder.DecodeJpeg(responseStream, 240, 240);
        //                //writableBitmap.LoadJpeg(ms);
        //                var bitmap = new BitmapImage {CreateOptions = BitmapCreateOptions.IgnoreImageCache};
        //                bitmap.SetSource(responseStream);
        //                image.Source = bitmap;
        //                responseStream.Dispose();
        //                OnImageOpened(null, null);
        //            });
        //        }
        //        catch (WebException)
        //        {
        //            Dispatcher.BeginInvoke(() => OnImageFailed(null, null));
        //        }
        //    }, request);
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            if (HideLoading)
                _txt.Visibility = Visibility.Collapsed;

            //if (!ReduceSize)
            //    return;

            //var image = (Image)sender;
            //var bitmap = (BitmapImage)image.Source;
            //try
            //{
            //    var wBitmap = new WriteableBitmap(bitmap);
            //    using (var ms = new MemoryStream())
            //    {
            //        wBitmap.SaveJpeg(ms, 240, 240, 0, 100);
            //        bitmap.SetSource(ms);
            //    }
            //}
            //catch (NullReferenceException)
            //{
            //}
        }

        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (_isPrimaryFailed)
            {
                if (!_isDefaultFailed)
                {
                    _isDefaultFailed = true;
                    ImageSource = DefaultImageSource;
                }
            }
            else
            {
                if (AdditionalImageSource != null)
                {
                    _isPrimaryFailed = true;
                    _additionalImage.Source = new BitmapImage(new Uri(AdditionalImageSource));
                    _additionalImage.Visibility = Visibility.Visible;
                }
                else
                    ImageSource = DefaultImageSource;
            }
        }

    }
}
