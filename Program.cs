using System.Text;
using TFVpdSharp.Serialization;
using VpdDiskData = TFVpdSharp.Models.VpdDiskData;

const string generatedVpdExtension = ".generated.vpd";

#region Console application arguments processing.
// Format: TFVpdSharp.exe <path>
if (args.Length < 1)
{
    Console.WriteLine("Missing input file argument.");
    return;
}

// Double check if the file even exists before reading it, otherwise exit immediately.
string filePath = args[0];
if (!File.Exists(filePath))
{
    Console.WriteLine($"Could not find input file : {filePath}.");
    return;
}
#endregion

// If your file isn't valid JSON, the JSON parser will throw an exception.
VpdDiskData? vpdDiskData = null;

// Read from JSON file into memory.
try
{
    vpdDiskData = VpdReaderWriter.Read(filePath);
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine("Conversion failed. Press any key to continue.");
    Console.ReadKey();
    return;
}

// Write from memory to binary VPD file.
try
{
    string vpdPath = Path.ChangeExtension(filePath, generatedVpdExtension);
    using var stream = File.Open(vpdPath, FileMode.Create);
    using var writer = new BinaryWriter(stream, Encoding.UTF8, false);
    VpdReaderWriter.Write(writer, vpdDiskData);
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine("Re-serialization failed. Press any key to continue.");
    Console.ReadKey();
}