using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Dr_Shop.Models.Filters
{
    public class PageExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly ILogger<PageExceptionFilter> _logger;
        public PageExceptionFilter(IModelMetadataProvider modelMetadataProvider, ITempDataProvider tempDataProvider, ILogger<PageExceptionFilter> logger)
        {
            _modelMetadataProvider = modelMetadataProvider;
            _tempDataProvider = tempDataProvider;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            _logger.Log(LogLevel.Error, context.Exception.Message);
            context.Result = new ViewResult
            {
                ViewName = "Error404",
                ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
                {
                    {"ErrorCode",400}
                },
                TempData = new TempDataDictionary(context.HttpContext, _tempDataProvider)
                {
                    {"Error",context.Exception.Data["Error"]??null}
                }
            };

        }


    }
    public class JsonExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly ILogger<JsonExceptionFilter> _logger;

        public JsonExceptionFilter(ILogger<JsonExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            _logger.Log(LogLevel.Error, context.Exception.Message);
            context.Result = new JsonResult(new ResultDto { Message = context.Exception.Data["Error"]?.ToString() ?? "خطایی رخ داد !" });
        }
    }
}
