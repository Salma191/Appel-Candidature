using System.ComponentModel.DataAnnotations;

namespace pfe_back.Services
{
    public class EnumUtils
    {
        public static string GetEnumDisplayName(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var displayAttribute = fieldInfo?.GetCustomAttributes(typeof(DisplayAttribute), false)
                                            .FirstOrDefault() as DisplayAttribute;
            return displayAttribute?.Name ?? value.ToString();
        }

    }
}
