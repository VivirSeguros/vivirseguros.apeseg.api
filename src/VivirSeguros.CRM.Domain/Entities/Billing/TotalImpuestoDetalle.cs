namespace VivirSeguros.CRM.Domain.Entities.Billing
{
    public class TotalImpuestoDetalle
    {
        public string idImpuesto { get; set; }
        public string montoImpuesto { get; set; }
        public string tipoAfectacion { get; set; }
        public string montoBase { get; set; }
        public string porcentaje { get; set; }
    }
}
