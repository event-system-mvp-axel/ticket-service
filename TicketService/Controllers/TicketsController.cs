using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketService.Data;
using TicketService.DTOs;
using TicketService.Models;
using TicketService.Services;

namespace TicketService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly TicketContext _context;
        private readonly QRCodeService _qrCodeService;
        private readonly EventServiceClient _eventServiceClient;

        public TicketsController(TicketContext context, QRCodeService qrCodeService, EventServiceClient eventServiceClient)
        {
            _context = context;
            _qrCodeService = qrCodeService;
            _eventServiceClient = eventServiceClient;
        }

        // POST: api/tickets/purchase
        [HttpPost("purchase")]
        public async Task<ActionResult<TicketDto>> PurchaseTicket(CreateTicketDto createDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Check if event exists (would normally call Event Service)
            // For MVP, we'll skip this check

            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                EventId = createDto.EventId,
                PurchaseDate = DateTime.UtcNow,
                Status = "Active",
                Price = createDto.Price,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Generate QR Code
            ticket.QRCode = _qrCodeService.GenerateQRCode(ticket.Id);

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Ok(new TicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                EventId = ticket.EventId,
                PurchaseDate = ticket.PurchaseDate,
                QRCode = ticket.QRCode,
                Status = ticket.Status,
                Price = ticket.Price
            });
        }

        // GET: api/tickets/my-tickets
        [HttpGet("my-tickets")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetMyTickets()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var tickets = await _context.Tickets
                .Where(t => t.UserId == Guid.Parse(userId))
                .OrderByDescending(t => t.PurchaseDate)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    EventId = t.EventId,
                    PurchaseDate = t.PurchaseDate,
                    QRCode = t.QRCode,
                    Status = t.Status,
                    Price = t.Price
                })
                .ToListAsync();

            return Ok(tickets);
        }

        // GET: api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetTicket(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == Guid.Parse(userId));

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(new TicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                EventId = ticket.EventId,
                PurchaseDate = ticket.PurchaseDate,
                QRCode = ticket.QRCode,
                Status = ticket.Status,
                Price = ticket.Price
            });
        }

        // GET: api/tickets/qr/{ticketId}
        [HttpGet("qr/{ticketId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTicketQRCode(Guid ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                return NotFound();
            }

            var qrCodeBytes = Convert.FromBase64String(ticket.QRCode);
            return File(qrCodeBytes, "image/png");
        }
    }
}