using System;
using Business.Features.Role.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Role.Query.GetRolesQuery;

public class GetRolesQueryRequest : IRequest<ResponseSuccess<List<RoleDTO>>>
{

}
