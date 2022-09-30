using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using TFVpdSharp;
using TFVpdSharp.Serialization;

string filePath = "M:\\SteamLibrary\\steamapps\\common\\Team Fortress 2\\tf\\scripts\\protodefs\\proto_defs.json";

using FileStream fileStream = File.OpenRead(filePath);
JsonSerializerOptions options = new JsonSerializerOptions {
    MaxDepth = 128,
    Converters = { new JsonStringEnumConverter(), new LegacyVpdDiskDataConverter() }
};
VpdDiskData? vpdDiskData = JsonSerializer.Deserialize<VpdDiskData>(fileStream, options);
fileStream.Close();

// Write to .vpd file.
string vpdPath = Path.ChangeExtension(filePath, ".generated.vpd");
using (var stream = File.Open(vpdPath, FileMode.Create))
{
    using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
    {
        vpdDiskData.Write(writer);
    }
}