namespace ePunkt.Api
{
    public class FlexDate
    {
        public FlexDate()
        { }

        public FlexDate(int? day, int? month, int? year, bool isRelative = false)
        {
            Day = (day ?? 0) <= 0 ? null : day;
            Month = (month ?? 0) <= 0 ? null : month;
            Year = (year ?? 0) <= 0 ? null : year;
            IsRelativeDate = isRelative;
        }

        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public bool IsRelativeDate { get; set; }
    }
}
