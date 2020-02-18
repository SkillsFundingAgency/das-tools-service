using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;


namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class ApplicationsForRoleViewModel
    {
        public List<Application> Applications { get; set; }
        public int ApplicationId { get; set; }
        public int RoleId { get; set; }
    }
}
