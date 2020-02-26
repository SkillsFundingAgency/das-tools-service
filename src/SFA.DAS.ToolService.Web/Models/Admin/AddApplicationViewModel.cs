using System.ComponentModel.DataAnnotations;

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
