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

        var query = from e in _exhibits select e.ToString();
        Console.WriteLine(string.Join(Environment.NewLine, query));
    }

    public void AddExhibit(Exhibit exhibit)
    {
        bool exists = (from e in _exhibits where e.Id == exhibit.Id select e).Any();
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
        Exhibit? toRemove = (from e in _exhibits where e.Id == id select e).FirstOrDefault();
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
        var query = from e in _exhibits
                    where string.Equals(e.Material, material, StringComparison.OrdinalIgnoreCase)
                    select e;
        return query.ToList();
    }

    public List<Exhibit> GetByYearAfter(int year)
    {
        var query = from e in _exhibits
                    where e.Year > year
                    select e;
        return query.ToList();
    }

    public decimal GetAverageValue()
    {
        if (_exhibits.Count == 0)
            return 0;

        decimal sum = (from e in _exhibits select e.Value).Sum();
        return sum / _exhibits.Count;
    }

    public int GetCountByArtist(string artist)
    {
        var query = from e in _exhibits
                    where string.Equals(e.Artist, artist, StringComparison.OrdinalIgnoreCase)
                    select e;
        return query.Count();
    }

    public bool ExistsId(int id)
    {
        return (from e in _exhibits where e.Id == id select e).Any();
    }
}
