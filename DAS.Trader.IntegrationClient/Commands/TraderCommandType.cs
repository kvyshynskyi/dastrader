using System.ComponentModel;
using DAS.Trader.IntegrationClient.Common;

namespace DAS.Trader.IntegrationClient.Commands;

public enum TraderCommandType
{
    None = 0,

    /// <summary>
    ///     LOGIN
    ///     The client sends login to the server
    /// </summary>
    [Description("LOGIN")] LOGIN_COMMAND = 1,

    /// <summary>
    ///     #LOGIN
    ///     The client sends login Occured
    /// </summary>
    [Description("#LOGIN")] [ParametersCount(3)]
    LOGIN_RESPONSE,

    /// <summary>
    ///     #POS
    ///     Mark the start when the server sends position list to
    ///     the client.
    /// </summary>
    [Description("#POS")] POS_BEGIN_RESPONSE,

    /// <summary>
    ///     #POSEND
    ///     Mark the end when the server sends position list to
    ///     the client
    /// </summary>
    [Description("#POSEND")] POS_END_RESPONSE,

    /// <summary>
    ///     %POS
    ///     Position Details
    /// </summary>
    [Description("%POS")] POS_RESPONSE,

    /// <summary>
    ///     POSREFRESH
    ///     User can use this command to Query all positions.
    /// </summary>
    [Description("POSREFRESH")] POSREFRESH_COMMAND,

    /// <summary>
    ///     #Order
    ///     Mark the start when the server sends order list to
    ///     the client.
    /// </summary>
    [Description("#Order")] ORDER_BEGIN_RESPONSE,

    /// <summary>
    ///     #OrderEnd
    ///     Mark the end when the server sends order list to
    ///     the client.
    /// </summary>
    [Description("#OrderEnd")] ORDER_END_RESPONSE,

    /// <summary>
    ///     %ORDER
    ///     Order details
    /// </summary>
    [Description("%ORDER")] ORDER_RESPONSE,

    /// <summary>
    ///     %OrderAct
    ///     Order’s action message.
    /// </summary>
    [Description("%OrderAct")] ORDER_ACTION_MESSAGE_RESPONSE,

    /// <summary>
    ///     #Trade
    ///     Mark the start when the server sends trade list to
    ///     the client.
    /// </summary>
    [Description("#Trade")] TRADE_BEGIN_RESPONSE,

    /// <summary>
    ///     #TradeEnd
    ///     Mark the end when the server sends trade list to
    ///     the client.
    /// </summary>
    [Description("#TradeEnd")] TRADE_END_RESPONSE,

    /// <summary>
    ///     %TRADE
    ///     Trade details
    /// </summary>
    [Description("%TRADE")] TRADE_RESPONSE,

    /// <summary>
    ///     NEWORDER
    ///     Place new order.
    /// </summary>
    [Description("NEWORDER")] NEWORDER_COMMAND,

    /// <summary>
    ///     CANCEL
    ///     Cancel an order or all open orders
    /// </summary>
    [Description("CANCEL")] CANCEL_COMMAND,

    /// <summary>
    ///     GET BP
    ///     Get current buying power of login account.
    /// </summary>
    [Description("GET BP")] GET_BUYING_POWER_COMMAND,

    /// <summary>
    ///     BP
    ///     The server sends the account’s current day buying
    ///     power and overnight buying power of the account.
    /// </summary>
    [Description("BP")] BUYING_POWER_RESPONSE,

    /// <summary>
    ///     GET SHORTINFO
    ///     Get a symbol’s short info, including shortable,
    ///     short size, marginable, long/short margin rate.
    /// </summary>
    [Description("GET SHORTINFO")] GET_SHORTINFO_COMMAND,

    /// <summary>
    ///     $SHORTINFO
    ///     This is returned symbol shortable info queried by GET SHORTINFO
    /// </summary>
    [Description("$SHORTINFO")] SHORTINFO_RESPONSE,

    /// <summary>
    ///     SB
    ///     Subscribe symbol quote data
    /// </summary>
    [Description("SB")] SB_COMMAND,

    /// <summary>
    ///     UNSB
    ///     Unsubscribe symbol quote data
    /// </summary>
    [Description("UNSB")] UNSB_COMMAND,

    /// <summary>
    ///     $Quote
    ///     Symbol’s level1 quote data.
    /// </summary>
    [Description("$Quote")] QUOTE_RESPONSE,

    /// <summary>
    ///     $T&S
    ///     Symbol’s time/sale quote data.
    /// </summary>
    [Description("$T&S")] TS_RESPONSE,

    /// <summary>
    ///     $Lv2
    ///     Symbol’s level2 quote data.
    /// </summary>
    [Description("$Lv2")] LV2_RESPONSE,

    /// <summary>
    ///     $BAR
    ///     Day/Minute chart data.
    /// </summary>
    [Description("$BAR")] BAR_RESPONSE,

    /// <summary>
    ///     $LDLU
    ///     Limit Down Price/Limit Up Price
    /// </summary>
    [Description("$LDLU")] LDLU_RESPONSE,

