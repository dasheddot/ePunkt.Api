using ePunkt.Api.Parameters;

namespace ePunkt.Api.Models
{
    public class CustomField : CustomFieldCreateUpdateParameter
    {
        public string Name { get; set; }
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
