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
using UmbracoTestTask.Services;

namespace UmbracoTestTask.Controllers
{
    public class LoginController : SurfaceController
    {
        private readonly ServiceContext serviceContext;
        private readonly IVariationContextAccessor variationContextAccessor;
        private readonly SOAPService soapService;
        public LoginController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider, IVariationContextAccessor variationContextAccessor, SOAPService soapService)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            serviceContext = services;
            this.variationContextAccessor = variationContextAccessor;
            this.soapService = soapService;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(LoginFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Not valid");
                return CurrentUmbracoPage();
            }
            var soapAnswer = await soapService.GetLoginResponse(model.Username, model.Password);
            
            var returnModel = new HomePage(CurrentPage, new PublishedValueFallback(serviceContext, variationContextAccessor))
            {
                _IsLogging = true
            };
            if (soapService.CheckLogged(soapAnswer))
            {
                returnModel._Answer = soapAnswer;
                returnModel.DesealizeModel();
            }
            return View("Views/HomePage.cshtml", returnModel);
        }
    }
}
