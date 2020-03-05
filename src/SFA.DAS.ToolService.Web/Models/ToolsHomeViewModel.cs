using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;

namespace SFA.DAS.ToolService.Web.Models
{
    public class ToolsHomeViewModel
    {
        public string UserName { get; set; }
        public List<Application> Applications { get; set; }
    }
}
