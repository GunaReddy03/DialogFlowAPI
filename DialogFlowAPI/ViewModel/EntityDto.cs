using Google.Cloud.Dialogflow.Cx.V3;

namespace DialogFlowAPI.ViewModel
{
    public class CreateEntityDto
    {
        public string? DisplayName { get; set; }
        public EntityType.Types.Kind Kind { get; set; } // Enum: MAP, LIST, KIND_UNSPECIFIED
        public List<EntityDto>? Entities { get; set; }
    }

    public class EntityDto
    {
        public string? Value { get; set; }
        public List<string>? Synonyms { get; set; }
    }

}
