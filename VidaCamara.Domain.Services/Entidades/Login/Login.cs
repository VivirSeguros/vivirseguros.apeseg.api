using System;
using System.Collections.Generic;
using System.Text;
using VidaCamara.Domain.Services.Entidades.Configurar;

namespace VidaCamara.Domain.Services.Entidades.Login
{
    public class Modulo {
        public Int32 idModulo { get; set; }
        public string Descripcion { get; set; }
        public Int32 Nivel { get; set; }
        public string Acceso { get; set; }
        public bool Estado { get; set; }
        public string Url { get; set; }
    }
    
    public class LoginParam {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class Login {
        public int idUsuario { get; set; }
        public int idPerfil { get; set; }
        public string desPerfil { get; set; }
        public string login { get; set; }
        public string nombreUsuario { get; set; }
        public bool estado { get; set; }
        public int idPersona { get; set; }
        public int idPtoVenta { get; set; }
        public string nombrePtoVenta { get; set; }
        public int idCanal { get; set; }
        public string nombreCanal { get; set; }
        public object jwt { get; set; }
        public object menu { get; set; }
        public bool ActiveDirectory { get; set; }
    }
   
    public class ComboParam
    {
        public string tabla { get; set; }
        public string campo1 { get; set; }
        public string campo2 { get; set; }
        public string campo3 { get; set; }
    }

    public class Combo
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }
}
