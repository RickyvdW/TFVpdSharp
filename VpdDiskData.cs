using ProtoBuf;
using TFVpdSharp.Protobuf;

namespace TFVpdSharp;

// Data class compatible with format specified by https://gist.github.com/Ne3tCode/b3e6f7f6e399045843239c30a9c36a15.
public class VpdDiskData
{
    public List<CMsgQuestMapNodeDef>? QuestMapNodeDefs;
    public List<CMsgQuestTheme>? QuestThemes;
    public List<CMsgQuestMapRegionDef>? QuestMapRegionDefs;
    public List<CMsgQuestDef>? QuestDefs;
    public List<CMsgQuestObjectiveDef>? QuestObjectiveDefs;
    public List<CMsgPaintKit_Variables>? PaintKitVariables;
    public List<CMsgPaintKit_Operation>? PaintKitOperations;
    public List<CMsgPaintKit_ItemDefinition>? PaintKitItemDefinitions;
    public List<CMsgPaintKit_Definition>? PaintKitDefinitions;
    public List<CMsgHeaderOnly>? HeaderOnlies;
    public List<CMsgQuestMapStoreItem>? QuestMapStoreItems;
    public List<CMsgQuestMapStarType>? QuestMapStarTypes;
    
    private void WriteGeneric<T>(ProtoDefTypes type, List<T>? list, BinaryWriter writer)
    {
        writer.Write((uint)type);
        if (list != null)
        {
            writer.Write((uint)QuestMapNodeDefs.Count);
            foreach (T u in list)
            {
                var position = writer.BaseStream.Position;
                uint size = 0;
                writer.Write(size);
                Serializer.Serialize(writer.BaseStream, u);
                size = (uint)(writer.BaseStream.Position - position);

                // Now we know how large the serialized message is, write 
                writer.BaseStream.Seek(position, SeekOrigin.Begin);
                writer.Write(size);
                writer.BaseStream.Seek(size, SeekOrigin.Current);
            }
        }
        else
        {
            writer.Write((uint)0);
        }
    }
    
    public void Write(BinaryWriter writer)
    {
        WriteGeneric(ProtoDefTypes.DEF_TYPE_QUEST_MAP_NODE, QuestMapNodeDefs, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_QUEST_THEME, QuestThemes, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_QUEST_MAP_REGION, QuestMapRegionDefs, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_QUEST, QuestDefs, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_QUEST_OBJECTIVE, QuestObjectiveDefs, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_PAINTKIT_VARIABLES, PaintKitVariables, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_PAINTKIT_OPERATION, PaintKitOperations, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_PAINTKIT_ITEM_DEFINITION, PaintKitItemDefinitions, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_PAINTKIT_DEFINITION, PaintKitDefinitions, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_HEADER_ONLY, HeaderOnlies, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_QUEST_MAP_STORE_ITEM, QuestMapStoreItems, writer);
        WriteGeneric(ProtoDefTypes.DEF_TYPE_QUEST_MAP_STAR_TYPE, QuestMapStarTypes, writer);
    }
}