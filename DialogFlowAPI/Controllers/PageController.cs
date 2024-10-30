using Google.Cloud.Dialogflow.Cx.V3;
using Microsoft.AspNetCore.Mvc;
namespace DialogFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        //private readonly Dialof _dialogflowClient;
        private readonly PagesClient _pagesClient;
        private readonly Google.Cloud.Dialogflow.Cx.V3.FlowsClient _flowsClient;

        public PageController(PagesClient pagesClient, Google.Cloud.Dialogflow.Cx.V3.FlowsClient flowsClient)
        {
            _pagesClient = pagesClient;
            _flowsClient = flowsClient;
        }

        // GET: /page/get-all
        [HttpGet("Page List")]
        public async Task<IActionResult> GetPages(string agentId, string flowId)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary

                // Create the parent flow resource name, now including agentId
                FlowName parent = new FlowName(projectId, location, agentId, flowId);

                // Create a request to list pages
                var request = new ListPagesRequest
                {
                    ParentAsFlowName = parent
                };

                // Create a list to hold the pages
                var pagesList = new List<Page>();

                // Fetch the pages from Dialogflow CX
                var pages = _pagesClient.ListPagesAsync(request);

                await foreach (var page in pages)
                {
                    pagesList.Add(page);
                }

                // Return the list of pages
                return Ok(pagesList);
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error retrieving pages", error = ex.Message });
            }
        }
        [HttpGet("GetPage")]
        public async Task<IActionResult> GetPageById(string agentId, string flowId, string pageId)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // Adjust if necessary

                // Construct the full resource name of the page
                PageName pageName = new PageName(projectId, location, agentId, flowId, pageId);

                // Make the request to get the page by its ID
                var page = await _pagesClient.GetPageAsync(pageName);

                // Return the page details
                return Ok(page);
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error retrieving the page", error = ex.Message });
            }
        }

        [HttpPost("create/{agentId}/{flowId}")]
        public async Task<IActionResult> CreatePage(string agentId, string flowId, [FromBody] PageDto pageDto)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // or your specific location

                // Construct the agent's parent resource name
                FlowName parent = new FlowName(projectId, location, agentId, flowId);

                // Create the new page object with minimal required fields
                Page newPage = new Page
                {
                    DisplayName = pageDto.DisplayName,
                //    EntryFulfillment = new Fulfillment
                //    {
                //        Messages = { new Message
                //    {
                //        Text = new Text { Text = { "Welcome to the new page!" } }
                //    }
                //}
                //    }
                };

                // Create the request to create the page
                var request = new CreatePageRequest
                {
                    ParentAsFlowName = parent,
                    Page = newPage
                };

                // Call Dialogflow CX to create the page
                var response = await _pagesClient.CreatePageAsync(request);

                // Return success response with the newly created page's ID
                return Ok(new { message = "Page created successfully", pageId = response.Name });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error creating page", error = ex.Message });
            }
        }
        public class PageDto
        {
            public string DisplayName { get; set; }
        }
        [HttpDelete("delete/{agentId}/{flowId}/{pageId}")]
        public async Task<IActionResult> DeletePage(string agentId, string flowId, string pageId)
        {
            try
            {
                // Replace with your actual project ID and location
                string projectId = "default-yrln";
                string location = "global"; // or your specific location

                // Construct the page resource name
                PageName pageName = new PageName(projectId, location, agentId, flowId, pageId);

                // Call Dialogflow CX to delete the page
                await _pagesClient.DeletePageAsync(pageName);

                // Return success response
                return Ok(new { message = "Page deleted successfully" });
            }
            catch (Exception ex)
            {
                // Handle errors and return a proper response
                return StatusCode(500, new { message = "Error deleting page", error = ex.Message });
            }
        }
        //[HttpPost("{agentId}/{flowId}/{pageId}/add-route")]
        //public async Task<IActionResult> AddRouteToPage(string agentId, string flowId, string pageId, [FromBody] AddRouteDto routeDto)
        //{
        //    try
        //    {
        //        // Replace with your actual project ID and location
        //        string projectId = "default-yrln";
        //        string location = "global";

        //        // Construct the page resource name
        //        PageName currentPage = new PageName(projectId, location, agentId, flowId, pageId);

        //        // Retrieve the existing page
        //        var page = await _pagesClient.GetPageAsync(currentPage);

        //        // Create a new TransitionRoute and populate fields based on the request body
        //        TransitionRoute newRoute = new TransitionRoute
        //        {
        //            Intent = routeDto.Intent != null ? IntentName.Format(projectId, location, agentId, routeDto.Intent) : null,
        //            Condition = routeDto.Condition,
        //            //TriggerFulfillment = new Fulfillment
        //            //{
        //            //    Messages = { new Message { Text = new Text { Text_ = { routeDto.TriggerFulfillment.Messages.FirstOrDefault() } } } }
        //            //}
        //        };

        //        // Determine if the route is targeting a flow or a page, and set the appropriate field
        //        if (!string.IsNullOrEmpty(routeDto.TargetPage))
        //        {
        //            // Set the target page
        //            newRoute.TargetPage = PageName.Format(projectId, location, agentId, flowId, routeDto.TargetPage);
        //        }
        //        else if (!string.IsNullOrEmpty(routeDto.TargetFlow))
        //        {
        //            // Set the target flow
        //            newRoute.TargetFlow = FlowName.Format(projectId, location, agentId, routeDto.TargetFlow);
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Either targetPage or targetFlow must be provided.");
        //        }

        //        // Add the new route to the page's transition routes
        //        page.TransitionRoutes.Add(newRoute);

        //        // Update the page with the new transition route
        //        var updateRequest = new UpdatePageRequest
        //        {
        //            Page = page,
        //            UpdateMask = new FieldMask { Paths = { "transition_routes" } }
        //        };
        //        var response = await _pagesClient.UpdatePageAsync(updateRequest);

        //        // Return success response with the updated page's ID
        //        return Ok(new { message = "Route added successfully", pageId = response.Name });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any errors and return an appropriate response
        //        return StatusCode(500, new { message = "Error adding route", error = ex.Message });
        //    }
        //}
        
      
        

  

    }
}
