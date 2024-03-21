using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RAA_GetThumbnails
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageWindow : Window
    {
        public ObservableCollection<ImageEntity> Images { get; set; }
        public ImageWindow(List<ImageEntity> imageList)
        {
            InitializeComponent();
            Images = new ObservableCollection<ImageEntity>();
            LoadImages(imageList);
        }

        // Method to add images, call this method to populate your grid
        public void LoadImages(IEnumerable<ImageEntity> imageEntities)
        {
            foreach (var imageEntity in imageEntities)
            {
                Images.Add(imageEntity);
            }

            ImageGrid.ItemsSource = Images;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Here you can handle the event when an image is clicked
            // For example, you can get the selected image and perform actions accordingly
            ImageEntity selectedImage = ((FrameworkElement)sender).DataContext as ImageEntity;
            if (selectedImage != null)
            {
                // Example: Show a message when an image is clicked
                TaskDialog td = new TaskDialog("Image Clicked");
                td.MainInstruction = "Insert family into Revit";
                td.Show();
            }
        }
    }
}
