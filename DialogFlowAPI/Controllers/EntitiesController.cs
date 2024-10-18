using DialogFlowAPI.ViewModel;
using Google.Cloud.Dialogflow.Cx.V3;
using Google.Protobuf.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Google.Cloud.Dialogflow.Cx.V3;
namespace DialogFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntitiesController : ControllerBase
    {
        private readonly EntityTypesClient _entityTypesClient;
        public EntitiesController(EntityTypesClient entityTypesClient) 
        {
            _entityTypesClient = entityTypesClient;
        }
        [HttpGet("Get-Entities")]
        public async Task<IActionResult> GetEntities(string agentId)
        {
            try
            {
                // Replace these with your actual project and agent details
                string projectId = "default-yrln";
                string location = "global"; // The Dialogflow CX agent ID

                // Create the parent resource name (AgentName)
                AgentName parent = new AgentName(projectId, location, agentId);

                // Prepare request to list entity types
                var request = new ListEntityTypesRequest
                {
                    ParentAsAgentName = parent
                };

                // Fetch the entities from Dialogflow CX
                var entityTypes = new List<EntityType>();

                await foreach (var entityType in _entityTypesClient.ListEntityTypesAsync(request))
                {
                    entityTypes.Add(entityType);
                }

                // Transform the result to a simple DTO (Optional, you can directly return the entityTypes if desired)
                var result = entityTypes.Select(et => new EntityTypeResponseDto
                {
                    DisplayName = et.DisplayName,
                    EntityTypeId = et.Name
                }).ToList();

                // Return the list of entities
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error retrieving entities", error = ex.Message });
            }
        }
        [HttpGet("get/{entityTypeId}")]
        public async Task<IActionResult> GetEntityById(string agentId ,string entityTypeId)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary
                //= "your-agent-id";

                // Create the resource name for the entity type
                EntityTypeName entityTypeName = new EntityTypeName(projectId, location, agentId, entityTypeId);

                // Fetch the entity type from Dialogflow CX
                var entityType = await _entityTypesClient.GetEntityTypeAsync(entityTypeName);

                // Return the entity type details
                return Ok(entityType);
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error retrieving entity", error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEntity(string agentId ,[FromBody] CreateEntityDto createEntityDto)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary
                //= "your-agent-id";

                // Define the parent agent name
                AgentName parent = new AgentName(projectId, location, agentId);

                // Create a new EntityType object
                var entityType = new EntityType
                {
                    DisplayName = createEntityDto.DisplayName,
                    Kind = createEntityDto.Kind,
                    Entities = { CreateEntities(createEntityDto.Entities) }
                };

                // Create the request for adding the entity type
                var request = new CreateEntityTypeRequest
                {
                    ParentAsAgentName = parent,
                    EntityType = entityType
                };

                // Create the entity in Dialogflow CX
                var createdEntityType = await _entityTypesClient.CreateEntityTypeAsync(request);

                // Return success response
                return Ok(new { message = "Entity created successfully", entityTypeId = createdEntityType.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error creating entity", error = ex.Message });
            }
        }
        // Json Format for Entity
        //{ 
        //  "displayName": "FruitEntity",
        //  "kind": "MAP",
        //  "entities": [
        //    {
        //      "value": "apple",
        //      "synonyms": ["green apple", "red apple"]
        //    },
        //    {
        //      "value": "banana",
        //      "synonyms": ["yellow banana", "ripe banana"]
        //}
        //  ]
        //}

        // Helper method to create entities
        private static RepeatedField<EntityType.Types.Entity> CreateEntities(List<EntityDto> entities)
        {
            var entityList = new RepeatedField<EntityType.Types.Entity>();
            foreach (var entity in entities)
            {
                entityList.Add(new EntityType.Types.Entity
                {
                    Value = entity.Value,
                    Synonyms = { entity.Synonyms }
                });
            }
            return entityList;
        }
        [HttpPut("update/{entityTypeId}")]
        public async Task<IActionResult> UpdateEntity(string agentId ,string entityTypeId, [FromBody] CreateEntityDto updateEntityDto)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary
                //string agentId = "your-agent-id";

                // Create the resource name for the entity type
                EntityTypeName entityTypeName = new EntityTypeName(projectId, location, agentId, entityTypeId);

                // Create the updated EntityType object
                var updatedEntityType = new EntityType
                {
                    Name = entityTypeName.ToString(),
                    DisplayName = updateEntityDto.DisplayName,
                    Kind = updateEntityDto.Kind,
                    Entities = { CreateEntities(updateEntityDto.Entities) }
                };

                // Create the request for updating the entity type
                var request = new UpdateEntityTypeRequest
                {
                    EntityType = updatedEntityType
                };

                // Update the entity type in Dialogflow CX
                var response = await _entityTypesClient.UpdateEntityTypeAsync(request);

                // Return success response
                return Ok(new { message = "Entity updated successfully", entityTypeId = response.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error updating entity", error = ex.Message });
            }
        }
        [HttpDelete("delete/{entityTypeId}")]
        public async Task<IActionResult> DeleteEntity(string agentId ,string entityTypeId)
        {
            try
            {
                // Replace with your actual project ID, location, and agent ID
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary
                //string agentId = "your-agent-id";

                // Create the resource name for the entity type
                EntityTypeName entityTypeName = new EntityTypeName(projectId, location, agentId, entityTypeId);

                // Delete the entity type from Dialogflow CX
                await _entityTypesClient.DeleteEntityTypeAsync(entityTypeName);

                // Return success response
                return Ok(new { message = "Entity deleted successfully", entityTypeId });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error deleting entity", error = ex.Message });
            }
        }

    }
}
