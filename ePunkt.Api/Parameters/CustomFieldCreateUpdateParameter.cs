using System;

namespace ePunkt.Api.Parameters
{
    public class CustomFieldCreateUpdateParameter
    {
        public string InternalName { get; set; }
        public string StringValue { get; set; }
        public bool? BoolValue { get; set; }
        public decimal? NumberValue { get; set; }
        public DateTime? DateValue { get; set; }
    }
}
