using SFA.DAS.ToolService.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class RemoveApplicationViewModel
    {
        public List<Application> ExistingApplications { get; set; }

        [Range(1, Double.MaxValue)]
        public int SelectedApplication { get; set; }
    }
}
