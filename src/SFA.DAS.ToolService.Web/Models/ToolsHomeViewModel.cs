using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace SFA.DAS.ToolService.Web.Models
{
    public class ToolsHomeViewModel
    {
        public string UserName { get; set; }
        public List<Application> Applications { get; set; }
    }
}
