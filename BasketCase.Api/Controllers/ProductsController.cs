using BasketCase.Business.Interfaces.Logging;
using BasketCase.Business.Interfaces.Product;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.Product;
using BasketCase.Domain.Enumerations;
using BasketCase.Framework.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BasketCase.Api.Controllers
{
    [ApiController]
    public class ProductsController : BaseApiController
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly ILogService _logService;

        #endregion

        #region Ctor

        public ProductsController(IProductService productService,
            ILogService logService)
        {
            _productService = productService;
            _logService = logService;
        }
        #endregion

        #region Methods

        [HttpPost("createproduct")]
        [AllowAnonymous]
        public virtual async Task<JsonResult> CreateAsync([FromBody] ProductCreateRequest request)
        {
            var serviceResponse = await _productService.CreateAsync(request);

            if (serviceResponse.Warnings.Count > 0 || serviceResponse.Warnings.Any())
            {
                _ = _logService.InsertLogAsync(LogLevel.Error, $"ProductsController - CreateAsync Error", JsonConvert.SerializeObject(serviceResponse));

                return BadResponse(new ResultModel
                {
                    Status = false,
                    Message = string.Join(Environment.NewLine, serviceResponse.Warnings.Select(err => string.Join(Environment.NewLine, err)))
                });
            }

            return OkResponse(new object
            {

            });
        }

        #endregion
    }
}
