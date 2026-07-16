using System;
using System.Collections.Generic;
using System.Text;

namespace VidaCamara.Domain.Services.Entidades.Configurar
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int idTipoPerfil { get; set; }
        public int idPuntoVenta { get; set; }
        public int estado { get; set; }
        public string desNombre { get; set; }
        public string desPerfil { get; set; }
        public bool activeDirectory { get; set; }

        // Persona
        public int idPersona { get; set; }
        public Int32 idTipoPersona { get; set; }
        public Int32 idTipoDocumento { get; set; }
        public Int32 idProducto { get; set; }
        public string nroDoc { get; set; }
        public string nombre1 { get; set; }
        public string nombre2 { get; set; }
        public string apPaterno { get; set; }
        public string apMaterno { get; set; }
        public string razonSocial { get; set; }

        // Direccion
        public Int32 idDireccion { get; set; }
        public string direccion { get; set; } //Direccion en la tabla
        public string idDepartamento { get; set; }
        public string idProvincia { get; set; }
        public string idDistrito { get; set; }

        // Telefono
        public Int32 idFono { get; set; }
        public string fijo { get; set; }
        public string celular { get; set; }
        public string email { get; set; }
        public string email2 { get; set; }
        public string producto { get; set; }
        public string usuarioCreacion { get; set; }
        public string fechaRegistro { get; set; }

        public string user { get; set; }
        public int result { get; set; }
    }
}
