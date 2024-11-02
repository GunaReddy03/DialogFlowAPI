namespace DialogFlowAPI.ViewModel
{
    public class AgentMessageDto
    {
        public string AgentId { get; set; }
        public string SessionId { get; set; }
        public string UserMessage { get; set; }
        public string LanguageCode { get; set; } = "en-US";
    }
    public class SessionRequest
    {
        public string SessionId { get; set; }
        public string Text { get; set; }
        public string LanguageCode { get; set; } = "en";
    }

    public class SessionResponse
    {
        public string ResponseMessage { get; set; }
        public bool IsEndOfConversation { get; set; }
    }


}
