using System;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StorageFileQueryResultBug
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void FolderPickerButton_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.Desktop,
                ViewMode = PickerViewMode.Thumbnail
            };
            folderPicker.FileTypeFilter.Add("*");
            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder == null)
            {
                return;
            }

            var queryOptions = new QueryOptions
            {
                IndexerOption = IndexerOption.UseIndexerWhenAvailable,
            };
            var query = folder.CreateFileQueryWithOptions(queryOptions);

            var files = (await query.GetFilesAsync());
            foreach (var file in files)
            {
                var imageProperties = await file.Properties.GetImagePropertiesAsync();
                folderThenQueryMethodTextBlock.Text =
                    $"Dimensions {imageProperties.Width}x{imageProperties.Height} DateTaken {imageProperties.DateTaken}";
            }
        }
        private async void OpenFileAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add("*");
            picker.SuggestedStartLocation = PickerLocationId.Desktop;

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var imageProperties = await file.Properties.GetImagePropertiesAsync();
                filePickerMethodTextBlock.Text =
                    $"Dimensions {imageProperties.Width}x{imageProperties.Height} DateTaken {imageProperties.DateTaken}";
            }
        }
    }
}
