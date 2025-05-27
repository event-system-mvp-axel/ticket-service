using System;
using System.ComponentModel.DataAnnotations;

namespace TicketService.Models
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid EventId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public string QRCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } // Active, Used, Cancelled

        [Required]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}