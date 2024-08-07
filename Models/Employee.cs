using System.Runtime.Serialization;

namespace Software_Developer_CSharp_Test_01_v1_Dec_2023.Models
{
    [DataContract]
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public float HoursWorked { get; set; }


    }
}
