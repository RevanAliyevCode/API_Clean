using System;
using Business.Features.User.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.User.Query.GetUsers;

public class GetUsersQueryRequest : IRequest<ResponseSuccess<List<UserDTO>>>
{

}
