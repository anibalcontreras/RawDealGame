using System.Text.Json;
using RawDeal.Exceptions;
namespace RawDeal.Loaders;
public class JsonLoader<T>
{
    private readonly string _dataPath;
    public JsonLoader(string dataPath)
    {
        _dataPath = dataPath;
    }
    public List<T> LoadFromJson()
    {
        if (File.Exists(_dataPath))
        {
            string json = File.ReadAllText(_dataPath);
            try
            {
                List<T> deserializedData = 
                    JsonSerializer.Deserialize<List<T>>(json) ?? 
                    throw new DeserializationNullException();
                return deserializedData;
            }
            catch (JsonException)
            {
                throw new DeserializationNullException();
            }
        }
        throw new EmptyFileException();
    }
}