internal class Program
{
    private static DatabaseHelper _db;

    private static void Main()
    {
        string fileName = "exhibits.dat";
        _db = new DatabaseHelper(fileName);
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n=== МУЗЕЙНЫЕ ЭКСПОНАТЫ ===");
            Console.WriteLine("1. Просмотр всех экспонатов");
            Console.WriteLine("2. Добавить экспонат");
            Console.WriteLine("3. Удалить экспонат по ID");
            Console.WriteLine("4. Запрос: экспонаты заданного материала");
            Console.WriteLine("5. Запрос: экспонаты после заданного года");
            Console.WriteLine("6. Запрос: средняя стоимость экспонатов");
            Console.WriteLine("7. Запрос: количество экспонатов автора");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    _db.ViewAll();
                    break;
                case "2":
                    AddExhibitDialog();
                    break;
                case "3":
                    DeleteExhibitDialog();
                    break;
                case "4":
                    QueryByMaterialDialog();
                    break;
                case "5":
                    QueryByYearDialog();
                    break;
                case "6":
                    QueryAverageValue();
                    break;
                case "7":
                    QueryCountByArtistDialog();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    break;
            }
        }
    }

    private static void AddExhibitDialog()
    {
        Console.WriteLine("\n--- Добавление нового экспоната ---");

        int id = ReadInt("Введите ID (целое число): ");
        if (_db.ExistsId(id))
        {
            Console.WriteLine($"Экспонат с ID {id} уже существует. Добавление отменено.");
            return;
        }

        string name = ReadNonEmptyString("Введите название: ");
        string artist = ReadNonEmptyString("Введите автора/создателя: ");
        int year = ReadInt("Введите год создания (положительное число): ", 1, DateTime.Now.Year);
        string material = ReadNonEmptyString("Введите материал: ");
        decimal value = ReadDecimal("Введите стоимость (неотрицательная): ", 0, decimal.MaxValue);

        Exhibit newExhibit = new Exhibit(id, name, artist, year, material, value);
        _db.AddExhibit(newExhibit);
    }

    private static void DeleteExhibitDialog()
    {
        int id = ReadInt("Введите ID экспоната для удаления: ");
        _db.DeleteById(id);
    }

    private static void QueryByMaterialDialog()
    {
        string material = ReadNonEmptyString("Введите материал: ");
        List<Exhibit> result = _db.GetByMaterial(material);

        if (result.Count == 0)
        {
            Console.WriteLine($"Экспонатов из материала '{material}' не найдено.");
        }
        else
        {
            Console.WriteLine($"\nЭкспонаты из материала '{material}':");
            foreach (Exhibit e in result)
            {
                Console.WriteLine(e);
            }
        }
    }

    private static void QueryByYearDialog()
    {
        int year = ReadInt("Введите год (будут показаны экспонаты созданные ПОСЛЕ этого года): ");
        List<Exhibit> result = _db.GetByYearAfter(year);

        if (result.Count == 0)
        {
            Console.WriteLine($"Экспонатов, созданных после {year} года, не найдено.");
        }
        else
        {
            Console.WriteLine($"\nЭкспонаты, созданные после {year} года:");
            foreach (Exhibit e in result)
            {
                Console.WriteLine(e);
            }
        }
    }

    private static void QueryAverageValue()
    {
        decimal avg = _db.GetAverageValue();
        Console.WriteLine($"Средняя стоимость экспонатов: {avg:F2} руб.");
    }

    private static void QueryCountByArtistDialog()
    {
        string artist = ReadNonEmptyString("Введите имя автора: ");
        int count = _db.GetCountByArtist(artist);
        Console.WriteLine($"Количество экспонатов автора '{artist}': {count}");
    }

    private static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        int value = 0;
        bool valid = false;
        do
        {
            Console.Write(prompt);
            valid = int.TryParse(Console.ReadLine(), out value) && value >= min && value <= max;
            if (!valid)
            {
                Console.WriteLine($"Ошибка: введите целое число в диапазоне [{min}, {max}].");
            }
        } while (!valid);
        return value;
    }

    private static decimal ReadDecimal(string prompt, decimal min = decimal.MinValue, decimal max = decimal.MaxValue)
    {
        decimal value = 0;
        bool valid = false;
        do
        {
            Console.Write(prompt);
            valid = decimal.TryParse(Console.ReadLine(), out value) && value >= min && value <= max;
            if (!valid)
                Console.WriteLine($"Ошибка: введите число в диапазоне [{min}, {max}].");
        } while (!valid);
        return value;
    }

    private static string ReadNonEmptyString(string prompt)
    {
        string? input = "";
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(input))
                return input;
            Console.WriteLine("Ошибка: строка не может быть пустой.");
        } while (true);
    }
}
