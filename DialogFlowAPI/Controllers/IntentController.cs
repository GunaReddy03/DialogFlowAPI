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
        [HttpPost("Create-Intent")]
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
                        Text = part.Phrase
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
        //Get Training Phrase from Intent
        [HttpGet("GetAllTrainingPhrases/{agentId}/{intentId}")]
        public async Task<IActionResult> GetAllTrainingPhrases(string agentId, string intentId)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary

                // Create the intent resource name
                IntentName intentName = new IntentName(projectId, location, agentId, intentId);

                // Get the current intent
                var getIntentRequest = new GetIntentRequest
                {
                    IntentName = intentName
                };

                var intent = await _intentsClient.GetIntentAsync(getIntentRequest);

                // Extract training phrases
                var trainingPhrases = intent.TrainingPhrases.Select(tp => new
                {
                    PhraseId = tp.Parts, // Assuming each training phrase has a unique ID
                    Parts = tp.Parts.Select(p => p.Text).ToList(),
                    RepeatCount = tp.RepeatCount
                }).ToList();

                // Return the list of training phrases
                return Ok(new { trainingPhrases });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error retrieving training phrases", error = ex.Message });
            }
        }

        // Add a Training Phrase to intent

        [HttpPost("AddTrainingPhrase/{agentId}/{intentId}")]
        public async Task<IActionResult> AddTrainingPhrase(string agentId, string intentId, [FromBody] PartDto newTrainingPhraseDto)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary

                // Create the intent resource name
                IntentName intentName = new IntentName(projectId, location, agentId, intentId);

                // Get the current intent
                var getIntentRequest = new GetIntentRequest
                {
                    IntentName = intentName
                };

                var intent = await _intentsClient.GetIntentAsync(getIntentRequest);

                // Add new training phrases
                intent.TrainingPhrases.Add(CreateTrainingPhrase(newTrainingPhraseDto.Phrase));

                // Create the request to update the intent with the new training phrases
                var updateIntentRequest = new UpdateIntentRequest
                {
                    Intent = intent
                };

                // Update the intent in Dialogflow CX
                var updatedIntent = await _intentsClient.UpdateIntentAsync(updateIntentRequest);

                // Return success response
                return Ok(new { message = "Training phrase added successfully", intentId = updatedIntent.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error adding training phrase", error = ex.Message });
            }
        }
        // Method to create a training phrase object from the DTO
        private Intent.Types.TrainingPhrase CreateTrainingPhrase(string phrase)
        {
            return new Intent.Types.TrainingPhrase
            {
                Parts = { new Intent.Types.TrainingPhrase.Types.Part { Text = phrase } },
                RepeatCount = 1
            };
        }
        // Delete a particular Training Phase 
        [HttpDelete("DeleteTrainingPhrase/{agentId}/{intentId}/{phraseIndex}")]
        public async Task<IActionResult> DeleteTrainingPhrase(string agentId, string intentId, int phraseIndex)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary

                // Create the intent resource name
                IntentName intentName = new IntentName(projectId, location, agentId, intentId);

                // Get the current intent
                var getIntentRequest = new GetIntentRequest
                {
                    IntentName = intentName
                };

                var intent = await _intentsClient.GetIntentAsync(getIntentRequest);

                // Check if the index is valid
                if (phraseIndex < 0 || phraseIndex >= intent.TrainingPhrases.Count)
                {
                    return BadRequest(new { message = "Invalid training phrase index" });
                }

                // Remove the specified training phrase by index
                intent.TrainingPhrases.RemoveAt(phraseIndex);

                // Create the request to update the intent
                var updateIntentRequest = new UpdateIntentRequest
                {
                    Intent = intent
                };

                // Update the intent in Dialogflow CX
                var updatedIntent = await _intentsClient.UpdateIntentAsync(updateIntentRequest);

                // Return success response
                return Ok(new { message = "Training phrase deleted successfully", intentId = updatedIntent.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error deleting training phrase", error = ex.Message });
            }
        }
        // Delete all Training Phrase in intent 
        [HttpDelete("DeleteAllTrainingPhrases/{agentId}/{intentId}")]
        public async Task<IActionResult> DeleteAllTrainingPhrases(string agentId, string intentId)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary

                // Create the intent resource name
                IntentName intentName = new IntentName(projectId, location, agentId, intentId);

                // Get the current intent
                var getIntentRequest = new GetIntentRequest
                {
                    IntentName = intentName
                };

                var intent = await _intentsClient.GetIntentAsync(getIntentRequest);

                // Clear all training phrases
                intent.TrainingPhrases.Clear();

                // Create the request to update the intent
                var updateIntentRequest = new UpdateIntentRequest
                {
                    Intent = intent
                };

                // Update the intent in Dialogflow CX
                var updatedIntent = await _intentsClient.UpdateIntentAsync(updateIntentRequest);

                // Return success response
                return Ok(new { message = "All training phrases deleted successfully", intentId = updatedIntent.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error deleting all training phrases", error = ex.Message });
            }
        }



    }
}
