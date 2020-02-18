using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ToolService.Core.Entities;

namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class RemoveApplicationViewModel
    {
        public List<Application> ExistingApplications { get; set; }
    }
}
