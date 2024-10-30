namespace DialogFlowAPI.ViewModel
{
    public class ParameterDto
    {
        public string DisplayName { get; set; }
        public string EntityType { get; set; } // The entity type of the parameter
        public bool Required { get; set; } // Whether the parameter is required
        public bool IsList { get; set; } // Whether the parameter is a list
        public bool Redact { get; set; } // Whether to redact the parameter value
        public string PromptMessages { get; set; } // The initial prompt message for the agent
    }

    public class IntentParameterDto
    {
        public string DisplayName { get; set; }    // Display name of the parameter
        public string EntityType { get; set; }     // Entity type, e.g., "@sys.date"
        public bool IsList { get; set; }           // Whether it’s a list parameter
        public bool Redact { get; set; }           // Whether to redact the parameter data
        public bool Required { get; set; }         // Whether the parameter is required
    }

}
