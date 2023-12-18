using ProjectHorizon.Microservice.Account.Components.DTO;
using ProjectHorizon.Microservice.Account.Components.Models;
using ProjectHorizon.Shared.Library.Common;

namespace ProjectHorizon.Microservice.Account.Components.Repositories
{
    public interface IAccountRepository
    {
        public Task<QueryResult> RegisterUser(RegisterModel model);

        public Task<QueryResult> LoginUser(LoginModel model);

        public Task<QueryResult> RecoveryUser(RecoveryModel model);
    }
}
