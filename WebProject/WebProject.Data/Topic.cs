namespace WebProject.Data
{
    public class Topic
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int Name { get; set; }
        public string Description { get; set; }
        public Course course { get; set; }
    }
}
