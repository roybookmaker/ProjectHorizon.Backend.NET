using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ProjectHorizon.Shared.Library.Base;
using ProjectHorizon.Shared.Library.Common;

namespace ProjectHorizon.Microservice.Account.Components.Base
{
    public class BaseController : ControllerBase
    {
        private readonly IDependencies Dependencies;
        public BaseController(IDependencies dependencies)
        {
            Dependencies = dependencies;
        }

        protected async Task<ActionResult> GetCommandResultResponse<T>(T command, bool remote = false, int timeout = 300) where T : AuthInfo
        {
            var validation = Validate(command);
            if (validation.IsSuccessful)
            {
                //ApplyAuthenticationInformation(command);
                var rto = RequestTimeout.After(s: timeout);

                try
                {
                    var client = remote
                        ? Dependencies.ClientFactory.CreateRequestClient<T>(timeout: rto)
                        : Dependencies.Mediator.CreateRequestClient<T>(timeout: rto);

                    var response = await client.GetResponse<CommandResult>(command);
                    return response.Message.IsSuccessful
                        ? new OkObjectResult(response.Message)
                        : new BadRequestObjectResult(response.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new BadRequestObjectResult(ex.Message);
                }
            }

            return new BadRequestObjectResult(validation);
        }

        protected async Task<ActionResult> GetQueryResultResponse<T>(T query, bool remote = false) where T : class
        {
            try
            {
                var client = remote ? Dependencies.ClientFactory.CreateRequestClient<T>() : Dependencies.Mediator.CreateRequestClient<T>();
                var response = await client.GetResponse<QueryResult>(query);
                return response.Message.IsSuccessful
                    ? new OkObjectResult(response.Message)
                    : new BadRequestObjectResult(response.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        protected CommandResult Validate<T>(T command) where T : class
        {
            try
            {
                //Validation Logic
                return CommandResult.Ok();
            }
            catch (Exception ex)
            {
                return CommandResult.Error($"Validator Error\n\nMore detail :\n{ex.Message}");
            }
        }
    }
}
