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
        public string Name { get; set; }
    }
    public class EditSectionModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}