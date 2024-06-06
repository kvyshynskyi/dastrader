using DAS.Trader.IntegrationClient.Enums;
using DAS.Trader.IntegrationClient.Interfaces;
using DAS.Trader.IntegrationClient.Response;

namespace DAS.Trader.IntegrationClient.Commands.TcpCommands
{
    public sealed class LoginCommand : BaseTcpCommand
    {
        public LoginCommand(string login, string password, string account) : base(TraderCommandType.LOGIN_COMMAND,
            true, false, login, password, account)
        {
        }

        public override void Subscribe(IResponseProcessor responseProcessor)
        {
            responseProcessor.LoginResponse += ResponseProcessor_LoginResponse;
        }

        private void ResponseProcessor_LoginResponse(object? sender, ResponseEventArgs e)
        {
            Result = e.Parameters?.Length == 1 ? e.Parameters[0] : e.Message ?? string.Empty;

            HasResult = true;
        }

        public override void Unsubscribe(IResponseProcessor responseProcessor)
        {
            responseProcessor.LoginResponse -= ResponseProcessor_LoginResponse;
        }
    }
}