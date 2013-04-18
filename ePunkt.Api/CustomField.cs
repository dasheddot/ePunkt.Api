using System;

namespace ePunkt.Api
{
    public class CustomField
    {
        public string Name { get; set; }
        public string InternalName { get; set; }
        public string StringValue { get; set; }
        public bool? BoolValue { get; set; }
        public decimal? NumberValue { get; set; }
        public DateTime? DateValue { get; set; }
        public CustomFieldType Type { get; set; }
        public bool IsPublic { get; set; }
    }

    public enum CustomFieldType
    {
        TextMultiLine = 1,
        CheckBox = 2,
        DropdownList = 3,
        CheckboxList = 4,
        Date = 5,
        TextSingleLine = 6
    }
}
