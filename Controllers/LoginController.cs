using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoTestTask.Models;

namespace UmbracoTestTask.Controllers
{
    public class LoginController : SurfaceController
    {
        readonly ServiceContext serviceContext;
        readonly IVariationContextAccessor variationContextAccessor;
        public LoginController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider, IVariationContextAccessor variationContextAccessor)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            serviceContext = services;
            this.variationContextAccessor = variationContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(LoginFormViewModel model)
        {
            Debug.WriteLine(model.Username);
            Debug.WriteLine(model.Password);
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Not valid");
                return CurrentUmbracoPage();
            }
            var client = new IICU.ICUTechClient();
            var ans = await client.LoginAsync(model.Username, model.Password, "");
            var props = JsonDocument.Parse(ans.@return);
            var productViewModel = new HomePage(CurrentPage, new PublishedValueFallback(serviceContext, variationContextAccessor))
            {
                _IsLogged = true
            };
            try
            {
                props.RootElement.GetProperty("EntityId").GetInt32();
                productViewModel._Answer = ans.@return;
            }
            catch (KeyNotFoundException)
            {
                Debug.WriteLine("User not found");
            }
            return View("Views/HomePage.cshtml", productViewModel);

            //return RedirectToCurrentUmbracoPage(new QueryString("?status=\"" + ans.@return + "\""));
        }
    }
}
