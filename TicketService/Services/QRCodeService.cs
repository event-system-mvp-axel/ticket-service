using System;

namespace TicketService.Services
{
    public class QRCodeService
    {
        public string GenerateQRCode(Guid ticketId)
        {
            // För MVP, returnera bara en placeholder Base64-sträng
            // I produktion skulle du använda ett riktigt QR-bibliotek

            // Detta är en 1x1 pixel transparent PNG som placeholder
            return "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg==";
        }
    }
}