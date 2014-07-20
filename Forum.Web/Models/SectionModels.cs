using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models
{
    public class SectionModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class CreateSectionModel
    {
        [Required(ErrorMessage = "请输入版块名称。")]
        public string Name { get; set; }
    }
    public class EditSectionModel
    {
        [Required(ErrorMessage = "版块ID不能为空。")]
        public string Id { get; set; }
        [Required(ErrorMessage = "请输入版块名称。")]
        public string Name { get; set; }
    }
}