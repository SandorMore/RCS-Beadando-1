using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RCS_Beadando.Types
{
    public static class EnumExtensions
    {
        // Reads the [Display(Name = ...)] attribute so we can write @hand.Position.DisplayName() in a page
        public static string DisplayName(this Enum value)
        {
            var member = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            return member?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? value.ToString();
        }
    }
}
