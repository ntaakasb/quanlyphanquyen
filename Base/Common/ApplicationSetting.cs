namespace Base.Common
{
    public class ApplicationSetting
    {
        public string WebAppId { get; set; }

        public string WebAppPassWord { get; set; }

        public int MaximumUserImportParallelPartitionsNumber { get; set; }
        public int UserImportBatchAmount { get; set; }
        public int MaximumUploadRecords { get; set; }
        public string EmailUrl { get; set; }
    }
}