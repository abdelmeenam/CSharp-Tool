using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyWebAPI.Models
{
    public class Drug
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DrugID { get; set; }

        public string DrugName { get; set; }

        [Column(TypeName = "varchar(max)")] // SQL Server: Use "varchar(max)" for non-Unicode

        public string NDC { get; set; }


        [MaxLength(50)]
        public string Form { get; set; }


        [MaxLength(50)]
        public string Strength { get; set; }


        [MaxLength(100)]
        public string Manufacturer { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AcquisitionCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AWP { get; set; }


        [MaxLength(50)]
        public string Dispensed { get; set; }

        public DateTime PreviousUpdate { get; set; }

        [Column(TypeName = "varchar(max)")] // SQL Server: Use "varchar(max)" for non-Unicode
        public string DrugClass { get; set; }

        [MaxLength(100)]
        public string EPCClass { get; set; }


        [Column(TypeName = "varchar(max)")] // SQL Server: Use "varchar(max)" for non-Unicode
        public string? RxCUI { get; set; }

    }
}