using System.ComponentModel;

namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class ManageApplicationsViewModel
    {
        public ManageApplicationActions Action { get; set; }
    }

    public enum ManageApplicationActions
    {
        [Description("Add an Application")]
        AddApplication,
        [Description("Remove an Application")]
        RemoveApplication        
    }
}
