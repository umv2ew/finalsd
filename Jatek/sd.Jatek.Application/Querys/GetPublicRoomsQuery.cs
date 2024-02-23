using MediatR;
using sd.Jatek.Application.ViewModels;

namespace sd.Jatek.Application.Querys;

public class GetPublicRoomsQuery : IRequest<List<PublicRoomsViewModel>> { }
