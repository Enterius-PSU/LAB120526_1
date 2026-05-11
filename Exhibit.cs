public class Exhibit
{
    private int _id;
    private string _name;
    private string _artist;
    private int _year;
    private string _material;
    private decimal _value;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Artist
    {
        get { return _artist; }
        set { _artist = value; }
    }

    public int Year
    {
        get { return _year; }
        set { _year = value; }
    }

    public string Material
    {
        get { return _material; }
        set { _material = value; }
    }

    public decimal Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public Exhibit()
    {
        _id = 0;
        _name = string.Empty;
        _artist = string.Empty;
        _year = 0;
        _material = string.Empty;
        _value = 0;
    }

    public Exhibit(int id, string name, string artist, int year, string material, decimal value)
    {
        _id = id;
        _name = name;
        _artist = artist;
        _year = year;
        _material = material;
        _value = value;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(_id);
        writer.Write(_name);
        writer.Write(_artist);
        writer.Write(_year);
        writer.Write(_material);
        writer.Write(_value);
    }

    public Exhibit(BinaryReader reader)
    {
        _id = reader.ReadInt32();
        _name = reader.ReadString();
        _artist = reader.ReadString();
        _year = reader.ReadInt32();
        _material = reader.ReadString();
        _value = reader.ReadDecimal();
    }

    public override string ToString()
    {
        return $"ID: {_id}, Название: {_name}, Автор: {_artist}, Год: {_year}, Материал: {_material}, Стоимость: {_value:F2} руб.";
    }
}