using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ToolService.Web.Configuration
{
    public class AdminRoleAssignmentRouteNames
    {
        public const string AddApplication = "AddApplication";
        public const string RemoveApplication = "RemoveApplication";
        public const string GetUnassignedApplications = "GetUnassignedApplications";
        public const string GetAssignedApplications = "GetAssignedApplications";
        public const string RoleAssignment = "RoleAssignment";
        public const string AssignmentChoice = "AssignmentChoice";
        public const string HandleAssignmentChoice = "HandleAssignmentChoice";
    }

    public class AdminApplicationRouteNames
    {
        public const string ManageApplications = "ManageApplications";

    }

    public class AdminRoleRouteNames
    {
        public const string ManageRoles = "ManageRoles";
        public const string SyncRoles = "SyncRoles";
    }

}