    /// <summary>
    ///     %IORDER
    ///     Same definition with %ORDER, but for watch
    ///     connection.
    /// </summary>
    [Description("%IORDER")] IORDER_RESPONSE,

    /// <summary>
    ///     %IPOS
    ///     Same definition with %POS, but for watch
    ///     connection.
    /// </summary>
    [Description("%IPOS")] IPOS_RESPONSE,

    /// <summary>
    ///     %ITRADE
    /// </summary>
    [Description("%ITRADE")] ITRADE_RESPONSE,

    /// <summary>
    ///     ECHO
    ///     The server will return ECHO on/off status.
    /// </summary>
    [Description("ECHO")] ECHO_COMMAND,

    /// <summary>
    ///     CLIENT
    ///     The server will return connected client numbers.
    /// </summary>
    [Description("CLIENT")] CLIENT_COMMAND,

    /// <summary>
    ///     Order Server connection status
    ///     Does not work
    /// </summary>
    [Description("Order Server connection status")]
    ORDER_SERVER_CONNECTION_STATUS_COMMAND,

    /// <summary>
    ///     Order Server logon status
    ///     Does not work
    /// </summary>
    [Description("Order Server logon status")]
    ORDER_SERVER_LOGON_STATUS_COMMAND,

    /// <summary>
    ///     Quote Server connection status
    ///     Does not work
    /// </summary>
    [Description("Quote Server connection status")]
    QUOTE_SERVER_CONNECTION_STATUS_COMMAND,

    /// <summary>
    ///     Quote Server logon status
    ///     Does not work
    /// </summary>
    [Description("Quote Server logon status")]
    QUOTE_SERVER_LOGON_STATUS_COMMAND,

    /// <summary>
    ///     Order Server connection status
    /// </summary>
    [Description("#OrderServer:Connect:")] ORDER_SERVER_CONNECTION_STATUS_RESPONSE,

    /// <summary>
    ///     Order Server logon status
    /// </summary>
    [Description("#OrderServer:Logon:")] ORDER_SERVER_LOGON_STATUS_RESPONSE,

    /// <summary>
    ///     Quote Server connection status
    /// </summary>
    [Description("#QuoteServer:Connect:")] QUOTE_SERVER_CONNECTION_STATUS_RESPONSE,

    /// <summary>
    ///     Quote Server logon status
    /// </summary>
    [Description("#QuoteServer:Logon:")] QUOTE_SERVER_LOGON_STATUS_RESPONSE,

    /// <summary>
    ///     SCRIPT
    /// </summary>
    [Description("SCRIPT")] SCRIPT_COMMAND,

    //SCRIPT............ 10
    /// <summary>
    ///     QUIT
    ///     Disconnect from the server.
    /// </summary>
    [Description("QUIT")] QUIT_COMMAND,

    /// <summary>
    ///     SLPRICEINQUIRE
    ///     Inquire related locate price.
    /// </summary>
    [Description("SLPRICEINQUIRE")] SLPRICEINQUIRE_COMMAND,

    /// <summary>
    ///     SLNEWORDER
    ///     Place a short locate order.
    /// </summary>
    [Description("SLNEWORDER")] SLNEWORDER_COMMAND,

    /// <summary>
    ///     SLCANCELORDER
    ///     Cancel an open locate order.
    /// </summary>
    [Description("SLCANCELORDER")] SLCANCELORDER_COMMAND,

    /// <summary>
    ///     SLOFFEROPERATION
    ///     Accept or reject an offered locate order.
    /// </summary>
    [Description("SLOFFEROPERATION")] SLOFFEROPERATION_COMMAND,

    /// <summary>
    ///     %SLRET
    ///     This command returns the price inquire result for command
    ///     SLPRICEINQUIRE and SLNEWORDER failure (already
    ///     shortable etc.)
    /// </summary>
    [Description("%SLRET")] [ParametersCount(5)]
    SLRET_RESPONSE,

    /// <summary>
    ///     #SLOrder
    ///     Mark the start when the server sends
    ///     short locate order list to the client.
    /// </summary>
    [Description("#SLOrder")] [ParametersCount(10)]
    SLORDER_BEGIN_RESPONSE,

    /// <summary>
    ///     #SLOrderEnd (# LOrderEnd)
    ///     Mark the end when the server sends
    ///     short locate order list to the client.
    /// </summary>
    [Description("#SLOrderEnd")] [ParametersCount(10)]
    SLORDER_END_RESPONSE,

    /// <summary>
    ///     %SLOrder
    ///     Short locate order. Order server will
    ///     return this message when short locate
    ///     order action changed.
    /// </summary>
    [Description("%SLOrder")] [ParametersCount(10)]
    SLORDER_RESPONSE,

    /// <summary>
    ///     SLAvailQuery
    ///     Query available locate shares to short.
    /// </summary>
    [Description("SLAvailQuery")] SLAVAILQUERY_COMMAND,

    /// <summary>
    ///     $SLAvailQueryRet
    ///     Query result for SLAvailQuery.
    /// </summary>
    [Description("$SLAvailQueryRet")] SLAVAILQUERY_RET_RESPONSE
}