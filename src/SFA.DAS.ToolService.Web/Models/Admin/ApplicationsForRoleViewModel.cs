using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;

namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class ApplicationsForRoleViewModel
    {
        public List<Application> AssignedApplications { get; set; }
        public List<Application> UnassignedApplications { get; set; }
        public int ApplicationId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
