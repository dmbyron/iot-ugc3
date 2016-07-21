using StudioUgc.Models;
using StudioUgc.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace StudioUgc
{
	public sealed partial class MainPage : Page
    {
		private const int IntervalInSeconds = 5;

		private int _ugcIndex = -1;
		private IList<IUgc> _ugcs = null;

        public MainPage()
        {
            InitializeComponent();
			LoadUgc();
		}

		private async void LoadUgc()
		{
			var ugcService = new UgcService();
			_ugcs = await ugcService.GetUgc("C:\\Users\\dillon.byron\\Pictures\\PresentationPhotos");

			if (_ugcs.Count == 0)
			{
				NoUgc.Visibility = Visibility.Visible;
				return;
			}

			NoUgc.Visibility = Visibility.Collapsed;
			AdvanceUgc();
		}

		private void AdvanceUgc()
		{
			_ugcIndex++;

			if (_ugcIndex >= _ugcs.Count)
			{
				_ugcIndex = 0;
			}

			ShowUgc(_ugcs[_ugcIndex]);
		}

		private void ShowUgc(IUgc ugc)
		{
			switch (ugc.Type)
			{
				case UgcType.Image:
					ShowImage(ugc as Models.Image);
					break;
				case UgcType.Video:
					PlayVideo(ugc as Models.Video);
					break;
			}
		}

		private async void ShowImage(Models.Image image)
		{
			var stream = await image.File.OpenAsync(Windows.Storage.FileAccessMode.Read);

			var bitmap = new BitmapImage();
			bitmap.ImageOpened += Image_ImageOpened;
			bitmap.ImageFailed += Image_ImageFailed;
			bitmap.SetSource(stream);

			Photo.Source = bitmap;
			Photo.Visibility = Visibility.Visible;
		}

		private async void PlayVideo(Video video)
		{
			var stream = await video.File.OpenAsync(Windows.Storage.FileAccessMode.Read);

			Video.SetSource(stream, string.Empty);
			Video.Play();
			Video.Visibility = Visibility.Visible;
		}

		private void Image_ImageOpened(object sender, RoutedEventArgs e)
		{
			Task.Delay(IntervalInSeconds * 1000).ContinueWith(t =>
			{
				Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
					Photo.Visibility = Visibility.Collapsed;
					AdvanceUgc();
				}).AsTask();
			});
		}

		private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
		{
			//throw new NotImplementedException();
		}

		private void Video_MediaEnded(object sender, RoutedEventArgs e)
		{
			Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
				Video.Visibility = Visibility.Collapsed;
				AdvanceUgc();
			}).AsTask();
		}

		private void Video_MediaFailed(object sender, ExceptionRoutedEventArgs e)
		{
			//throw new NotImplementedException();
		}
	}
}
