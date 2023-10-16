using System.Text.Json;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Web;

namespace UmbracoTestTask.Services
{
    public class SOAPService : ISOAPService
    {
        public SOAPService() { }

        public async Task<string> GetLoginResponse(string login, string password)
        {

            var client = new IICU.ICUTechClient();
            var ans = await client.LoginAsync(login, password, "");
            return ans.@return;
        }
        public bool CheckLogged(string answer)
        {

            try
            {
                var jsonDoc = JsonDocument.Parse(answer);
                jsonDoc.RootElement.GetProperty("EntityId").GetInt32(); //Check entityId exists (Exception here)
                return true;
            }
            catch (KeyNotFoundException) { return false; }
        }
    }
}
