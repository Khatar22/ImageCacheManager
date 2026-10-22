using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCacheManager.Models
{
    /// Индивидуальные данные конкретного изображения.
    public sealed class ImageMetadata
    {
        public string Name { get; }
        public string Url { get; }
        public ImageType Type { get; }

        public ImageMetadata(string name, string url, ImageType type)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(url);

            Name = name;
            Url = url;
            Type = type;
        }
    }
}
