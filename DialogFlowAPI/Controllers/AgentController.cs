using Google.Api.Gax.ResourceNames;
using Google.Cloud.Dialogflow.Cx.V3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DialogFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly AgentsClient _agentsClient;
        public AgentController()
        {
            _agentsClient = AgentsClient.Create();
        }
        [HttpGet("Get-Agents")]
        public async Task<IActionResult> GetAgents()
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global";  // Usually 'global' for Dialogflow CX
                LocationName parent = new LocationName(projectId, location);

                var request = new ListAgentsRequest
                {
                    ParentAsLocationName = parent
                };

                var agentsList = new List<Agent>();

                // Fetch the agents from Dialogflow CX
                await foreach (var agent in _agentsClient.ListAgentsAsync(request))
                {
                    agentsList.Add(agent);
                }

                // Return the list of agents
                return Ok(agentsList);
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error retrieving agents", error = ex.Message });
            }
        }
    }
}
