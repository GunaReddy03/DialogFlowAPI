using DialogFlowAPI.ViewModel;
using Google.Apis.Discovery;
using Google.Cloud.Dialogflow.Cx.V3;
using Microsoft.AspNetCore.Mvc;
using static Google.Cloud.Dialogflow.Cx.V3.Intents;
using Google.Protobuf.WellKnownTypes; // For UpdatePageRequest, if required


namespace DialogFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly PagesClient _pagesClient;
        private readonly FlowsClient _flowsClient;
        private readonly Google.Cloud.Dialogflow.Cx.V3.IntentsClient _intentsClient;

        public RoutesController(PagesClient pagesClient,FlowsClient flowsClient,Google.Cloud.Dialogflow.Cx.V3.IntentsClient intentsClient)
        {
            _pagesClient = pagesClient;
            _flowsClient = flowsClient;
            _intentsClient = intentsClient;
        }
        [HttpPost("createTransitionRoute")]
        public async Task<IActionResult> CreateTransitionRoute(string AgentId, string FlowId, [FromBody] TransitionRouteDto2 transitionRouteDto)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // or your specific location

                // Construct the parent flow resource name from the request body
                FlowName parentFlow = new FlowName(projectId, location, AgentId, FlowId);

                // Create the transition route object
                var transitionRoute = new TransitionRoute
                {
                    Intent = transitionRouteDto.IntentId,
                    Condition = transitionRouteDto.Condition,
                    TriggerFulfillment = new Fulfillment
                    {
                        Messages =
                        {
                      new ResponseMessage
                    {
                        Text = new ResponseMessage.Types.Text
                        {
                            Text_ = { transitionRouteDto.FulfillmentMessage }
                        }
                    }
                        }
                    }
                };

                // Check if TargetPage is provided, otherwise leave it unset
                if (!string.IsNullOrEmpty(transitionRouteDto.TargetPage))
                {
                    transitionRoute.TargetPage = transitionRouteDto.TargetPage;  // Set TargetPage if provided
                }

                // Check if TargetFlow is provided, otherwise leave it unset
                if (!string.IsNullOrEmpty(transitionRouteDto.TargetFlow))
                {
                    transitionRoute.TargetFlow = transitionRouteDto.TargetFlow;  // Set TargetFlow if provided
                }

                // Check if neither TargetPage nor TargetFlow is provided
                if (string.IsNullOrEmpty(transitionRouteDto.TargetPage) && string.IsNullOrEmpty(transitionRouteDto.TargetFlow))
                {
                    return BadRequest(new { message = "Either TargetPage or TargetFlow must be provided." });
                }

                // Get the flow where you want to add the transition route
                var getFlowRequest = new GetFlowRequest
                {
                    FlowName = parentFlow
                };

                // Fetch the current flow
                var flow = await _flowsClient.GetFlowAsync(getFlowRequest);

                // Add the new transition route to the existing flow
                flow.TransitionRoutes.Add(transitionRoute);

                // Update the flow with the new transition route
                var updateFlowRequest = new UpdateFlowRequest
                {
                    Flow = flow
                };

                var response = await _flowsClient.UpdateFlowAsync(updateFlowRequest);

                // Return success response with the updated flow ID
                return Ok(new { message = "Transition route created successfully", flowId = response.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error creating transition route", error = ex.Message });
            }
        }
        [HttpPut("updateTransitionRoute")]
        public async Task<IActionResult> UpdateTransitionRoute(string AgentId, string FlowId, string TransitionRouteId, [FromBody] TransitionRouteDto2 transitionRouteDto)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // or your specific location

                // Construct the flow resource name
                FlowName parentFlow = new FlowName(projectId, location, AgentId, FlowId);

                // Fetch the current flow containing the transition route
                var getFlowRequest = new GetFlowRequest
                {
                    FlowName = parentFlow
                };
                var flow = await _flowsClient.GetFlowAsync(getFlowRequest);

                // Find the transition route to be updated
                var transitionRoute = flow.TransitionRoutes.FirstOrDefault(tr => tr.Name.EndsWith(TransitionRouteId));
                if (transitionRoute == null)
                {
                    return NotFound(new { message = "Transition route not found." });
                }

                // Update transition route fields if provided
                if (!string.IsNullOrEmpty(transitionRouteDto.IntentId))
                    transitionRoute.Intent = transitionRouteDto.IntentId;

                if (!string.IsNullOrEmpty(transitionRouteDto.Condition))
                    transitionRoute.Condition = transitionRouteDto.Condition;

                if (!string.IsNullOrEmpty(transitionRouteDto.FulfillmentMessage))
                {
                    transitionRoute.TriggerFulfillment = new Fulfillment
                    {
                        Messages =
                {
                    new ResponseMessage
                    {
                        Text = new ResponseMessage.Types.Text
                        {
                            Text_ = { transitionRouteDto.FulfillmentMessage }
                        }
                    }
                }
                    };
                }

                if (!string.IsNullOrEmpty(transitionRouteDto.TargetPage))
                    transitionRoute.TargetPage = transitionRouteDto.TargetPage;

                if (!string.IsNullOrEmpty(transitionRouteDto.TargetFlow))
                    transitionRoute.TargetFlow = transitionRouteDto.TargetFlow;

                // Update the flow with modified transition route
                var updateFlowRequest = new UpdateFlowRequest
                {
                    Flow = flow
                };
                var response = await _flowsClient.UpdateFlowAsync(updateFlowRequest);

                // Return success response
                return Ok(new { message = "Transition route updated successfully", flowId = response.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error updating transition route", error = ex.Message });
            }
        }
        [HttpPost("createTransitionRouteInPage")]
        public async Task<IActionResult> CreateTransitionRouteInPage(string agentId, string flowId, string pageId, [FromBody] TransitionRouteDto2 transitionRouteDto)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // or your specific location

                // Construct the page resource name
                PageName pageName = new PageName(projectId, location, agentId, flowId, pageId);

                // Retrieve the existing page
                var page = await _pagesClient.GetPageAsync(pageName);

                // Create the transition route object
                var transitionRoute = new TransitionRoute
                {
                    Intent = transitionRouteDto.IntentId,
                    Condition = transitionRouteDto.Condition,
                    TriggerFulfillment = new Fulfillment
                    {
                        Messages =
                    {
                        new ResponseMessage
                        {
                            Text = new ResponseMessage.Types.Text
                            {
                                Text_ = { transitionRouteDto.FulfillmentMessage }
                            }
                        }
                    }
                    }
                };

                // Add the target page or target flow if specified
                if (!string.IsNullOrEmpty(transitionRouteDto.TargetPage))
                {
                    transitionRoute.TargetPage = transitionRouteDto.TargetPage;
                }

                if (!string.IsNullOrEmpty(transitionRouteDto.TargetFlow))
                {
                    transitionRoute.TargetFlow = transitionRouteDto.TargetFlow;
                }

                // Add the new transition route to the page's transition routes
                page.TransitionRoutes.Add(transitionRoute);

                // Update the page with the new transition route
                var updatePageRequest = new UpdatePageRequest
                {
                    Page = page
                };

                var response = await _pagesClient.UpdatePageAsync(updatePageRequest);

                // Return success response with the updated page ID
                return Ok(new { message = "Transition route created successfully in page", pageId = response.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error creating transition route in page", error = ex.Message });
            }
        }

        [HttpGet("{agentId}/{flowId}/transitionRoutes")]
        public async Task<IActionResult> GetTransitionRoutes(string agentId, string flowId)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global";  // Adjust if necessary

                // Create the parent flow resource name
                FlowName parentFlow = new FlowName(projectId, location, agentId, flowId);

                // Create a request to list pages
                var request = new ListPagesRequest
                {
                    ParentAsFlowName = parentFlow
                };

                var transitionRoutesList = new List<object>(); // List to hold transition route details

                // Fetch the pages from Dialogflow CX
                var pages = _pagesClient.ListPagesAsync(request);

                await foreach (var page in pages)
                {
                    // Inspect transition routes for the current page
                    foreach (var route in page.TransitionRoutes)
                    {
                        // Create an object to hold the details of the transition route
                        var routeDetails = new
                        {
                            PageName = page.DisplayName,
                            TargetPage = route.TargetPage,
                            TargetFlow = route.TargetFlow,
                            Condition = route.Condition,
                            IntentId = route.Intent,
                            TriggerFulfillment = route.TriggerFulfillment
                        };

                        // Add the route details to the list
                        transitionRoutesList.Add(routeDetails);
                    }
                }

                // Return the list of transition routes
                return Ok(transitionRoutesList);
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error retrieving transition routes", error = ex.Message });
            }
        }
        [HttpPost("addPageParameter")]
        public async Task<IActionResult> AddPageParameter(string agentId, string flowId, string pageId, [FromBody] ParameterDto parameterDto)
        {
            try
            {
                // Define your project ID and location
                string projectId = "default-yrln"; // Replace with your actual project ID
                string location = "global"; // or your specific location

                // Define the page's resource name
                var pageName = PageName.FromProjectLocationAgentFlowPage(projectId, location, agentId, flowId, pageId);

                // Retrieve the existing page
                var pageRequest = new GetPageRequest { Name = pageName.ToString() };
                var page = await _pagesClient.GetPageAsync(pageRequest);

                // Ensure the Form is initialized
                if (page.Form == null)
                {
                    page.Form = new Form();
                }

                // Construct the parameter object and set prompt messages
                var parameter = new Form.Types.Parameter
                {
                    DisplayName = parameterDto.DisplayName,
                    EntityType = parameterDto.EntityType,
                    Required = parameterDto.Required,
                    IsList = parameterDto.IsList,
                    Redact = parameterDto.Redact,
                    FillBehavior = new Form.Types.Parameter.Types.FillBehavior
                    {
                        InitialPromptFulfillment = new Fulfillment
                        {
                            Messages = {
                        new ResponseMessage
                        {
                            Text = new ResponseMessage.Types.Text
                            {
                                Text_ = { parameterDto.PromptMessages }
                            }
                        }
                    }
                        }
                    }
                };

                // Add the parameter to the form
                page.Form.Parameters.Add(parameter);

                // Prepare the update request
                var updatePageRequest = new UpdatePageRequest
                {
                    Page = page,
                    UpdateMask = new FieldMask { Paths = { "form.parameters" } } // Specify the path to update
                };

                // Update the page with the new parameter
                var updatedPage = await _pagesClient.UpdatePageAsync(updatePageRequest);

                return Ok(new { message = "Parameter added to page successfully", pageId = updatedPage.Name });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error adding parameter to page", error = ex.Message });
            }
        }

        [HttpPost("createEntryFulfillment")]
        public async Task<IActionResult> CreateEntryFulfillment(string agentId, string flowId, string pageId, [FromBody] FulfillmentDto fulfillmentDto)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global";

                // Construct the page resource name
                PageName pageName = new PageName(projectId, location, agentId, flowId, pageId);

                // Retrieve the existing page
                var page = await _pagesClient.GetPageAsync(pageName);

                // Create the entry fulfillment object
                var entryFulfillment = new Fulfillment
                {
                    Messages =
                {
                    new ResponseMessage
                    {
                        Text = new ResponseMessage.Types.Text
                        {
                            Text_ = { fulfillmentDto.Messages }
                        }
                    }
                }
                };

                // Add the entry fulfillment to the page
                page.EntryFulfillment = entryFulfillment;

                // Update the page with the new entry fulfillment
                var updatePageRequest = new UpdatePageRequest
                {
                    Page = page,
                    UpdateMask = new Google.Protobuf.WellKnownTypes.FieldMask { Paths = { "entry_fulfillment" } }
                };

                var response = await _pagesClient.UpdatePageAsync(updatePageRequest);

                // Return success response with the updated page ID
                return Ok(new { message = "Entry fulfillment created successfully in page", pageId = response.Name });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating entry fulfillment in page", error = ex.Message });
            }
        }

        [HttpPut("updateEntryFulfillment")]
        public async Task<IActionResult> UpdateEntryFulfillment(string agentId, string flowId, string pageId, [FromBody] FulfillmentDto fulfillmentDto)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global";

                // Construct the page resource name
                PageName pageName = new PageName(projectId, location, agentId, flowId, pageId);

                // Retrieve the existing page
                var page = await _pagesClient.GetPageAsync(pageName);

                // Update the entry fulfillment
                page.EntryFulfillment = new Fulfillment
                {
                    Messages =
            {
                new ResponseMessage
                {
                    Text = new ResponseMessage.Types.Text
                    {
                        Text_ = { fulfillmentDto.Messages }
                    }
                }
            }
                };

                // Update the page with the modified entry fulfillment
                var updatePageRequest = new UpdatePageRequest
                {
                    Page = page,
                    UpdateMask = new Google.Protobuf.WellKnownTypes.FieldMask { Paths = { "entry_fulfillment" } }
                };

                var response = await _pagesClient.UpdatePageAsync(updatePageRequest);

                return Ok(new { message = "Entry fulfillment updated successfully in page", pageId = response.Name });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating entry fulfillment in page", error = ex.Message });
            }
        }


    }
}
