namespace notifyme.shared.Models
{
    public class VapidDetails
    {
        public string PublicKey { get; }
        public string PrivateKey { get; }
        public string MailTo { get; }

        public VapidDetails(string publicKey, string privateKey, string mailTo)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
            MailTo = mailTo;
        }
    }
}