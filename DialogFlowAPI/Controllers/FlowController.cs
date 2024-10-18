using Google.Cloud.Dialogflow.Cx.V3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DialogFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowController : ControllerBase
    {
        private readonly FlowsClient _flowsClient;

        public FlowController(FlowsClient flowsClient)
        {
            _flowsClient = flowsClient;
        }

        // GET: /flow/list
        [HttpGet("Flow List")]
        public async Task<IActionResult> GetFlows(string agentId)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary
                // = "your-agent-id";

                // Create the parent agent resource name
                AgentName parent = new AgentName(projectId, location, agentId);

                // Create a request to list flows
                var request = new ListFlowsRequest
                {
                    ParentAsAgentName = parent
                };

                // Create a list to hold the flows
                var flowsList = new List<Flow>();

                // Fetch the flows from Dialogflow CX
                var flows = _flowsClient.ListFlowsAsync(request);

                await foreach (var flow in flows)
                {
                    flowsList.Add(flow);
                }

                // Return the list of flows
                return Ok(flowsList);
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error retrieving flows", error = ex.Message });
            }
        }
    }
}
