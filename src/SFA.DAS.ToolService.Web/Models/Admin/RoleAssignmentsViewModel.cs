using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;


namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class RoleAssignmentsViewModel
    {
        public int RoleId { get; set; }
        public List<Role> Roles { get; set; }
    }
}
