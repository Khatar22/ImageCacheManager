using System.Diagnostics;
using System.Net.Http;
using ImageCacheManager.Models;

namespace ImageCacheManager.Patterns
{
    public sealed class ImageProxy
    {
        private static readonly HttpClient HttpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        private readonly ImageMetadata _metadata;
        private byte[]? _imageData;

        public ImageMetadata Metadata => _metadata;

        public bool IsLoaded => _imageData is not null;

        public long LoadedBytes => _imageData?.LongLength ?? 0;

        public ImageSettings Settings { get; }

        public ImageProxy(ImageMetadata metadata)
        {
            ArgumentNullException.ThrowIfNull(metadata);

            if (!Uri.TryCreate(
                    metadata.Url,
                    UriKind.Absolute,
                    out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp &&
                 uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException(
                    "Укажите корректный HTTP- или HTTPS-адрес изображения.",
                    nameof(metadata));
            }

            _metadata = metadata;
            Settings = ImageSettingsFactory.Instance
                .GetSettings(metadata.Type);

            Console.WriteLine(
                $"[Proxy] Создан прокси для изображения: {_metadata.Name}");
        }

        /// Загружает изображение при первом обращении
        /// и повторно использует данные из памяти.
        public async Task DisplayAsync()
        {
            if (!IsLoaded)
            {
                await LoadImageAsync();
            }
            else
            {
                Console.WriteLine(
                    $"[Proxy] {_metadata.Name} уже находится в памяти. " +
                    "Повторная загрузка не требуется.");
            }

            Console.WriteLine(
                $"[Proxy] Изображение готово: {_metadata.Name}");

            Console.WriteLine(
                $"[Proxy] Размер данных: {LoadedBytes:N0} байт");

            Console.WriteLine(
                $"[Proxy] Общие настройки: {Settings}");
        }

        private async Task LoadImageAsync()
        {
            Console.WriteLine(
                $"[Proxy] Скачивание изображения: {_metadata.Url}");

            var stopwatch = Stopwatch.StartNew();

            using var response = await HttpClient.GetAsync(
                _metadata.Url,
                HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var imageData = await response.Content.ReadAsByteArrayAsync();

            if (imageData.Length == 0)
            {
                throw new InvalidOperationException(
                    $"Сервер вернул пустые данные для {_metadata.Name}.");
            }

            _imageData = imageData;

            stopwatch.Stop();

            Console.WriteLine(
                $"[Proxy] Изображение скачано: {_metadata.Name}");

            Console.WriteLine(
                $"[Proxy] Время загрузки: {stopwatch.ElapsedMilliseconds} мс");
        }
    }

}
