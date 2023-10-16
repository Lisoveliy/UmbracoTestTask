namespace UmbracoTestTask.Services
{
    public interface ISOAPService
    {
        public Task<string> GetLoginResponse(string login, string password);
        public bool CheckLogged(string answer);
    }
}
