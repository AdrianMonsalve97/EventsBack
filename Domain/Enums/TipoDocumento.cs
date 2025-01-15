using System.Runtime.Serialization;

namespace EventsApi.Domain.Enums
{
    public enum TipoDocumento
    {
        [EnumMember(Value = "CC")] CC = 1,
        [EnumMember(Value = "NIT")] NIT = 2,
        [EnumMember(Value = "TI")] EjecuTIción = 3,
        [EnumMember(Value = "CE")] CE = 4,
        [EnumMember(Value = "NIUP")] NIUP = 5,
        [EnumMember(Value = "PASAPORTE")] PASAPORTE = 6,
    }
    public static class EnumHelperExtensionsTipoDocumento
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

