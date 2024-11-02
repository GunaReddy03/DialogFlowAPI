using DialogFlowAPI.ViewModel;
using Google.Cloud.Dialogflow.Cx.V3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DialogFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly SessionsClient _sessionsClient;
        public SessionController()
        {
            _sessionsClient = SessionsClient.Create();
        }
        [HttpPost("StartSession")]
        public async Task<IActionResult> StartSession(string agentId, string sessionId, string userquery)
        {
            try
            {
                string projectId = "default-yrln"; // Update with your actual project ID
                string location = "global"; // Adjust as necessary
                var sessionName = SessionName.FromProjectLocationAgentSession(projectId, location, agentId, sessionId);

                var detectIntentRequest = new DetectIntentRequest
                {
                    SessionAsSessionName = sessionName,
                    QueryInput = new QueryInput
                    {
                        Text = new TextInput
                        {
                            Text = "Hello", // Initial message
                            //LanguageCode = "en" // Specify the language code
                        },
                        LanguageCode = "en"
                    }
                };

                var response = await _sessionsClient.DetectIntentAsync(detectIntentRequest);
                return Ok(new { message = "Session started", response = response.QueryResult.Text });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error starting session", error = ex.Message });
            }
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(string agentId, string sessionId, string userquery)
        {
            try
            {
               
                string projectId = "default-yrln"; // Update with your actual project ID
                string location = "global"; // Adjust as necessary
                var sessionName = SessionName.FromProjectLocationAgentSession(projectId, location, agentId, sessionId);

                var detectIntentRequest = new DetectIntentRequest
                {
                    SessionAsSessionName = sessionName,
                    QueryInput = new QueryInput
                    {
                        Text = new TextInput
                        {
                            Text = userquery,
                             // Specify the language code
                        },
                        LanguageCode = "en"
                    }
                };

                var response = await _sessionsClient.DetectIntentAsync(detectIntentRequest);
                return Ok(new { message = "Agent response", response = response.QueryResult.Text });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error sending message", error = ex.Message });
            }
        }
        // Define DTOs for request bodies
        public class StartSessionDto
        {
            public string AgentId { get; set; }
            public string SessionId { get; set; }
            public string FlowId { get; set; }
        }

        public class SendMessageDto
        {
            public string AgentId { get; set; }
            public string SessionId { get; set; }
            public string FlowId { get; set; }
            public string PageId { get; set; }
            public string UserQuery { get; set; }
        }
        [HttpPost("GetAgentResponse")]
        public async Task<IActionResult> GetAgentResponse(string agentId, string sessionId, string flowId, string userQuery)
        {
            try
            {
                // Set up your project details
                string projectId = "default-yrln"; // Your actual project ID
                string location = "global"; // Usually 'global' for Dialogflow CX

                // Create the session name
                var sessionName = SessionName.FromProjectLocationAgentSession(projectId, location, agentId, sessionId);

                // Create the detect intent request
                var detectIntentRequest = new DetectIntentRequest
                {
                    SessionAsSessionName = sessionName,
                    QueryInput = new QueryInput
                    {
                        Text = new TextInput
                        {
                            Text = userQuery, // User's message
                             // Set the appropriate language code
                        },
                        LanguageCode = "en-US"
                    }
                };

                // Call the Dialogflow API to detect the intent
                var response = await _sessionsClient.DetectIntentAsync(detectIntentRequest);

                // Extract the relevant details from the response
                var fulfillmentText = response.QueryResult.ResponseMessages;
                     // Get the first text or empty if none

                // Return the response from the agent
                return Ok(new
                {
                    message = "Agent response received",
                    fulfillmentText = fulfillmentText,
                    intent = response.QueryResult.Intent.DisplayName, // The detected intent name
                    languageCode = response.QueryResult.LanguageCode // The language code from the response
                });
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the process
                return StatusCode(500, new { message = "Error retrieving agent response", error = ex.Message });
            }
        }

        // Helper method to extract messages from agent response
        private List<string> ExtractBotResponse1(DetectIntentResponse response)
        {
            var messages = new List<string>();

            foreach (var message in response.QueryResult.ResponseMessages)
            {
                if (message.Text != null)
                {
                    messages.AddRange(message.Text.Text_);
                }
            }

            return messages;
        }

    }
}


