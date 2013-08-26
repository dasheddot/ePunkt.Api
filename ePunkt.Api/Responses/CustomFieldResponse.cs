using ePunkt.Api.Parameters;

namespace ePunkt.Api.Responses
{
    public class CustomFieldResponse : CustomFieldUpdateParameter
    {
        public string Name { get; set; }
        public CustomFieldResponseType Type { get; set; }
        public bool IsPublic { get; set; }
    }

    public enum CustomFieldResponseType
    {
        TextMultiLine = 1,
        CheckBox = 2,
        DropdownList = 3,
        CheckboxList = 4,
        Date = 5,
        TextSingleLine = 6
    }
}
