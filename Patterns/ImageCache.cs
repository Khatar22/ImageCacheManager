using global::ImageCacheManager.Models;
using ImageCacheManager.Models;

namespace ImageCacheManager.Patterns
{
    /// Единственный кэш изображений в приложении.
    /// Реализует паттерн Singleton.
    public sealed class ImageCache
    {
        private static readonly Lazy<ImageCache> _instance =
            new(() => new ImageCache());

        private readonly Dictionary<string, ImageProxy> _images =
            new(StringComparer.Ordinal);

        private ImageCache()
        {
            Console.WriteLine(
                "[Singleton] Создан единственный экземпляр кэша.");
        }

        public static ImageCache Instance => _instance.Value;

        /// Возвращает существующий прокси или создаёт новый.
        public ImageProxy GetImage(
            string name,
            string url,
            ImageType type)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(url);

            if (_images.TryGetValue(url, out var existingImage))
            {
                Console.WriteLine(
                    $"[Singleton] Прокси найден в кэше: {url}");

                return existingImage;
            }

            var image = new ImageProxy(
                new ImageMetadata(name, url, type));

            _images[url] = image;

            Console.WriteLine(
                $"[Singleton] Новый прокси добавлен в кэш: {url}");

            return image;
        }

        public int Count => _images.Count;

        /// Общий объём скачанных данных изображений в байтах.
        public long TotalLoadedBytes =>
            _images.Values.Sum(image => image.LoadedBytes);
    }
}
