using Google.Cloud.Dialogflow.Cx.V3;

namespace DialogFlowAPI.ViewModel
{
    public class SimpleFlowDto
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
    }

    public class CreateFlowDto
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public List<TransitionRouteDto>? TransitionRoutes { get; set; }
        public NluSettings.Types.ModelType ModelType { get; set; }
        public float ClassificationThreshold { get; set; }
    }

    public class TransitionRouteDto
    {
        public string? Intent { get; set; }
        public string? TargetFlow { get; set; }
        public string? TargetPage { get; set; }
        public FulfillmentDto? Fulfillment { get; set; }
    }
    public class TransitionRouteDto2
    {
        public string IntentId { get; set; }          // Intent to trigger the transition
        public string Condition { get; set; }         // Optional condition for the transition
        public string TargetPage { get; set; }        // The target page to transition to
        public string TargetFlow { get; set; }        // Optional target flow (can transition to another flow)
        public string FulfillmentMessage { get; set; } // The message that will be displayed upon triggering the route
    }

    public class FulfillmentDto
    {
        public List<string>? Messages { get; set; }
    }
    public class TransitionRouteDto1
    {
        public string Intent { get; set; }  // Optional, Dialogflow intent to trigger the route
        public string Condition { get; set; }  // Optional, condition to trigger the route
        public string TargetFlow { get; set; }  // Optional, the target flow for the route
        public string TargetPage { get; set; }  // Optional, the target page for the route
        //public string FulfillmentMessage { get; set; }  // The fulfillment message that should be sent when this route is triggered
    }
    public class RouteWithConditionDto
    {
        public string IntentId { get; set; }  // Intent ID, not the full resource name
        public string ConditionType { get; set; }  // Can be "OR", "AND", or "CUSTOM"
        public List<string> Rules { get; set; }  // List of rules to apply if using "OR" or "AND"
        public string CustomExpression { get; set; }  // Custom expression for "CUSTOM" option
        public string TargetFlow { get; set; }  // Flow ID, will be formatted into a resource name
        public string TargetPage { get; set; }  // Page ID, will be formatted into a resource name
        //public string FulfillmentMessage { get; set; }  // The fulfillment message that should be sent when this route is triggered
    }
    public class AddRouteDto
    {
        public string Name { get; set; }  // Optional field for route name
        public string Description { get; set; }  // Optional field for description
        public string Intent { get; set; }  // Intent ID (optional)
        public string Condition { get; set; }  // The condition under which this route is triggered
        public FulfillmentDto TriggerFulfillment { get; set; }  // Fulfillment message

        // Only one of these should be set, based on whether you're transitioning to a flow or a page
        public string TargetPage { get; set; }  // Target page ID
        public string TargetFlow { get; set; }  // Target flow ID
    }

    //public class FulfillmentDto
    //{
    //    public List<string> Messages { get; set; }  // List of messages for fulfillment
    //}
    public class RouteDto
    {
        public string Intent { get; set; }  // The intent ID for the route
        public string Condition { get; set; }  // The condition under which this route should trigger
        public string FulfillmentMessage { get; set; }  // Message to be returned when the route is triggered
        public string TargetPage { get; set; }  // The target page to route to
        public string TargetFlow { get; set; }  // The target flow to route to
    }



}
