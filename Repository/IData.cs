using Food_Application.Models;
using System.Security.Claims;

namespace Food_Application.Repository
{
    public interface IData
    {
        Task<ApplicationUser> GetUser(ClaimsPrincipal claims);
    }
}
