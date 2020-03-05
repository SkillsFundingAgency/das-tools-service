using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using System.Threading.Tasks;

namespace SFA.DAS.ToolService.Infrastructure.Auth0
{
    public interface IAuth0ApiClient
    {
        Task<IPagedList<Role>> GetAuth0Roles();
    }
}
