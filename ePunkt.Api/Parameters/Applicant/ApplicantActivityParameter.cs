namespace ePunkt.Api.Parameters
{
    public class ApplicantActivityParameter
    {
        public int ApplicantId { get; set; }
        public int Name { get; set; }

        public FlexDate Start { get; set; }
        public FlexDate End { get; set; }
    }
}
