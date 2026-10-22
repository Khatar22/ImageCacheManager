using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCacheManager.Models
{
    /// Общие настройки, которые могут разделять изображения одного типа.
    public sealed class ImageSettings
    {
        public int Width { get; }
        public int Height { get; }
        public string Palette { get; }
        public string CaptionFont { get; }

        public ImageSettings(
            int width,
            int height,
            string palette,
            string captionFont)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width));

            if (height <= 0)
                throw new ArgumentOutOfRangeException(nameof(height));

            ArgumentException.ThrowIfNullOrWhiteSpace(palette);
            ArgumentException.ThrowIfNullOrWhiteSpace(captionFont);

            Width = width;
            Height = height;
            Palette = palette;
            CaptionFont = captionFont;
        }

        public override string ToString()
        {
            return $"{Width}x{Height}, палитра: {Palette}, " +
                   $"шрифт: {CaptionFont}";
        }
    }
}
