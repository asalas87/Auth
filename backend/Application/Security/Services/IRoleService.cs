using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Security.Common.DTOS;
using ErrorOr;

namespace Application.Security.Services
{
    public interface IRoleService
    {
        Task<ErrorOr<List<RoleDTO>>> GetRolesAsync();
    }
}
