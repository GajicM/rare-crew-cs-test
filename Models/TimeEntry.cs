using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Software_Developer_CSharp_Test_01_v1_Dec_2023.Util;

namespace Software_Developer_CSharp_Test_01_v1_Dec_2023.Models
{
    [DataContract]
    public class TimeEntry

    {

        [JsonConstructor]
        public TimeEntry(String Id, String EmployeeName, String StartTimeUtc, String EndTimeUtc, String EntryNotes, String? DeletedOn) {
            this.Id = Id;
            this.EmployeeName = EmployeeName;
            this.EntryNotes = EntryNotes;  
            this.DeletedOn = DeletedOn;
            this.EndTimeUtc = EndTimeUtc;
            this.StartTimeUtc = StartTimeUtc;
            if (string.IsNullOrEmpty(EmployeeName)) {
                this.EmployeeName = "MISSING NAME";
            }
            if (string.IsNullOrEmpty(this.StartTimeUtc) || string.IsNullOrEmpty(this.EndTimeUtc))
            {
                TotalTimeWorked = 0;
            }
            else
            {
                TotalTimeWorked = DateParser.CalculateTotalHoursWorked(this.StartTimeUtc, this.EndTimeUtc);
            }
        }
      
        public String? Id { get; set; }
        public String? EmployeeName { get; set; }
        [JsonPropertyName("StarTimeUtc")]
        public String? StartTimeUtc { get; set; }
        public String? EndTimeUtc { get; set; }
        public String? EntryNotes { get; set; }
        public String? DeletedOn { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public long TotalTimeWorked { get; set; }
    }
}
