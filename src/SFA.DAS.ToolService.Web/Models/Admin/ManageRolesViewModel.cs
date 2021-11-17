using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;

namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class ManageRolesViewModel
    {
        public List<Role> LocalRoles { get; set; }
        public int SelectedRole { get; set; }
    }
}
