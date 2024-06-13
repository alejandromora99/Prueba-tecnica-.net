using System;
using System.ComponentModel.DataAnnotations;

namespace CronProject.Models
{
    public class DataRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(12)]
        public required string RutClienteEnvia { get; set; }

        [Required]
        [MaxLength(100)]
        public required string NombreClienteEnvia { get; set; }

        [Required]
        [MaxLength(20)]
        public required string IdTransaccion { get; set; }

        [Required]
        [MaxLength(12)]
        public required string RutClienteRecibe { get; set; }

        [Required]
        [MaxLength(100)]
        public required string NombreClienteRecibe { get; set; }

        [Required]
        [MaxLength(5)]
        public required string CodigoBancoReceptor { get; set; }

        [Required]
        [MaxLength(100)]
        public required string NombreBancoReceptor { get; set; }

        [Required]
        [Range(0, 9999999999999999.99)]
        public required decimal MontoTransferencia { get; set; }

        [Required]
        [MaxLength(3)]
        public required string MonedaTransferencia { get; set; }

        [Required]
        public required DateTime FechaTransferencia { get; set; }
    }
}
