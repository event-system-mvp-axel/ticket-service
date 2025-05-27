using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TicketService.Services
{
    public class EventServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _eventServiceUrl;

        public EventServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _eventServiceUrl = configuration["EventServiceUrl"];
        }

        public async Task<EventInfo> GetEventAsync(Guid eventId)
        {
            var response = await _httpClient.GetAsync($"{_eventServiceUrl}/api/events/{eventId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EventInfo>(content);
            }

            return null;
        }
    }

    public class EventInfo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
    }
}