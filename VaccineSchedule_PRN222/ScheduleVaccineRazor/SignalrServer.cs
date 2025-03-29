using Microsoft.AspNetCore.SignalR;

namespace ScheduleVaccineRazor
{
    public class SignalrServer: Hub
    {
        public async Task NotifyItemDeleted(string itemId)
        {
            await Clients.All.SendAsync("ItemDeleted", itemId);
        }
        public async Task NotifyItemCreate(string itemId)
        {
            await Clients.All.SendAsync("ItemCreated", itemId);
        }

        public async Task NotifyItemUpdate(string itemId)
        {
            await Clients.All.SendAsync("ItemUpdated", itemId);
        }
    }
}
