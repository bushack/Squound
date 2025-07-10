using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquoundApp_v1.Models
{
    public class ProductModel
    {
        private enum ImageIndex
        {
            IMAGE_0,
            IMAGE_1,
            IMAGE_2,
            IMAGE_3,
            IMAGE_4,
            IMAGE_5,
            IMAGE_COUNT
        }

        public ObservableCollection<string> Images { get; } = new();

        private string GetImage(ImageIndex index)
        {
            return Images.ElementAt<string>((int)index);
        }

        private void SetImage(ImageIndex index, string value)
        {
            // Ensure the Images collection has enough space for the image url.
            while (Images.Count <= (int)index)
            {
                Images.Add(string.Empty);
            }

            Images[(int)index] = value;
        }

        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Manufacturer { get; set; }
        public required string Description { get; set; }
        public required string Price { get; set; }
        public required string Image0 { get => GetImage(ImageIndex.IMAGE_0); set => SetImage(ImageIndex.IMAGE_0, value); }
        public required string Image1 { get => GetImage(ImageIndex.IMAGE_1); set => SetImage(ImageIndex.IMAGE_1, value); }
        public required string Image2 { get => GetImage(ImageIndex.IMAGE_2); set => SetImage(ImageIndex.IMAGE_2, value); }
        public required string Image3 { get => GetImage(ImageIndex.IMAGE_3); set => SetImage(ImageIndex.IMAGE_3, value); }
        public required string Image4 { get => GetImage(ImageIndex.IMAGE_4); set => SetImage(ImageIndex.IMAGE_4, value); }
        public required string Image5 { get => GetImage(ImageIndex.IMAGE_5); set => SetImage(ImageIndex.IMAGE_5, value); }
    }
}
