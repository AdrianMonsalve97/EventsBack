using System.ComponentModel;
using System.Runtime.Serialization;

namespace EventsApi.Models.Enums
{
/// <summary>
/// Enum de formatos
/// </summary>
public enum Rol
{
    [EnumMember(Value = "Administrador")]
    Administrador =1,
    [EnumMember(Value = "Usuario")]
    Usuario = 2,
    [EnumMember(Value = " Aprobador")]
    Aprobador = 3,
    [EnumMember(Value = " Cliente")]
    Cliente = 4,
    [EnumMember(Value = " Logistico")]
    Logistico = 5,


}
/// <summary>
/// Extencion de objetos enum
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// obtiene en el EnumMemberValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static string GetEnumMemberValue<T>(this T enumValue) where T : Enum
    {
        var type = enumValue.GetType();
        var fieldInfo = type.GetField(enumValue.ToString());
        var attribute = fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .FirstOrDefault() as EnumMemberAttribute;

        return attribute != null ? attribute.Value : enumValue.ToString();
    }
}
}