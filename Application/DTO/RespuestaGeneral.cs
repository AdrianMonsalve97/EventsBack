using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.resgeneral
{

    /// <summary>
    /// Respuesta General
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RespuestaGeneral<T>
    {
        public bool Error { get; set; }
        public string Mensaje { get; set; } = null!;
        public T Resultado { get; set; } = default(T)!;
    }
}
