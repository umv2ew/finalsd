﻿using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using sd.Jatek.Application.Commands;
using sd.Jatek.Application.Dtos;
using sd.Jatek.Application.Querys;
using sd.Jatek.Integration;
using sd.Jatek.Web.Models;
using System.Diagnostics;

namespace sd.Jatek.Web.Controllers
{
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        readonly IPublishEndpoint _publishEndpoint;
        private readonly ISender _mediator;

        public GameController(ILogger<GameController> logger, IPublishEndpoint publishEndpoint, ISender mediator)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _mediator = mediator;
        }

        [HttpGet("CreateRoom")]
        public async Task<IActionResult> Index(string roomId, int rounds)
        {
            ViewBag.RoomId = roomId;
            ViewBag.Rounds = rounds;
            return View();
        }

        [HttpGet("EnterRoom")]
        public async Task<IActionResult> EnterRoom(string roomId)
        {
            ViewBag.RoomId = roomId;
            var userId = Request.Cookies["UserId"];

            var roomEntered = await _mediator.Send(new EnterRoomCommand(
                    roomId,
                    userId,
                    Request.Cookies["UserName"]));

            if (!roomEntered)
            {
                return RedirectToAction("StartGame");
            }
            return View("Index");
        }

        [HttpGet("StartGame")]
        public async Task<IActionResult> StartGame()
        {
            ViewData["rooms"] = await _mediator.Send(new GetPublicRoomsQuery());

            return View();
        }

        [HttpPost("StartGame")]
        public async Task<IActionResult> StartGame(GameViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var roomId = Guid.NewGuid().ToString();

                var userId = Request.Cookies["UserId"];

                await _mediator.Send(new CreateRoomCommand(
                    new RoomDto
                    {
                        PlayerId = userId,
                        RoomId = roomId,
                        PlayerName = Request.Cookies["UserName"],
                        Rounds = viewModel.Rounds,
                        IsPublic = viewModel.Public,
                    }, true));

                return Redirect("https://localhost:5005/Game/CreateRoom?roomId=" + roomId + "&Rounds=" + viewModel.Rounds);
            }

            ViewData["rooms"] = await _mediator.Send(new GetPublicRoomsQuery());

            return View(viewModel);
        }

        [HttpGet("GetRole")]
        public async Task<string> GetRole(string id)
        {
            return await _mediator.Send(new GetRoleByIdQuery(Request.Cookies["UserId"], id));
        }


        [HttpGet("GetPainterFinished")]
        public async Task<string> GetPainterFinished(string id)
        {
            return await _mediator.Send(new GetPainterFinishedQuery(id));
        }

        [HttpGet("RemovePlayer")]
        public async Task<IActionResult> RemovePlayer()
        {
            await _mediator.Send(new RemovePlayerCommand(Request.Cookies["UserId"]));

            return Redirect("https://localhost:7187/Game/StartGame");
        }

        [HttpPost("Send")]
        public async Task<JsonResult> Post([FromBody] StatisticsIntegrationDto dto)
        {
            await _publishEndpoint.Publish(dto);

            return Json("Statistics updated");
        }

        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}