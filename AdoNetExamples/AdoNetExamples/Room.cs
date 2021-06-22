namespace AdoNetExamples
{
    public class Room : IData
    {
        public int Id { get; set; }
        public double Rent { get; set; }
        public Door Door { get; set; }
        public Window Window { get; set; }
    }
}
