using System.Text.Json;
using System.Text.Json.Serialization;
using ProtoBuf;
using TFVpdSharp.Models;

namespace TFVpdSharp.Serialization;

public static class VpdReaderWriter
{
    private const int MaxDepth = 128;

    public static VpdDiskData? Read(string filePath)
    {
        using FileStream fileStream = File.OpenRead(filePath);
        return Read(fileStream);
    }

    private static VpdDiskData? Read(Stream stream)
    {
        return JsonSerializer.Deserialize<VpdDiskData>(stream,
            new JsonSerializerOptions
            {
                MaxDepth = MaxDepth,
                Converters = { new JsonStringEnumConverter(), new TFVpdSharp.Serialization.LegacyVpdDiskDataConverter() }
            }
        );
    }
        
    private static void Write<T>(ProtoDefTypes type, List<T>? list, BinaryWriter writer)
    {
        // Convert enum to unsigned integer according to the specification.
        writer.Write((uint)type);
            
        // Just make sure there's even a list otherwise just write 0 elements.
        if (list == null)
        {
            writer.Write((uint)0);
            return;
        }

        // Write the number of elements into the stream according to the specification.
        writer.Write((uint)list.Count);
            
        // Write the list.
        foreach (var u in list)
        {
            // 
            var p = writer.BaseStream.Position;
                
            // Write a placeholder value for the number of bytes this message is.
            uint s = 0;
            writer.Write(s);
                
            // Use protobuf serializer to push bytes to the stream.
            Serializer.Serialize(writer.BaseStream, u);
                
            // Calculate the number of bytes in the message.
            s = (uint)(writer.BaseStream.Position - p);

            // Now we know how large the serialized message is, go back and overwrite the previous value we wrote. (0)
            writer.BaseStream.Seek(p, SeekOrigin.Begin);
                
            // Adjust for the size variable itself.  
            s -= sizeof(uint);
            writer.Write(s);
                
            // Go back to where we ended before overwriting the size.
            writer.BaseStream.Seek(s, SeekOrigin.Current);
        }
    }
        
    public static void Write(BinaryWriter writer, VpdDiskData? vpdDiskData)
    {
        if (vpdDiskData == null)
            throw new Exception("Vpd disk data class is null.");
        
        Write(ProtoDefTypes.DEF_TYPE_QUEST_MAP_NODE, vpdDiskData.QuestMapNodeDefs, writer);
        Write(ProtoDefTypes.DEF_TYPE_QUEST_THEME, vpdDiskData.QuestThemes, writer);
        Write(ProtoDefTypes.DEF_TYPE_QUEST_MAP_REGION, vpdDiskData.QuestMapRegionDefs, writer);
        Write(ProtoDefTypes.DEF_TYPE_QUEST, vpdDiskData.QuestDefs, writer);
        Write(ProtoDefTypes.DEF_TYPE_QUEST_OBJECTIVE, vpdDiskData.QuestObjectiveDefs, writer);
        Write(ProtoDefTypes.DEF_TYPE_PAINTKIT_VARIABLES, vpdDiskData.PaintKitVariables, writer);
        Write(ProtoDefTypes.DEF_TYPE_PAINTKIT_OPERATION, vpdDiskData.PaintKitOperations, writer);
        Write(ProtoDefTypes.DEF_TYPE_PAINTKIT_ITEM_DEFINITION, vpdDiskData.PaintKitItemDefinitions, writer);
        Write(ProtoDefTypes.DEF_TYPE_PAINTKIT_DEFINITION, vpdDiskData.PaintKitDefinitions, writer);
        Write(ProtoDefTypes.DEF_TYPE_HEADER_ONLY, vpdDiskData.HeaderOnlies, writer);
        Write(ProtoDefTypes.DEF_TYPE_QUEST_MAP_STORE_ITEM, vpdDiskData.QuestMapStoreItems, writer);
        Write(ProtoDefTypes.DEF_TYPE_QUEST_MAP_STAR_TYPE, vpdDiskData.QuestMapStarTypes, writer);
    }
}