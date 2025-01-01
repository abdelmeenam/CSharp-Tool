using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models
{
    public class Prescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
        public int ScriptId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        public int Script { get; set; }

        [Column("RNumber")]
        public int RNumber { get; set; }

        public int RA { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string DrugName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Ins { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string PF { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Prescriber { get; set; }

        public double Qty { get; set; }

        public double ACQ { get; set; }

        public int Discount { get; set; }

        public double InsPay { get; set; }

        public double PatPay { get; set; }

        public long NDC { get; set; }

        public double RxCui { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Class { get; set; }

        public double Net { get; set; }


    }
}
