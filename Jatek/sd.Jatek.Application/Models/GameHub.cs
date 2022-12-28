using MediatR;
using Microsoft.AspNetCore.SignalR;
using sd.Jatek.Application.Commands;
using sd.Jatek.Application.Querys;

namespace sd.Jatek.Application.Models
{
    public class GameHub : Hub
    {
        private readonly ISender _mediator;
        public GameHub(ISender mediator)
        {
            _mediator = mediator;
        }

        public Task JoinGroup(string group)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public Task LeaveRoom(string group)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        }
        public async Task SendMessage(string group, string user, string message)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
        }

        public async Task StartGame(string group)
        {
            await _mediator.Send(new StartGameCommand(group));

            await Clients.Group(group).SendAsync("GameStarted");
        }

        public async Task RightGuess(string group, string playerId)
        {
            await _mediator.Send(new RightGuessCommand(group, playerId));

            await Clients.Group(group).SendAsync("RecieveRightGuesses");
        }

        public async Task GetWord(string group)
        {
            var word = await _mediator.Send(new GetWordQuery());

            await Clients.Group(group).SendAsync("RecieveWord", word);
        }

        public async Task GetGroupsPlayers(string group)
        {
            var data = await _mediator.Send(new GetGameDataQuery(group));

            await Clients.Group(group).SendAsync("RecievePlayers", data.Players);
        }

        public async Task GetRounds(string group)
        {
            var data = await _mediator.Send(new GetGameDataQuery(group));

            await Clients.Group(group).SendAsync("RecieveRounds", data.Rounds);
        }

        public async Task NextPlayer(string group)
        {
            var roundOver = await _mediator.Send(new GetNextPlayerQuery(group));

            await Clients.Group(group).SendAsync("RecieveRoundOver", roundOver);
        }

        public async Task GameOver(string group)
        {
            var winner = await _mediator.Send(new GetWinnerQuery(group));

            await _mediator.Send(new RemoveRoomCommand(group));

            await Clients.Group(group).SendAsync("RecieveWinnerOnGameOver", winner.Tie, winner.Points, winner.Winners);
        }

        public async Task UpdateCanvas(string group, int prevX, int prevY, int currX, int currY, int width, string color)
        {
            await Clients.Group(group).SendAsync("RecieveUpdateCanvas", prevX, prevY, currX, currY, width, color);
        }

        public async Task ClearCanvas(string group)
        {
            await Clients.Group(group).SendAsync("RecieveClearCanvas");
        }
    }
}
