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
    public class ProductVariantsController : BaseApiController
    {
        #region Fields
        private readonly IProductVariantService _productVariantService;
        private readonly ILogService _logService;

        #endregion

        #region Ctor
        public ProductVariantsController(IProductVariantService productVariantService,
            ILogService logService)
        {
            _productVariantService = productVariantService;
            _logService = logService;
        }
        #endregion

        #region Methods

        [HttpPost("create-variant")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> CreateAsync([FromBody] ProductVariantCreateRequest request)
        {
            _ = _logService.InsertLogAsync(LogLevel.Information, $"ProductVariantsController - CreateAsyncRequest", JsonConvert.SerializeObject(request));

            var serviceResponse = await _productVariantService.CreateAsync(request);

            if (serviceResponse.Warnings.Count > 0 || serviceResponse.Warnings.Any())
            {
                _ = _logService.InsertLogAsync(LogLevel.Error, $"ProductVariantsController - CreateAsync Error", JsonConvert.SerializeObject(serviceResponse));

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

        [HttpGet("id/{id}")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetByIdAsync(string id)
        {
            var data = await _productVariantService.GetByIdAsync(id);

            return OkResponse(data);
        }

        [HttpGet("productId/{productId}")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> GetByProductIdAsync(string productId)
        {
            var data = await _productVariantService.GetByProductIdAsync(productId);

            return OkResponse(data);
        }

        #endregion
    }
}
