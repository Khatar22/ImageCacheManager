
using ImageCacheManager.Models;
using ImageCacheManager.Patterns;

Console.WriteLine("==========================================");
Console.WriteLine("       МЕНЕДЖЕР КЭША ИЗОБРАЖЕНИЙ");
Console.WriteLine("==========================================");

try
{
    Console.WriteLine("\n=== 1. SINGLETON ===");

    var cache1 = ImageCache.Instance;
    var cache2 = ImageCache.Instance;

    Console.WriteLine(
        $"Оба обращения возвращают один кэш: " +
        $"{ReferenceEquals(cache1, cache2)}");

    Console.WriteLine("\n=== 2. PROXY: РЕАЛЬНАЯ ЗАГРУЗКА ===");

    var banner = cache1.GetImage(
        "banner1.png",
        "https://placehold.co/1200x400/png?text=Banner+1",
        ImageType.Banner);

    Console.WriteLine(
        $"Изображение загружено до первого обращения: " +
        $"{banner.IsLoaded}");

    Console.WriteLine("\nПервое обращение к изображению:");
    await banner.DisplayAsync();

    Console.WriteLine(
        $"Изображение загружено: {banner.IsLoaded}");

    Console.WriteLine("\nПовторное обращение к изображению:");
    await banner.DisplayAsync();

    Console.WriteLine("\n=== 3. FLYWEIGHT: ОБЩИЕ НАСТРОЙКИ ===");

    var banner2 = cache1.GetImage(
        "banner2.png",
        "https://placehold.co/1200x400/png?text=Banner+2",
        ImageType.Banner);

    var banner3 = cache1.GetImage(
        "banner3.png",
        "https://placehold.co/1200x400/png?text=Banner+3",
        ImageType.Banner);


    Console.WriteLine(
        $"Настройки первого и второго баннеров совпадают: " +
        $"{ReferenceEquals(banner.Settings, banner2.Settings)}");

    Console.WriteLine(
        $"Настройки второго и третьего баннеров совпадают: " +
        $"{ReferenceEquals(banner2.Settings, banner3.Settings)}");

    Console.WriteLine("\nЗагружаем остальные изображения:");

    await banner2.DisplayAsync();
    await banner3.DisplayAsync();

    Console.WriteLine("\n=== 4. РЕЗУЛЬТАТЫ КЭШИРОВАНИЯ ===");

    Console.WriteLine(
        $"Количество изображений в кэше: {cache1.Count}");

    Console.WriteLine(
        $"Количество уникальных объектов настроек: " +
        $"{ImageSettingsFactory.Instance.UniqueSettingsCount}");

    Console.WriteLine(
        $"Общий объём загруженных данных: " +
        $"{cache1.TotalLoadedBytes:N0} байт");

    Console.WriteLine("\n=== 5. ПРОВЕРКА ПОВТОРНОГО ЗАПРОСА ===");

    var sameBanner = cache1.GetImage(
        "another-name.png",
        "https://placehold.co/1200x400/png?text=Banner+1",
        ImageType.Banner);

    Console.WriteLine(
        $"Возвращён тот же прокси: " +
        $"{ReferenceEquals(banner, sameBanner)}");

    Console.WriteLine(
        $"Данные уже загружены: {sameBanner.IsLoaded}");

    Console.WriteLine("\nДемонстрация завершена.");
}
catch (HttpRequestException ex)
{
    Console.WriteLine(
        $"\nОшибка HTTP-запроса: {ex.Message}");

    Console.WriteLine(
        "Проверь подключение к интернету и доступность сервера.");
}
catch (TaskCanceledException)
{
    Console.WriteLine(
        "\nЗагрузка отменена или превышено время ожидания.");
}
catch (Exception ex)
{
    Console.WriteLine(
        $"\nОшибка: {ex.Message}");
}

Console.WriteLine("\nНажмите Enter для завершения.");
Console.ReadLine();
