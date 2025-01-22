using System.Runtime.Serialization;

namespace EventsApi.Domain.Enums
{
    /// <summary>
    /// Enum de roles
    /// </summary>
    public enum Rol
    {
        [EnumMember(Value = "Administrador")]
        Administrador = 1,

        [EnumMember(Value = "Usuario")]
        Usuario = 2,

        [EnumMember(Value = "Aprobador")]
        Aprobador = 3,

        [EnumMember(Value = "Cliente")]
        Cliente = 4,

        [EnumMember(Value = "Logistico")]
        Logistico = 5,
        
        [EnumMember(Value = "Ejecutivo")]
        Ejecutivo = 6,
        
    }

    /// <summary>
    /// Extensión para obtener el valor de EnumMember.
    /// </summary>
    public static partial class EnumExtensions
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