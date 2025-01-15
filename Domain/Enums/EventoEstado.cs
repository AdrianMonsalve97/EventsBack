using System;
using System.Linq;
using System.Runtime.Serialization;

namespace EventsApi.Domain.Enums
{
    public enum EventoEstado
    {
        [EnumMember(Value = "Asignado")] Asignado = 1,
        [EnumMember(Value = "Cotización")] Cotización = 2,
        [EnumMember(Value = "Ejecución")] Ejecución = 3,
        [EnumMember(Value = "Finalizado")] Finalizado = 4,
        [EnumMember(Value = "Facturación")] Facturación = 5,
        [EnumMember(Value = "Fin Facturación")] FinFacturación = 6,
        [EnumMember(Value = "Espera por asignacion")] Espera = 7
    }

    public static class EnumHelperExtensionsEventoEstado
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


