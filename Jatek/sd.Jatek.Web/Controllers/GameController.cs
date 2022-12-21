using MassTransit;
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

        [Route("CreateRoom")]
        public async Task<IActionResult> Index(string roomId, int rounds)
        {
            ViewBag.RoomId = roomId;
            ViewBag.Rounds = rounds;
            return View();
        }
        /*
        public async Task<IActionResult> Index(int rounds)
        {
            ViewBag.Rounds = rounds;
            var roomId = Guid.NewGuid().ToString();

            var userId = Request.Cookies["UserId"];

            var created = await _mediator.Send(new CreateRoomCommand(
                new RoomDto
                {
                    PlayerId = userId,
                    RoomId = roomId,
                    PlayerName = Request.Cookies["UserName"],
                    Rounds = rounds,
                }, true));

            ViewBag.RoomId = created == "" ? roomId : created;
            return View();
        }
         */

        [Route("EnterRoom")]
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

        [HttpPost]
        [Route("StartGame")]
        public async Task<IActionResult> StartGame(StartGameViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var roomId = Guid.NewGuid().ToString();

                var userId = Request.Cookies["UserId"];

                var created = await _mediator.Send(new CreateRoomCommand(
                    new RoomDto
                    {
                        PlayerId = userId,
                        RoomId = roomId,
                        PlayerName = Request.Cookies["UserName"],
                        Rounds = viewModel.Rounds,
                    }, true));

                return Redirect("https://localhost:5005/Game/CreateRoom?roomId=" + roomId + "&Rounds=" + viewModel.Rounds);
            }
            return View(viewModel);
        }

        [HttpGet]
        [Route("GetRole")]
        public async Task<string> GetRole(string id)
        {
            return await _mediator.Send(new GetRoleByIdQuery(Request.Cookies["UserId"], id));
        }


        [HttpGet]
        [Route("GetPainterFinished")]
        public async Task<string> GetPainterFinished(string id)
        {
            return await _mediator.Send(new GetPainterFinishedQuery(id));
        }

        [HttpGet]
        [Route("GetRoundOver")]
        public async Task<string> GetRoundOver(string id)
        {
            return await _mediator.Send(new GetRoundOverQuery(id));
        }

        [HttpGet]
        [Route("StartGame")]
        public IActionResult StartGame()
        {
            return View();
        }


        [HttpPost]
        [Route("Send")]
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