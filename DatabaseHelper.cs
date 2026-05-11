internal class DatabaseHelper
{
    private readonly string _filePath;
    private List<Exhibit> _exhibits;

    public DatabaseHelper(string filePath)
    {
        _filePath = filePath;
        _exhibits = new List<Exhibit>();
        LoadFromFile();
    }

    private void LoadFromFile()
    {
        if (!File.Exists(_filePath))
        {
            _exhibits = new List<Exhibit>();
            return;
        }

        try
        {
            using (FileStream fs = new FileStream(_filePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                int count = reader.ReadInt32();
                _exhibits = new List<Exhibit>(count);
                for (int i = 0; i < count; i++)
                {
                    _exhibits.Add(new Exhibit(reader));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            _exhibits = new List<Exhibit>();
        }
    }

    private void SaveToFile()
    {
        try
        {
            using (FileStream fs = new FileStream(_filePath, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                writer.Write(_exhibits.Count);
                foreach (Exhibit e in _exhibits)
                {
                    e.Write(writer);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
        }
    }

    public void ViewAll()
    {
        if (_exhibits.Count == 0)
        {
            Console.WriteLine("База данных пуста.");
            return;
        }

        foreach (Exhibit e in _exhibits)
        {
            Console.WriteLine(e);
        }
    }

    public void AddExhibit(Exhibit exhibit)
    {
        bool exists = false;
        foreach (Exhibit e in _exhibits)
        {
            if (e.Id == exhibit.Id)
            {
                exists = true;
                break;
            }
        }

        if (exists)
        {
            Console.WriteLine($"Экспонат с ID {exhibit.Id} уже существует. Добавление отменено.");
            return;
        }

        _exhibits.Add(exhibit);
        SaveToFile();
        Console.WriteLine("Экспонат добавлен.");
    }

    public void DeleteById(int id)
    {
        Exhibit? toRemove = null;
        foreach (Exhibit e in _exhibits)
        {
            if (e.Id == id)
            {
                toRemove = e;
                break;
            }
        }

        if (toRemove == null)
        {
            Console.WriteLine($"Экспонат с ID {id} не найден.");
            return;
        }

        _exhibits.Remove(toRemove);
        SaveToFile();
        Console.WriteLine($"Экспонат с ID {id} удалён.");
    }

    public List<Exhibit> GetByMaterial(string material)
    {
        List<Exhibit> result = new List<Exhibit>();
        foreach (Exhibit e in _exhibits)
        {
            if (string.Equals(e.Material, material, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(e);
            }
        }
        return result;
    }

    public List<Exhibit> GetByYearAfter(int year)
    {
        List<Exhibit> result = new List<Exhibit>();
        foreach (Exhibit e in _exhibits)
        {
            if (e.Year > year)
            {
                result.Add(e);
            }
        }
        return result;
    }

    public decimal GetAverageValue()
    {
        if (_exhibits.Count == 0)
            return 0;

        decimal sum = 0;
        foreach (Exhibit e in _exhibits)
        {
            sum += e.Value;
        }
        return sum / _exhibits.Count;
    }

    public int GetCountByArtist(string artist)
    {
        int count = 0;
        foreach (Exhibit e in _exhibits)
        {
            if (string.Equals(e.Artist, artist, StringComparison.OrdinalIgnoreCase))
            {
                count++;
            }
        }
        return count;
    }

    public bool ExistsId(int id)
    {
        foreach (Exhibit e in _exhibits)
        {
            if (e.Id == id)
                return true;
        }
        return false;
    }
}