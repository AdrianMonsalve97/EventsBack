using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EventsApi.Models.Enums
{
    public enum Prioridad
    {
        [EnumMember(Value = "Alta")]
        Alta = 1,

        [EnumMember(Value = "Media")]
        Media = 2,

        [EnumMember(Value = "Baja")]
        Baja = 3,

        [EnumMember(Value = "Crítica")]
        Critica = 4,

        [EnumMember(Value = "Urgente")]
        Urgente = 5,

        [EnumMember(Value = "No urgente")]
        NoUrgente = 6
    }

    public static class EnumHelperExtensions
    {
        public static string GetEnumMemberValue<T>(this T enumValue) where T : Enum
        {
            var type = enumValue.GetType();
            var fieldInfo = type.GetField(enumValue.ToString());
            var attribute = fieldInfo?.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .FirstOrDefault() as EnumMemberAttribute;

            return attribute?.Value ?? enumValue.ToString();
        }
    }

}