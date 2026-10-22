using global::ImageCacheManager.Models;
using ImageCacheManager.Models;

namespace ImageCacheManager.Patterns
{
    /// Фабрика общих настроек изображений.
    /// Реализует паттерн Flyweight.
    public sealed class ImageSettingsFactory
    {
        private static readonly Lazy<ImageSettingsFactory> _instance =
                new(() => new ImageSettingsFactory());

        public static ImageSettingsFactory Instance => _instance.Value;

        private readonly Dictionary<ImageType, ImageSettings> _settings = new();

        private ImageSettingsFactory()
        {
        }
        public ImageSettings GetSettings(ImageType type)
        {
            if (_settings.TryGetValue(type, out var existingSettings))
            {
                Console.WriteLine(
                    $"[Flyweight] Повторно используем настройки: {type}");

                return existingSettings;
            }

            var settings = CreateSettings(type);

            _settings[type] = settings;

            Console.WriteLine(
                $"[Flyweight] Созданы общие настройки: {type}");

            return settings;
        }

        private static ImageSettings CreateSettings(ImageType type)
        {
            return type switch
            {
                ImageType.Icon => new ImageSettings(
                    64, 64, "Монохромная", "Arial"),

                ImageType.Banner => new ImageSettings(
                    1200, 400, "Яркая", "Arial"),

                ImageType.Preview => new ImageSettings(
                    320, 200, "Естественная", "Verdana"),

                _ => throw new ArgumentOutOfRangeException(
                    nameof(type), type, "Неизвестный тип изображения")
            };
        }

        public int UniqueSettingsCount => _settings.Count;
    }
}
