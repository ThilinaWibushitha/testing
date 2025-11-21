using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pospointe
{
    class BatchData
    {
        //Request-----------------------------------------
        public string[] BatchCloseReqNormalData { get; set; }
        public string[] ForceBatchCloseReqNormalData { get; set; }
        public string[] PurgeBatchReqNormalData { get; set; }
        public string[] SafUploadReqNormalData { get; set; }
        public string[] DeleteSafFileReqNormalData { get; set; }
        public string[] DeleteTransReqNormalData { get; set; }
        public string[] MultiMerchantReqData { get; set; }
        public int EdcTypeIndex { get; set; }
        public int TransTypeIndex { get; set; }
        public int CardTypeIndex { get; set; }
        //Response--------------------------------------
        public string[] BatchCloseRspNormalData { get; set; }
        public string[] ForceBatchCloseRspNormalData { get; set; }
        public string[] BatchClearRspNormalData { get; set; }
        public string[] PurgeBatchRspNormalData { get; set; }
        public string[] SafUploadRspNormalData { get; set; }
        public string[] DeleteSafFileRspNormalData { get; set; }
        public string[] DeleteTransRspNormalData { get; set; }
        public string[] HostInfoRspData { get; set; }
        public string[] TorInfoRspData { get; set; }
        public string[] MultiMerchantRspData { get; set; }

        private static BatchData _batchData;
        private BatchData()
        {
            RequestClear();
            ResponseClear();
        }
        public static BatchData GetBatchData()
        {
            if (_batchData == null)
            {
                _batchData = new BatchData();
            }
            return _batchData;
        }

        public void RequestClear()
        {
            BatchCloseReqNormalData = new string[BatchCommon.BatchCloseReqNormal.Length / 2];
            ForceBatchCloseReqNormalData = new string[BatchCommon.ForceBatchCloseReqNormal.Length / 2];
            PurgeBatchReqNormalData = new string[BatchCommon.PurgeBatchReqNormal.Length / 2];
            SafUploadReqNormalData = new string[BatchCommon.SafUploadReqNormal.Length / 2];
            DeleteSafFileReqNormalData = new string[BatchCommon.DeleteSafFileReqNormal.Length / 2];
            DeleteTransReqNormalData = new string[BatchCommon.DeleteTransReqNormal.Length / 2];
            MultiMerchantReqData = new string[BatchCommon.MultiMerchantNames.Length / 2];
            EdcTypeIndex = 0;
            TransTypeIndex = 0;
            CardTypeIndex = 0;
        }

        public void ResponseClear()
        {
            BatchCloseRspNormalData = new string[BatchCommon.BatchCloseRspNormal.Length / 2];
            ForceBatchCloseRspNormalData = new string[BatchCommon.ForceBatchCloseRspNormal.Length / 2];
            BatchClearRspNormalData = new string[BatchCommon.BatchClearRspNormal.Length / 2];
            PurgeBatchRspNormalData = new string[BatchCommon.PurgeBatchRspNormal.Length / 2];
            SafUploadRspNormalData = new string[BatchCommon.SafUploadRspNormal.Length / 2];
            DeleteSafFileRspNormalData = new string[BatchCommon.DeleteSafFileRspNormal.Length / 2];
            DeleteTransRspNormalData = new string[BatchCommon.DeleteTransRspNormal.Length / 2];
            HostInfoRspData = new string[BatchCommon.HostInfoRsp.Length / 2];
            TorInfoRspData = new string[BatchCommon.TorInfoRsp.Length / 2];
            MultiMerchantRspData = new string[BatchCommon.MultiMerchantNames.Length / 2];
        }
    }
}
