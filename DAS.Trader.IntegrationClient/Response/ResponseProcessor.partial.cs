using System.Diagnostics;
using System.Text;
using DAS.Trader.IntegrationClient.Commands;
using DAS.Trader.IntegrationClient.Common;
using DAS.Trader.IntegrationClient.Enums;

namespace DAS.Trader.IntegrationClient.Response;

public partial class ResponseProcessor
{
    private class WrapResponseEventArgs : EventArgs
    {
        public WrapResponseEventArgs(StringBuilder responseStringBuilder, Guid correlationId)
        {
            ResponseStringBuilder = responseStringBuilder;
            CorrelationId = correlationId;
        }

        public StringBuilder ResponseStringBuilder { get; }
        public Guid CorrelationId { get; }
    }

    private static class ResponseWrapper
    {
        internal static void WrapResponse(object? sender, WrapResponseEventArgs e)
        {
            if (sender == null)
                return;

            var rp = (ResponseProcessor)sender;

            foreach (var line in e.ResponseStringBuilder.ToString()
                         .Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
            {
                ProceedLine(rp, line, e.CorrelationId);
                Debug.WriteLine($"|<-| {e.CorrelationId.ToString().ToUpper()} |<-| {line}");
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

        private static void ProceedLine(ResponseProcessor rp, string value, Guid correlationId)
        {
            ResponseEventArgs args;
            const StringComparison ignoreCase = StringComparison.InvariantCultureIgnoreCase;

            switch (value)
            {
                case { } when value.StartsWith(TraderCommandType.LOGIN_RESPONSE.GetDescription()!, ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.LOGIN_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.SLRET_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.SLRET_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.SLORDER_END_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.SLORDER_END_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.SLORDER_BEGIN_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.SLORDER_BEGIN_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.SLORDER_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.SLORDER_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.POS_END_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.POS_END_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.POS_BEGIN_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.POS_BEGIN_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.POS_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.POS_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.ORDER_END_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.ORDER_END_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.ORDER_BEGIN_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.ORDER_BEGIN_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.ORDER_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.ORDER_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.TRADE_END_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.TRADE_END_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.TRADE_BEGIN_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.TRADE_BEGIN_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.TRADE_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.TRADE_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.ORDER_ACTION_MESSAGE_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.ORDER_ACTION_MESSAGE_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.BUYING_POWER_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.BUYING_POWER_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.CLIENT_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.CLIENT_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.SHORTINFO_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.SHORTINFO_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.QUOTE_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.QUOTE_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.LDLU_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.LDLU_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.IPOS_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.IPOS_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.IORDER_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.IORDER_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.ITRADE_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.ITRADE_RESPONSE);
                    break;
                case { } when value.StartsWith(TraderCommandType.TS_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.TS_RESPONSE);
                    break;

                case { } when value.StartsWith(TraderCommandType.LV2_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.LV2_RESPONSE);
                    break;

                case { } when value.StartsWith(TraderCommandType.BAR_RESPONSE.GetDescription()!,
                    ignoreCase):
                    args = GetResponseEventArgs(value, correlationId, TraderCommandType.BAR_RESPONSE);
                    break;
                default:
                    return;
            }

            rp.RiseEvent(args);
            //Debug.WriteLine($"|<-| {args.CorrelationId.ToString().ToUpper()} |<<| Event detected: {args.CommandType} |<<| {args.Parameters.Length} params |<<| {string.Join(", ", args.Parameters)}");
        }

        private static ResponseEventArgs GetResponseEventArgs(string value, Guid correlationId,
            TraderCommandType commandType)
        {
            return new ResponseEventArgs(correlationId, commandType, value, GetParams(value, commandType));
        }
    }
}