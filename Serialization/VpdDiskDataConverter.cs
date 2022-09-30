using System.Text.Json;
using System.Text.Json.Serialization;
using TFVpdSharp.Models;

namespace TFVpdSharp.Serialization
{
// .json converter compatible with format specified by https://gist.github.com/Ne3tCode/b3e6f7f6e399045843239c30a9c36a15.
    public class LegacyVpdDiskDataConverter : JsonConverter<VpdDiskData>
    {
        #region Constructors

        public LegacyVpdDiskDataConverter()
        {
        }

        public LegacyVpdDiskDataConverter(JsonSerializerOptions options)
        {
        }

        #endregion

        public override VpdDiskData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            VpdDiskData diskData = new VpdDiskData();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return diskData;

                // Read message type enum.
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    if (Enum.TryParse(reader.GetString(), out ProtoDefTypes type))
                    {
                        switch (type)
                        {
                            case (ProtoDefTypes.DEF_TYPE_QUEST_MAP_NODE):
                                diskData.QuestMapNodeDefs =
                                    JsonSerializer.Deserialize<List<CMsgQuestMapNodeDef>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_QUEST_THEME):
                                diskData.QuestThemes =
                                    JsonSerializer.Deserialize<List<CMsgQuestTheme>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_QUEST_MAP_REGION):
                                diskData.QuestMapRegionDefs =
                                    JsonSerializer.Deserialize<List<CMsgQuestMapRegionDef>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_QUEST):
                                diskData.QuestDefs =
                                    JsonSerializer.Deserialize<List<CMsgQuestDef>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_QUEST_OBJECTIVE):
                                diskData.QuestObjectiveDefs =
                                    JsonSerializer.Deserialize<List<CMsgQuestObjectiveDef>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_PAINTKIT_VARIABLES):
                                diskData.PaintKitVariables =
                                    JsonSerializer.Deserialize<List<CMsgPaintKit_Variables>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_PAINTKIT_OPERATION):
                                diskData.PaintKitOperations =
                                    JsonSerializer.Deserialize<List<CMsgPaintKit_Operation>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_PAINTKIT_ITEM_DEFINITION):
                                diskData.PaintKitItemDefinitions =
                                    JsonSerializer.Deserialize<List<CMsgPaintKit_ItemDefinition>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_PAINTKIT_DEFINITION):
                                diskData.PaintKitDefinitions =
                                    JsonSerializer.Deserialize<List<CMsgPaintKit_Definition>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_HEADER_ONLY):
                                diskData.HeaderOnlies =
                                    JsonSerializer.Deserialize<List<CMsgHeaderOnly>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_QUEST_MAP_STORE_ITEM):
                                diskData.QuestMapStoreItems =
                                    JsonSerializer.Deserialize<List<CMsgQuestMapStoreItem>>(ref reader, options)!;
                                break;
                            case (ProtoDefTypes.DEF_TYPE_QUEST_MAP_STAR_TYPE):
                                diskData.QuestMapStarTypes =
                                    JsonSerializer.Deserialize<List<CMsgQuestMapStarType>>(ref reader, options)!;
                                break;
                        }
                    }
                }
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, VpdDiskData value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}