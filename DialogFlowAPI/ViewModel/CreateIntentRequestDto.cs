using Google.Cloud.Dialogflow.Cx.V3;

namespace DialogFlowAPI.ViewModel
{
    public class NewIntentDto
    {
        public string? DisplayName { get; set; }
        public List<TrainingPhraseDto>? TrainingPhrases { get; set; }
    }

    public class TrainingPhraseDto
    {
        public List<PartDto>? Parts { get; set; }
    }

    public class PartDto
    {
        public string? Text { get; set; }
    }
    public class UpdateIntentDto
    {
        public string? DisplayName { get; set; }
        public List<TrainingPhraseDto>? TrainingPhrases { get; set; }
    }


    public class EntityTypeResponseDto
    {
        public string? DisplayName { get; set; }
        public string? EntityTypeId { get; set; }
    }


}
