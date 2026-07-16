using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.Domain.Services.Entidades.Configurar
{
    public class Rol
    {
        public int idRol { get; set; }
        public int estado { get; set; }
        public int idModulo { get; set; }
        public string descripcion { get; set; }
        public string url { get; set; }
        public int idPadre { get; set; }
        public int idPerfil { get; set; }
        public string grupo { get; set; }
        public string acceso { get; set; }
        public string usuarioCreacion { get; set; }
        public string fechaRegistro { get; set; }
        public string user { get; set; }
        public int result { get; set; }
    }




}
