using Microsoft.EntityFrameworkCore;

namespace DialogFlowAPI.Models
{
    public class TempClass
    {
        public int Id { get; set; }
        public int? NumberofStudents { get; set; }
        public string? State { get; set; }
        public string? Programme { get; set; }
        public DateTime? Createdon { get; set; }
        public DateTime? Updatedon { get; set; }

        
    }
}
