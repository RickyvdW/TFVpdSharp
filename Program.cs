using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using TFVpdSharp;
using TFVpdSharp.Serialization;

if (args.Length < 1)
{
    Console.WriteLine("Missing input file argument.");
    return;
}

string filePath = args[0];
if (!File.Exists(filePath))
{
    Console.WriteLine($"Could not find input file : {filePath}.");
    return;
}

// If your file isn't valid JSON, the JSON parser will throw an exception.
try
{
    using FileStream fileStream = File.OpenRead(filePath);
    VpdDiskData? vpdDiskData = JsonSerializer.Deserialize<VpdDiskData>(fileStream, 
        new JsonSerializerOptions
        {
            MaxDepth = 128,
            Converters = { new JsonStringEnumConverter(), new LegacyVpdDiskDataConverter() }
        }
    );
    fileStream.Close();

    if (vpdDiskData == null)
    {
        // Something went wrong during JSON deserialization step.
        Console.WriteLine($"Reading input .json file failed. {vpdDiskData}");
        return;
    }

    // Write to .vpd file.
    string vpdPath = Path.ChangeExtension(filePath, ".generated.vpd");
    using var stream = File.Open(vpdPath, FileMode.Create);
    using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
    vpdDiskData.Write(writer);
}
catch (Exception e)
{
    Console.WriteLine(e);
}