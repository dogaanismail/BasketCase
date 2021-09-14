using BasketCase.Business.Interfaces.Basket;
using BasketCase.Business.Interfaces.Logging;
using BasketCase.Domain.Common;
using BasketCase.Domain.Dto.Request.ShoppingCart;
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
    public class ShoppingCartsController : BaseApiController
    {
        #region Fields
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ILogService _logService;

        #endregion

        #region Ctor
        public ShoppingCartsController(IShoppingCartService shoppingCartService,
            ILogService logService)
        {
            _shoppingCartService = shoppingCartService;
            _logService = logService;
        }

        #endregion

        #region Methods

        [HttpPost("add-to-cart")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> AddToCartAsync([FromBody] AddToCartRequest request)
        {
            var serviceResponse = await _shoppingCartService.AddToCartAsync(request);

            if (serviceResponse.Warnings.Count > 0 || serviceResponse.Warnings.Any())
            {
                _ = _logService.InsertLogAsync(LogLevel.Error, $"ShoppingCartsController - AddToCartAsync Error", JsonConvert.SerializeObject(serviceResponse));

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
