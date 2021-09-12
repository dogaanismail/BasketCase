using BasketCase.Business.Interfaces.Product;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.Product;
using BasketCase.Framework.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        #endregion

        #region Ctor

        public ProductsController(IProductService productService)
        {
            _productService = productService;
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
