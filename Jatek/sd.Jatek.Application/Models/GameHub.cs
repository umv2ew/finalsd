using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using sd.Jatek.Application.Commands;
using sd.Jatek.Application.Querys;

namespace sd.Jatek.Application.Models;

public class GameHub : Hub
{
    private readonly ISender _mediator;
    private readonly ILogger<GameHub> _logger;

    public GameHub(ISender mediator, ILogger<GameHub> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public Task JoinGroup(string group)
    {
        _logger.LogDebug("signalR: {group} created", group);

        return Groups.AddToGroupAsync(Context.ConnectionId, group);
    }

    public Task LeaveRoom(string group)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
    }

    public async Task SendMessage(string group, string user, string message)
    {
        _logger.LogDebug("signalR: user {user} sent a message {message} in group: {group}", user, message, group);

        await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
    }

    public async Task StartGame(string group)
    {
        await _mediator.Send(new StartGameCommand(group));

        await Clients.Group(group).SendAsync("GameStarted");

        _logger.LogDebug("signalR: game started in group: {group}", group);
    }

    public async Task RightGuess(string group, string playerId)
    {
        await _mediator.Send(new RightGuessCommand(group, playerId));

        await Clients.Group(group).SendAsync("RecieveRightGuesses");

        _logger.LogDebug("signalR: player{playerId} guessed right in group: {group}", playerId, group);
    }

    public async Task GetWord(string group)
    {
        var word = await _mediator.Send(new GetWordQuery());

        await Clients.Group(group).SendAsync("RecieveWord", word);

        _logger.LogDebug("signalR: next word {word} in group: {group}", word, group);
    }

    public async Task GetGroupsPlayers(string group)
    {
        var data = await _mediator.Send(new GetGameDataQuery(group));

        await Clients.Group(group).SendAsync("RecievePlayers", data.Players);

        _logger.LogDebug("signalR: players {players} in group: {group}", data.Players, group);
    }

    public async Task GetRounds(string group)
    {
        var data = await _mediator.Send(new GetGameDataQuery(group));

        await Clients.Group(group).SendAsync("RecieveRounds", data.Rounds);

        _logger.LogDebug("signalR: reamaining rounds in group: {group} {rounds}", group, data.Rounds);
    }

    public async Task NextPlayer(string group)
    {
        var roundOver = await _mediator.Send(new GetNextPlayerQuery(group));

        await Clients.Group(group).SendAsync("RecieveRoundOver", roundOver);

        _logger.LogDebug("signalR: player change in group: {group}", group);
    }

    public async Task GameOver(string group)
    {
        var winner = await _mediator.Send(new GetWinnerQuery(group));

        await _mediator.Send(new RemoveRoomCommand(group));

        await Clients.Group(group).SendAsync("RecieveWinnerOnGameOver", winner.Tie, winner.Points, winner.Winners);

        _logger.LogDebug("signalR: game over in group: {group} winner: {winner} point: {point}",
            group,
            winner.Winners,
            winner.Points);
    }

    public async Task UpdateCanvas(string group, int prevX, int prevY, int currX, int currY, int width, string color)
    {
        await Clients.Group(group).SendAsync("RecieveUpdateCanvas", prevX, prevY, currX, currY, width, color);
    }

    public async Task ClearCanvas(string group)
    {
        await Clients.Group(group).SendAsync("RecieveClearCanvas");

        _logger.LogDebug("signalR: canvas clear in group: {group}", group);
    }
}
