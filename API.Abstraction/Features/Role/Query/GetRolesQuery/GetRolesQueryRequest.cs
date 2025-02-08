using System;
using API.Application.Features.Role.Dtos;
using API.Application.Wrappers;
using MediatR;

namespace API.Application.Features.Role.Query.GetRolesQuery;

public class GetRolesQueryRequest : IRequest<ResponseSuccess<List<RoleDTO>>>
{

}
