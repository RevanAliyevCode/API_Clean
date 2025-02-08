using System;
using API.Application.Features.User.Dtos;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.User.Query.GetUsers;

public class GetUsersQueryRequest : IRequest<ResponseSuccess<List<UserDTO>>>
{

}
