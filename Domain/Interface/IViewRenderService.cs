using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IViewRenderService
    {
        Task<string> RenderViewToStringAsync(ControllerContext controllerContext, string viewName, object model);
    }

}
