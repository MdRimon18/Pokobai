using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Shared
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ViewRenderService(ICompositeViewEngine viewEngine, IHttpContextAccessor httpContextAccessor)
        {
            _viewEngine = viewEngine;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> RenderViewToStringAsync(ControllerContext controllerContext, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(nameof(viewName));

            var actionContext = controllerContext ?? new ControllerContext
            {
                HttpContext = _httpContextAccessor.HttpContext
            };

            using var stringWriter = new StringWriter();
            var viewResult = _viewEngine.FindView(actionContext, viewName, false);

            if (viewResult.View == null)
                throw new InvalidOperationException($"View '{viewName}' not found.");

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(actionContext.HttpContext, new SessionStateTempDataProvider()),
                stringWriter,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return stringWriter.ToString();
        }
    }


}
