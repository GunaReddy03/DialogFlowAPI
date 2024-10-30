using DialogFlowAPI.DbContext;
using DialogFlowAPI.Models;
using DialogFlowAPI.ViewModel;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Dialogflow.Cx.V3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static DialogFlowAPI.Models.Logins;

namespace DialogFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly AgentsClient _agentsClient;
        private readonly DialogFlowDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AgentController(UserManager<ApplicationUser> userManager,DialogFlowDbContext dialogFlowDbContext)
        {
            _agentsClient = AgentsClient.Create();
            _userManager = userManager;
            _context = dialogFlowDbContext;
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
        [HttpGet("GetAvailableLocations")]
        public IActionResult GetAvailableLocations()
        {
            try
            {
                // Static list of available Google Cloud locations for Dialogflow CX
                var locations = new List<object>
        {
            new { LocationId = "global", DisplayName = "Global" },
            new { LocationId = "us-central1", DisplayName = "US Central" },
            new { LocationId = "us-east1", DisplayName = "US East" },
            new { LocationId = "us-west1", DisplayName = "US West" },
            new { LocationId = "europe-west1", DisplayName = "Europe West" },
            new { LocationId = "asia-northeast1", DisplayName = "Asia Northeast" }
            // Add more locations as necessary
        };

                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving locations", error = ex.Message });
            }
        }
        [HttpGet("GetTimeZones")]
        public IActionResult GetTimeZones()
        {
            try
            {
                // Get time zone names
                var timeZones = TimeZoneInfo.GetSystemTimeZones().Select(tz => new
                {
                    Id = tz.Id,
                    DisplayName = tz.DisplayName
                }).ToList();

                return Ok(timeZones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving time zones", error = ex.Message });
            }
        }
        [HttpGet("GetDefaultLanguages")]
        public IActionResult GetDefaultLanguages()
        {
            try
            {
                // Example list of language codes supported by Dialogflow CX
                var languages = new List<object>
        {
            new { LanguageCode = "en", DisplayName = "English" },
            new { LanguageCode = "es", DisplayName = "Spanish" },
            new { LanguageCode = "fr", DisplayName = "French" },
            new { LanguageCode = "de", DisplayName = "German" },
            new { LanguageCode = "ja", DisplayName = "Japanese" }
            // Add more languages as needed
        };

                return Ok(languages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving default languages", error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("CreateAgent")]
        public async Task<IActionResult> CreateAgent([FromBody] AgentDto agentDto)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary

                // Construct the parent resource name for the project location
                LocationName parent = new LocationName(projectId, location);

                // Prepare the agent to be created
                var agent = new Agent
                {
                    DisplayName = agentDto.DisplayName,
                    DefaultLanguageCode = agentDto.DefaultLanguageCode,
                    TimeZone = agentDto.TimeZone,
                    Description = agentDto.Description,
                    AvatarUri = agentDto.AvatarUri
                };

                // Create the agent creation request
                var request = new CreateAgentRequest
                {
                    ParentAsLocationName = parent,
                    Agent = agent
                };

                // Call Dialogflow CX to create the agent
                var response = await _agentsClient.CreateAgentAsync(request);

                // Get the current user ID
                var agentId = response.Name.Split('/').Last();

                // Get the currently logged-in user ID
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

                // Save the UserId and AgentId in AgentUser table
                var agentUser = new AgentUserModel
                {
                    UserId = userId,
                    AgentID = agentId
                };

                _context.AgentUser.Add(agentUser);
                await _context.SaveChangesAsync();

                // Return success response with the new agent ID
                return Ok(new { message = "Agent created successfully", agentId = response.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error creating agent", error = ex.Message });
            }
        }

    }
}
