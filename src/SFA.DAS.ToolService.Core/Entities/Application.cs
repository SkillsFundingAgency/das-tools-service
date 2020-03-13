namespace SFA.DAS.ToolService.Core.Entities
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public int IsExternal { get; set; }
        public int Public { get; set; }
        public int Admin { get; set; }
    }
}
