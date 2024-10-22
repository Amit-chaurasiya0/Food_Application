using Microsoft.AspNetCore.Mvc;
using Food_Application.Models;
using Food_Application.Repository;
using Microsoft.AspNetCore.Authorization;
using Food_Application.ContextDbConfig;

namespace Food_Application.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IData data;
        private readonly FoodDBContext context;

        public CartController(IData data,FoodDBContext context)
        {
            this.data = data;
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await data.GetUser(HttpContext.User);
            var cartsList= context.Carts.Where(c=>c.UserId==user.Id).ToList();
            return View(cartsList);
        }
        [HttpPost]
        public async Task<IActionResult> SaveCart(Cart cart)
        {
            var user = await data.GetUser(HttpContext.User);
            cart.UserId = user?.Id;
            if (ModelState.IsValid)
            {
                await context.Carts.AddAsync(cart);
                await context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetAddedCart()
        {
            var user =await data.GetUser(HttpContext.User);
            var carts = context.Carts.Where(c =>c.UserId == user.Id).Select(c=> c.RecipeId).ToList();
            return Ok(carts);

        }
        [HttpPost]
        public IActionResult RemoveCartFromList(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var cart =context.Carts.Where(c => c.RecipeId==id).FirstOrDefault();
                if(cart != null)
                {
                    context.Carts.Remove(cart);
                    context.SaveChanges();
                    return Ok();
                }                
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetCartList()
        {
            var user = await data.GetUser(HttpContext.User);
            var cartList = context.Carts.Where(c=>c.UserId==user.Id).Take(3).ToList();
            return PartialView("_GetCartList",cartList);
        }
    }
}
