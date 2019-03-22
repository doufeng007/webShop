namespace EmailServer
{
    public class SendEmailInput 
    {
        #region 表字段

        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        #endregion
    }
}