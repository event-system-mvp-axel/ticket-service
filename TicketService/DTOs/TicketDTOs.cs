using System;
using System.ComponentModel.DataAnnotations;

namespace TicketService.DTOs
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string QRCode { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateTicketDto
    {
        [Required]
        public Guid EventId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }

    public class TicketWithEventDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string EventTitle { get; set; }
        public string EventLocation { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string QRCode { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }
}