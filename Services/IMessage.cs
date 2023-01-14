namespace IdentityApp.Services
{
    public interface IMessage
    {
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
