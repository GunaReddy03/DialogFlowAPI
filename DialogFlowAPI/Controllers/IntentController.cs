using DialogFlowAPI.ViewModel;
using Google.Api.Gax;
using Google.Cloud.Dialogflow.Cx.V3;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DialogFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntentController : ControllerBase
    {
        private readonly IntentsClient _intentsClient;
        public IntentController(IntentsClient intentsClient)
        {
            _intentsClient = intentsClient;
        }
        [HttpGet("Get-Intents")]
        public async Task<IActionResult> GetIntents(string agentId)
        {
            try
            {
                // Replace with your actual project ID, agent ID, and location
                string projectId = "default-yrln";
                string location = "global"; // Usually 'global' for Dialogflow CX

                // Create the parent resource name (agent path)
                AgentName parent = new AgentName(projectId, location, agentId);

                // Request object for listing intents
                var request = new ListIntentsRequest
                {
                    ParentAsAgentName = parent
                };

                var intentsList = new List<Intent>();

                // Fetch the intents using PagedAsyncEnumerable
                PagedAsyncEnumerable<ListIntentsResponse, Intent> response = _intentsClient.ListIntentsAsync(request);

                // Asynchronously iterate through the intents and add them to the list
                await foreach (var intent in response)
                {
                    intentsList.Add(intent);
                }

                // Return the list of intents as a response
                return Ok(intentsList);
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error retrieving intents", error = ex.Message });
            }
        }
        [HttpGet("{Get-By-intentId}")]
        public async Task<IActionResult> GetIntentById(string agentId,string intentId)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary
                // = "your-agent-id";

                // Create the resource name for the intent
                IntentName intentName = new IntentName(projectId, location, agentId, intentId);

                // Fetch the intent
                var intent = await _intentsClient.GetIntentAsync(intentName);

                // Return the intent details
                return Ok(intent);
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error retrieving intent", error = ex.Message });
            }
        }
        [HttpPost("Create-Agent")]
        public async Task<IActionResult> CreateIntent(string agentId, [FromBody] NewIntentDto newIntentDto)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary
                //= "your-agent-id";

                // Create parent resource name (AgentName)
                AgentName parent = new AgentName(projectId, location, agentId);

                // Prepare the intent
                var intent = new Intent
                {
                    DisplayName = newIntentDto.DisplayName,
                    TrainingPhrases = { CreateTrainingPhrases(newIntentDto.TrainingPhrases) }
                };

                // Create the intent
                var request = new CreateIntentRequest
                {
                    ParentAsAgentName = parent,
                    Intent = intent
                };

                var createdIntent = await _intentsClient.CreateIntentAsync(request);

                // Return success response
                return Ok(new { message = "Intent created successfully", intentId = createdIntent.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error creating intent", error = ex.Message });
            }
        }

        // Helper method to create training phrases
        private static IEnumerable<Intent.Types.TrainingPhrase> CreateTrainingPhrases(List<TrainingPhraseDto> trainingPhrases)
        {
            foreach (var phrase in trainingPhrases)
            {
                var parts = new List<Intent.Types.TrainingPhrase.Types.Part>();
                foreach (var part in phrase.Parts)
                {
                    parts.Add(new Intent.Types.TrainingPhrase.Types.Part
                    {
                        Text = part.Text
                    });
                }

                yield return new Intent.Types.TrainingPhrase
                {
                    Parts = { parts },
                    RepeatCount = 1
                };
            }
        }
        [HttpPut("update/{intentId}")]
        public async Task<IActionResult> UpdateIntent(string agentId,string intentId, [FromBody] UpdateIntentDto updateIntentDto)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global";// Adjust if necessary
               // = "your-agent-id";

                // Create the resource name for the intent
                IntentName intentName = new IntentName(projectId, location, agentId, intentId);

                // Prepare the intent update
                var intent = new Intent
                {
                    Name = intentName.ToString(),
                    DisplayName = updateIntentDto.DisplayName,
                    TrainingPhrases = { CreateTrainingPhrases(updateIntentDto.TrainingPhrases) }
                };

                // Create the request for updating the intent
                var request = new UpdateIntentRequest
                {
                    Intent = intent,
                    UpdateMask = new FieldMask
                    {
                        Paths = { "display_name", "training_phrases" }
                    }
                };

                // Update the intent
                var updatedIntent = await _intentsClient.UpdateIntentAsync(request);

                // Return success response
                return Ok(new { message = "Intent updated successfully", intentId = updatedIntent.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error updating intent", error = ex.Message });
            }
        }
        // Helper method to create training phrases
        // Json Input
        //{
        //  "displayName": "TurnOffAlarmIntent",
        //  "trainingPhrases": [
        //    {
        //      "parts": [
        //        { "text": "Turn off the alarm" }
        //      ]
        //    },
        //    {
        //    "parts": [
        //        { "text": "Please turn off the alarm" }
        //      ]
        //    }
        //  ]
        //}
        [HttpDelete("{intentId}")]
        public async Task<IActionResult> DeleteIntent(string agentId,string intentId)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary
                                            //= "your-agent-id";

                // Create the resource name for the intent
                IntentName intentName = new IntentName(projectId, location, agentId, intentId);

                // Delete the intent
                await _intentsClient.DeleteIntentAsync(intentName);

                // Return success response
                return Ok(new { message = "Intent deleted successfully", intentId });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error deleting intent", error = ex.Message });
            }
        }




    }
}
