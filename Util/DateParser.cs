namespace Software_Developer_CSharp_Test_01_v1_Dec_2023.Util
{
    public class DateParser
    {
        public static long ParseDate(string dateUtc)
        {
            DateTime utcDateTime = DateTime.Parse(dateUtc, null, System.Globalization.DateTimeStyles.RoundtripKind);
            DateTimeOffset dateTimeOffset = new DateTimeOffset(utcDateTime);
            long epochTime = dateTimeOffset.ToUnixTimeSeconds();
            return epochTime;
        }
        public static long CalculateTotalHoursWorked(string startDateUtc, string endDateUtc)
        {
            long startDateEpoch = ParseDate(startDateUtc);
            long endDateEpoch = ParseDate(endDateUtc);
            return (endDateEpoch - startDateEpoch) / 60 / 60; //sec*minutes
        }
    }
}
