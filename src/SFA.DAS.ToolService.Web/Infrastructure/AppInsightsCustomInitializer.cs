using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace SFA.DAS.ToolService.Web.Infrastructure
{
    public class ToolsTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
            {
                telemetry.Context.Cloud.RoleName = "das-tool-service-web";
                //telemetry.Context.Cloud.RoleInstance = "Custom RoleInstance";
            }
        }
    }
}
