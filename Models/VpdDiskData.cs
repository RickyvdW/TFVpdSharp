namespace TFVpdSharp.Models;

// Data class compatible with format specified by https://gist.github.com/Ne3tCode/b3e6f7f6e399045843239c30a9c36a15.
public partial class VpdDiskData
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
}