using Microsoft.AspNetCore.SignalR;
using S1.Operator.SignalR.Models;

namespace S1.Operator.SignalR.Hubs;

public class LastBetHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var brandId = httpContext?.Request.Query["brandId"];
        if (brandId is null)
        {
            Context.Abort();

            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, brandId.ToString());

        await base.OnConnectedAsync().ConfigureAwait(false);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var httpContext = Context.GetHttpContext();
        var brandId = httpContext?.Request.Query["brandId"];
        if (brandId is not null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, brandId.ToString());
        }

        await base.OnDisconnectedAsync(exception);
    }
    public async Task SendLastBet(BetItem betItem)
    {
        await Clients.Group(betItem.BrandId.ToString()).SendAsync("ReceiveLastBet", betItem);
    }
}
