namespace Bloc3_CSharp.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string OnDate { get; set; }
        public string OffDate { get; set; }
        public int Value { get; set; }
        public Discount() { }
        public Discount(int id, string onDate, string offDate, int value)
        {
            Id = id;
            OnDate = onDate;
            OffDate = offDate;
            Value = value;
        }

    }
}
