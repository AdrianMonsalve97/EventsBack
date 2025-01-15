using System.Runtime.Serialization;

namespace EventsApi.Domain.Enums
{
    public enum EstadoAcIn
    {
        [EnumMember(Value = "Activo")]
        Activo = 1,

        [EnumMember(Value = "Inactivo")]
        Inactivo = 2

    }

    public static class EnumHelperExtensionsEstado
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