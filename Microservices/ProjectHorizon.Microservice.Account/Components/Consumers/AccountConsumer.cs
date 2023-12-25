using MassTransit;
using ProjectHorizon.Microservice.Account.Components.Models;
using ProjectHorizon.Microservice.Account.Components.Repositories;
using ProjectHorizon.Shared.Library.Common;

namespace ProjectHorizon.Microservice.Account.Components.Consumers
{
    public class AccountConsumer : IConsumer<LoginModel>, IConsumer<RegisterModel>, IConsumer<RecoveryModel>, IConsumer<ResetModel>
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

        public async Task Consume(ConsumeContext<ResetModel> context)
        {
            var resetProcess = await _accountRepository.ResetUserPass(context.Message);
            await context.RespondAsync(resetProcess);
            return;
        }
    }
}
