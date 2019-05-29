using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace SFA.DAS.ToolService.Authentication.Models
{
    public class AccessDeniedViewModel
    {
        public string[] Organizations { get; set; }

        public AccessDeniedViewModel(string organizations)
        {
            Organizations = organizations.Split(",");
        }

    }
}