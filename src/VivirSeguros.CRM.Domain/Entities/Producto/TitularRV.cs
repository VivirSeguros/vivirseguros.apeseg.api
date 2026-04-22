using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivirSeguros.CRM.Domain.Entities.Producto
{
    public class TitularRV
    {
        public string TipoDoc { get; set; }
        public string NoDoc { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Nombre { get; set; }
        public string SegundoNombre { get; set; }
        public string FechaNacimiento { get; set; }
        public string FechaFall { get; set; }
        public string SitInv { get; set; }
        public decimal mto_pension { get; set; }
        public string EstVigente { get; set; }
        public string Parentesco { get; set; }
        public string Estudiante { get; set; }
        public string Genero { get; set; }
        public string EstadoCivil { get; set; }
        public string FechaIngreso { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Telefono3 { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public string Direccion_Corr { get; set; }
        public string Departamento_Corr { get; set; }
        public string Provincia_Corr { get; set; }
        public string Distrito_Corr { get; set; }
        public string rutaPoliza { get; set; }
        public string rutaConstancia { get; set; }
        public List<RutaBoleta> Boletas { get; set; }
    }
}
