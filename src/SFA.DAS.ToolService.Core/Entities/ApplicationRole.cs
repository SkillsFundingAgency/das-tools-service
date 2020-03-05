namespace SFA.DAS.ToolService.Core.Entities
{
    public class ApplicationRole
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int RoleId { get; set; }
        public virtual Application Application { get; set; }
        public virtual Role Role { get; set; }
    }
}
