using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;


namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class GetAvailableRolesViewModel
    {
        public int RoleId { get; set; }
        public string Action { get; set; }
        public List<Role> Roles { get; set; }
    }
}
