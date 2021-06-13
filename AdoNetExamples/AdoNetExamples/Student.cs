namespace AdoNetExamples
{
    partial class Program
    {
        public class Student : IData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Weight { get; set; }
        }
    }
}
