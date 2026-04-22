namespace VivirSeguros.CRM.Domain.Entities.Billing
{
    public class IdentificadorDocumento
    {
        public string numeracion { get; set; }
        public string fechaEmision { get; set; }
        public string codTipoDocumento { get; set; }
        public string tipoMoneda { get; set; }
        public string fechaVencimiento { get; set; }
    }
}
