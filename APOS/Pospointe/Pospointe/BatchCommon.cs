using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe
{
   
        class BatchCommon
        {
            //Request---------------------------------------------------------
            public static readonly string[] BatchCloseReqButtonNames = new string[]
            {
            "Normal",
            "Multi Merchant"
            };

            public static readonly string[,] BatchCloseReqNormal = new string[,]
            {
            { "TimeStamp", "Time Stamp"}
            };

            public static readonly string[,] ForceBatchCloseReqNormal = new string[,]
            {
            { "TimeStamp", "Time Stamp"}
            };

            public static readonly string[,] BatchClearReqNormal = new string[,]
            {
            { "EdcType", "EDC Type"}
            };

            public static readonly string[,] PurgeBatchReqNormal = new string[,]
            {
            { "TimeStamp", "Time Stamp"}
            };

            public static readonly string[,] SafUploadReqNormal = new string[,]
            {
            { "SafIndicator", "SAF Indicator"}
            };

            public static readonly string[,] DeleteSafFileReqNormal = new string[,]
            {
            { "SafIndicator", "SAF Indicator"}
            };

            public static readonly string[,] DeleteTransReqNormalComboBox = new string[,]
            {
            {"EdcType", "EDC Type" },
            {"TransactionType", "Transaction Type" },
            {"CardType", "Card Type" }
            };

            public static readonly string[,] DeleteTransReqNormal = new string[,]
    {
            {"RecordNumber", "Record Number" },
            {"OrigRefNum", "Orig Ref Num" },
            {"AuthCode", "Auth Code" },
            {"EcrRefNum", "ECR Ref Num" },
    };

            //Response------------------------------------------------------
            public static readonly string[] BatchCloseRspButtonNames = new string[]
            {
            "Normal",
            "Host",
            "TOR",
            "Multi Merchant"
            };

            public static readonly string[] ForceBatchCloseRspButtonNames = new string[]
            {
            "Normal",
            "Host",
            "TOR"
            };

            public static readonly string[] BatchClearRspButtonNames = new string[]
            {
            "Normal",
            "TOR"
            };

            public static readonly string[] PurgeBatchRspButtonNames = new string[]
            {
            "Normal",
            "Host",
            "TOR"
            };

            public static readonly string[] SafUploadRspButtonNames = new string[]
            {
            "Normal",
            "TOR"
            };

            public static readonly string[] DeleteSafFileRspButtonNames = new string[]
            {
            "Normal",
            "TOR"
            };

            public static readonly string[] DeleteTransRspButtonNames = new string[]
            {
            "Normal",
            "TOR"
            };

            public static readonly string[,] BatchCloseRspNormal = new string[,]
            {
            { "ResponseCode", "Response Code" },
            { "ResponseMessage", "Response Message" },
            { "TotalCount", "Total Count" },
            { "TotalAmount", "Total Amount" },
            { "TimeStamp", "Time Stamp" },
            { "Tid", "TID" },
            { "Mid", "MID" },
            { "FailedTransNO", "Failed Trans Nunber" },
            { "FailedCount", "Failed Count" },
            { "SafFailedCount", "SAF Failed Count" },
            { "SafFailedTotal", "SAF Failed Total" }
            };

            public static readonly string[,] ForceBatchCloseRspNormal = new string[,]
            {
            { "ResponseCode", "Response Code" },
            { "ResponseMessage", "Response Message" },
            { "LineNumber", "Line Number" },
            { "LinesMessage", "Lines Message" },
            { "TimeStamp", "Time Stamp" },
            { "Tid", "TID" },
            { "Mid", "MID" },
            { "FailedTransNO", "Failed Trans Nunber" },
            { "FailedCount", "Failed Count" },
            { "SafFailedCount", "SAF Failed Count" },
            { "SafFailedTotal", "SAF Failed Total" }
            };

            public static readonly string[,] BatchClearRspNormal = new string[,]
            {
            { "ResponseCode", "Response Code" },
            { "ResponseMessage", "Response Message" },
            };

            public static readonly string[,] PurgeBatchRspNormal = new string[,]
            {
            { "ResponseCode", "Response Code" },
            { "ResponseMessage", "Response Message" },
            { "LineNumber", "Line Number" },
            { "LinesMessage", "Lines Message" },
            { "TimeStamp", "Time Stamp" },
            { "Tid", "TID" },
            { "Mid", "MID" }
            };

            public static readonly string[,] SafUploadRspNormal = new string[,]
            {
            { "ResponseCode", "Response Code" },
            { "ResponseMessage", "Response Message" },
            { "SafTotalCount", "SAF Total Count" },
            { "SafTotalAmount", "SAF Total Amount" },
            { "TimeStamp", "Time Stamp" },
            { "SafUploadedCount", "SAF Uploaded Count" },
            { "SafUploadedAmount", "SAF Uploaded Amount" },
            { "SafFailedCount", "SAF Failed Count" },
            { "SafFailedTotal", "SAF Failed Total" }
            };

            public static readonly string[,] DeleteSafFileRspNormal = new string[,]
            {
            { "ResponseCode", "Response Code" },
            { "ResponseMessage", "Response Message" },
            { "SafDeletedCount", "SAF Deleted Count" }
            };

            public static readonly string[,] DeleteTransRspNormal = new string[,]
            {
            { "ResponseCode", "Response Code" },
            { "ResponseMessage", "Response Message" }
            };

            public static readonly string[,] HostInfoRsp = new string[,]
            {
            { "HostResponseCode", "Host Response Code" },
            { "HostResponseMessage", "Host Response Message" },
            { "AuthCode", "Auth Code" },
            { "HostRefNum", "Host Ref Num" },
            { "TraceNumber", "Trace Number" },
            { "BatchNumber", "Batch Number" },
            { "TransactionIdentifier", "Transaction Identifier" },
            { "GatewayTransactionID", "Gateway Transaction ID" },
            };

            public static readonly string[,] TorInfoRsp = new string[,]
            {
            { "RecordType", "Record Type" },
            { "ReversalTimeStamp", "Reversal Time Stamp" },
            { "HostResponseCode", "Host Response Code" },
            { "HostResponseMessage", "Host Response Message" },
            { "HostRefNum", "Host Ref Num" },
            { "GatewayTransactionID", "Gateway Transaction ID" },
            { "OrigAmount", "Orig Amount" },
            { "MaskedPan", "Masked PAN" },
            { "BatchNo", "Batch Number" },
            { "ReversalAuthCode", "Reversal Auth Code" },
            { "OrigTransType", "Orig Trans Type" },
            { "OrigTransDateTime", "Orig Trans Date Time" },
            { "OrigTransAuthCode", "Orig Trans Auth Code" },
            };

            //ExtData-------------------------------------------------------
            public static readonly string[,] MultiMerchantNames = new string[,]
            {
            { "MultiMerchantId", "Multi Merchant ID" },
            { "MultiMerchantName", "Multi Merchant Name" }
            };
        }
    }

