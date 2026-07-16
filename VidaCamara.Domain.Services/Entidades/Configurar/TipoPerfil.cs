using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.Domain.Services.Entidades.Configurar
{
    public class TipoPerfil
    {
        public int idTipoPerfil { get; set; }
        public int idProducto { get; set; }
        public string descripcion { get; set; }
        public string descripcionCorta { get; set; }
        public string usuarioCreacion { get; set; }
        public string fechaRegistro { get; set; }
        public int estado { get; set; }
        public string user { get; set; }
        public int result { get; set; }
    }
}

