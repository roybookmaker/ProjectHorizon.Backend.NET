using MassTransit;
using ProjectHorizon.Microservice.Account.Components.Models;
using ProjectHorizon.Microservice.Account.Components.Repositories;
using ProjectHorizon.Shared.Library.Common;

namespace ProjectHorizon.Microservice.Account.Components.Consumers
{
    public class AccountConsumer : IConsumer<LoginModel>, IConsumer<RegisterModel>, IConsumer<RecoveryModel>
    {
        public IAccountRepository _accountRepository;

        public AccountConsumer(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Consume(ConsumeContext<LoginModel> context)
        {
            var loginProcess = await _accountRepository.LoginUser(context.Message);
            await context.RespondAsync(loginProcess);
            return;
        }

        public async Task Consume(ConsumeContext<RegisterModel> context)
        {
            var registerProcess = await _accountRepository.RegisterUser(context.Message);
            await context.RespondAsync(registerProcess);
            return;
        }

        public async Task Consume(ConsumeContext<RecoveryModel> context)
        {
            var recoveryProcess = await _accountRepository.RecoveryUser(context.Message);
            await context.RespondAsync(recoveryProcess);
            return;
        }
    }
}
