using DialogFlowAPI.ViewModel;
using Google.Cloud.Dialogflow.Cx.V3;
using Google.Protobuf.Collections;
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
        [HttpPost("create-agent")]
        public async Task<IActionResult> CreateSimpleFlow(string agentId,[FromBody] SimpleFlowDto flowDto)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global"; // or your specific location
                // = "your-agent-id";

                // Construct the agent's parent resource name
                AgentName parent = new AgentName(projectId, location, agentId);

                // Create the new flow object with minimal required fields
                Flow newFlow = new Flow
                {
                    DisplayName = flowDto.DisplayName,
                    Description = flowDto.Description
                };

                // Create the request to create the flow
                var request = new CreateFlowRequest
                {
                    ParentAsAgentName = parent,
                    Flow = newFlow
                };

                // Call Dialogflow CX to create the flow
                var response = await _flowsClient.CreateFlowAsync(request);

                // Return success response with the newly created flow's ID
                return Ok(new { message = "Flow created successfully", flowId = response.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error creating flow", error = ex.Message });
            }
        }
        [HttpDelete("delete/{flowId}")]
        public async Task<IActionResult> DeleteFlow(string agentId,string flowId)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary
                // = "your-agent-id";

                // Construct the resource name for the flow
                FlowName flowName = new FlowName(projectId, location, agentId, flowId);

                // Create the request to delete the flow
                var request = new DeleteFlowRequest
                {
                    FlowName = flowName
                };

                // Call the Dialogflow CX API to delete the flow
                await _flowsClient.DeleteFlowAsync(request);

                // Return success response
                return Ok(new { message = "Flow deleted successfully", flowId });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error deleting flow", error = ex.Message });
            }
        }
        private static RepeatedField<TransitionRoute> CreateTransitionRoutes(List<TransitionRouteDto> transitionRoutes)
        {
            var routes = new RepeatedField<TransitionRoute>();
            foreach (var route in transitionRoutes)
            {
                routes.Add(new TransitionRoute
                {
                    Intent = route.Intent,
                    TargetFlow = route.TargetFlow,
                    TargetPage = route.TargetPage
                });
            }
            return routes;
        }


    }
}
