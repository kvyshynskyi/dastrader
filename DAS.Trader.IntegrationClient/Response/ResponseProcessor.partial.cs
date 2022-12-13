using System.Text;
using DAS.Trader.IntegrationClient.Commands;
using DAS.Trader.IntegrationClient.Common;

namespace DAS.Trader.IntegrationClient.Response;

public partial class ResponseProcessor
{
    protected class WrapResponseEventArgs : EventArgs
    {
        public WrapResponseEventArgs(StringBuilder responseStringBuilder, Guid correlationId)
        {
            ResponseStringBuilder = responseStringBuilder;
            CorrelationId = correlationId;
        }

        public StringBuilder ResponseStringBuilder { get; }
        public Guid CorrelationId { get; }
    }

    protected class ResponseWrapper
    {
        internal static void WrapResponse(object? sender, WrapResponseEventArgs e)
        {
            if (sender == null)
                return;

            //var value = Random.Shared.Next(1,10);
            //Thread.Sleep(TimeSpan.FromSeconds(value));

            var rp = (ResponseProcessor)sender;

            foreach (var line in e.ResponseStringBuilder.ToString()
                         .Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
            {
                ProceedLine(rp, line, e.CorrelationId);
                //Console.WriteLine($"|<--|    {line}, thread was sleep for {value} second(s)");
                Console.WriteLine($"|<-| {e.CorrelationId.ToString().ToUpper()} |<-| {line}");
            }
        }

        private static string[]? GetParams(string value, TraderCommandType commandType)
        {
            var paramsCount = commandType.GetParamsCount()!.Value;
            var stringPattern = commandType.GetDescription();
            var separator = new[] { ' ' };

            return value.Replace(stringPattern!, string.Empty)
                .Split(separator, paramsCount, StringSplitOptions.RemoveEmptyEntries);
        }

        private static void ProceedLine(ResponseProcessor rp, string line, Guid correlationId)
        {
            ResponseEventArgs args;
            const StringComparison ignoreCase = StringComparison.InvariantCultureIgnoreCase;

            var loginResponse = TraderCommandType.LOGIN_RESPONSE;
            var slPriceResponse = TraderCommandType.SLRET_RESPONSE;
            var slOrderEndResponse = TraderCommandType.SLORDER_END_RESPONSE;
            var slOrderBeginResponse = TraderCommandType.SLORDER_BEGIN_RESPONSE;
            var slOrderResponse = TraderCommandType.SLORDER_RESPONSE;

            switch (line)
            {
                case { } when line.StartsWith(loginResponse.GetDescription()!, ignoreCase):
                    args = GetResponseEventArgs(line, correlationId, loginResponse);
                    break;
                case { } when line.StartsWith(slPriceResponse.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(line, correlationId, slPriceResponse);
                    break;
                case { } when line.StartsWith(slOrderEndResponse.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(line, correlationId, slOrderEndResponse);
                    break;
                case { } when line.StartsWith(slOrderBeginResponse.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(line, correlationId, slOrderBeginResponse);
                    break;
                case { } when line.StartsWith(slOrderResponse.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(line, correlationId, slOrderResponse);
                    break;

                default:
                    return;
            }

            rp.RiseEvent(args);
            //Console.WriteLine($"|<<<|    Event detected: {args.CommandType} |<<<|    With {args.Parameters.Length} params |<<<|    {string.Join(", ", args.Parameters)}");
        }

        private static ResponseEventArgs GetResponseEventArgs(string value, Guid correlationId,
            TraderCommandType commandType)
        {
            return new ResponseEventArgs(correlationId, commandType, value, GetParams(value, commandType));
        }
    }
}