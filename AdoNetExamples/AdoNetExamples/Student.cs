namespace AdoNetExamples
{
    public class Student : IData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public Address Address { get; set; }
    }
}
