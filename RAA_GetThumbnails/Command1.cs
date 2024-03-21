using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Drawing; // Add this namespace for Image

namespace RAA_GetThumbnails
{
    // Define the ImageEntity class to hold image data
    public class ImageEntity
    {
        public string Name { get; set; }
        public Image Image { get; set; }
    }

    [Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // Get image thumbnails from elements in the document
            List<ImageEntity> imageData = GetElementThumbnails(doc);

            // Show the images in a window
            ImageWindow imageWindow = new ImageWindow(imageData);
            imageWindow.ShowDialog();

            return Result.Succeeded;
        }

        private List<ImageEntity> GetElementThumbnails(Document doc)
        {
            List<ImageEntity> imageData = new List<ImageEntity>();

            // Define a filtered element collector to get all element types

            FilteredElementCollector collector = new FilteredElementCollector(doc);

            collector.OfClass(typeof(FamilyInstance));

            foreach (FamilyInstance fi in collector)
            {
                Debug.Assert(null != fi.Category,
                  "expected family instance to have a valid category");

                ElementId typeId = fi.GetTypeId();

                ElementType type = doc.GetElement(typeId) as ElementType;
                // Get the preview image of the element type
                Bitmap previewImage = type.GetPreviewImage(new Size(100, 100)); // Adjust size as needed

                if (previewImage != null)
                {
                    // Convert Image to BitmapSource
                    // Assuming you have an ImageEntity class with Name and Image properties
                    ImageEntity imageEntity = new ImageEntity();
                    imageEntity.Name = type.Name;
                    imageEntity.Image = previewImage; // Assuming ImageEntity has Image property of type Image
                    imageData.Add(imageEntity);
                }
            }

            return imageData;
        }

        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand1";
            string buttonTitle = "Button 1";

            ButtonDataClass myButtonData1 = new ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Blue_32,
                Properties.Resources.Blue_16,
                "This is a tooltip for Button 1");

            return myButtonData1.Data;
        }
    }
}
