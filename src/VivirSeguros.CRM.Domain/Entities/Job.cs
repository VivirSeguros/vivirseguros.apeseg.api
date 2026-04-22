using System;

namespace VivirSeguros.CRM.Domain.Entities
{
    public class Job
    {
        public int JobId { get; set; }
        public int ProductId { get; set; }
        public int TypeJobId { get; set; }
        public int PaymentId { get; set; }
        public string Skey { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int State { get; set; }
        public string CreationUser { get; set; }
        public DateTime RegisterDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime ModificationDate { get; set; }
        public JobDetail DetailJob { get; set; } = new JobDetail();
    }
}
