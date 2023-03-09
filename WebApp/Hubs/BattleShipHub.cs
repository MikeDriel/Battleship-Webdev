using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs
{
    public class BattleShipHub : Hub
    {
        public async Task CreateGame()
        {

        }
        
        public async Task JoinGame(string gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
        }

        public async Task Attack(string gameId, int squareId)
        {
            await Clients.OthersInGroup(gameId).SendAsync("OpponentAttacked", squareId);
        }
    }
}