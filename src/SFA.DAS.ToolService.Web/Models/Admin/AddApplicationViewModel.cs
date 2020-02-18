using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ToolService.Core.Entities;

namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class AddApplicationViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Path { get; set; }
    }
}
