using System.ComponentModel;

namespace SFA.DAS.ToolService.Web.Models.Admin
{
    public class AssignmentChoiceViewModel
    {
        public AssignmentChoiceActions Action { get; set; }
    }

    public enum AssignmentChoiceActions
    {
        [Description("Add assignment")]
        GetUnassignedApplications,

        [Description("Remove assignment")]
        GetAssignedApplications
    }
}
