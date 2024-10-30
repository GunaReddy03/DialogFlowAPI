using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static DialogFlowAPI.Models.Logins;

namespace DialogFlowAPI.Models
{
    public class AgentUserModel
    {
        [Key]
        //public int AgentID { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AgentID { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
