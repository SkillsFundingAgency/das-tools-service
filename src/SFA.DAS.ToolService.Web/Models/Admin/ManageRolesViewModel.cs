using SFA.DAS.ToolService.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel;

namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class ManageRolesViewModel
    {
        public List<Auth0Role> IdentiyProviderRoles { get; set; }
    }

}
