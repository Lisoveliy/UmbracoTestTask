using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Services;

namespace UmbracoTestTask.Services
{
    public class RegisterSOAPServiceComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<SOAPService>();
        }
    }
}
