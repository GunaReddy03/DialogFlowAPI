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
        [HttpPost("GetAgentResponse")]
        public async Task<IActionResult> GetAgentResponse(string agentId, string sessionId, string flowId, string userQuery)
        {
            try
            {
                string projectId = "default-yrln";
                string location = "global";
                var sessionName = SessionName.FromProjectLocationAgentSession(projectId, location, agentId, sessionId);

                // Create the detect intent request
                var detectIntentRequest = new DetectIntentRequest
                {
                    SessionAsSessionName = sessionName,
                    QueryInput = new QueryInput
                    {
                        Text = new TextInput
                        {
                            Text = userQuery
                        },
                        LanguageCode = "en-US"
                    }
                };

                // Call the Dialogflow API to detect the intent
                var response = await _sessionsClient.DetectIntentAsync(detectIntentRequest);

                // Check for a valid response
                if (response?.QueryResult == null)
                {
                    return StatusCode(500, new { message = "No response from Dialogflow", error = "QueryResult is null" });
                }

                // Extract the response text
                var fulfillmentText = response.QueryResult.ResponseMessages
                    .Where(m => m.MessageCase == ResponseMessage.MessageOneofCase.Text)
                    .Select(m => m.Text.Text_.FirstOrDefault())
                    .FirstOrDefault() ?? "No fulfillment text found";

                // Return the response details
                return Ok(new
                {
                    message = "Agent response received",
                    fulfillmentText,
                    intent = response.QueryResult.Intent?.DisplayName ?? "No intent detected",
                    languageCode = response.QueryResult.LanguageCode ?? "en-US",
                    flowId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving agent response", error = ex.Message });
            }
        }
        //[HttpPost("SendMessage")]
        //public async Task<IActionResult> SendMessage(string agentId, string sessionId, string userquery)
        //{
        //    try
        //    {

        //        string projectId = "default-yrln"; // Update with your actual project ID
        //        string location = "global"; // Adjust as necessary
        //        var sessionName = SessionName.FromProjectLocationAgentSession(projectId, location, agentId, sessionId);

        //        var detectIntentRequest = new DetectIntentRequest
        //        {
        //            SessionAsSessionName = sessionName,
        //            QueryInput = new QueryInput
        //            {
        //                Text = new TextInput
        //                {
        //                    Text = userquery,
        //                     // Specify the language code
        //                },
        //                LanguageCode = "en"
        //            }
        //        };

        //        var response = await _sessionsClient.DetectIntentAsync(detectIntentRequest);
        //        return Ok(new { message = "Agent response", response = response.QueryResult.Text });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Error sending message", error = ex.Message });
        //    }
        //}
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


