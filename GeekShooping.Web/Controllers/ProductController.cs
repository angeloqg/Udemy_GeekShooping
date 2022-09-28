using GeekShooping.Web.Models;
using GeekShooping.Web.Services;
using GeekShooping.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShooping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentException(nameof(productService));
        }

        [Authorize]
        public async Task<IActionResult> ProductIndex()
        {
            var products = await _productService.FindAllProducts();
            return View(products);
        }

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProductCreate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProduct(model);

                if (response != null) return RedirectToAction(nameof(ProductIndex));

            }

            return View(model);
        }

        public async Task<IActionResult> ProductUpdate(long id)
        {
            var model = await _productService.FindByIdProduct(id);
            if (model != null) return View(model);

            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProductUpdate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProduct(model);

                if (response != null) return RedirectToAction(nameof(ProductIndex));

            }

            return View(model);
        }

        public async Task<IActionResult> ProductDelete(long id)
        {
            var model = await _productService.FindByIdProduct(id);
            if (model != null) return View(model);

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ProductDelete(ProductModel model)
        {
            var success = await _productService.DeleteProductById(model.Id);

            if (success) return RedirectToAction(nameof(ProductIndex));

            return View(model);
        }
    }
}
