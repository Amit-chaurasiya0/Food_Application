using Food_Application.ContextDbConfig;
using Food_Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Food_Application.Controllers
{
    public class RecipeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FoodDBContext context;
        public RecipeController(UserManager<ApplicationUser> userManager, FoodDBContext context)
        {
            _userManager = userManager;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult GetRecipeCard([FromBody] List<Recipe> recipes)
        {
            return PartialView("_RecipeCard", recipes);
        }
        public IActionResult Search([FromQuery] string recipe) 
        {
            ViewBag.Recipe = recipe;
            return View();
        }
        public IActionResult Order([FromQuery] string id)
        {
            ViewBag.Id = id;            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ShowOrder( OrderRecipeDetails orderRecipeDetails) 
        {
            Random random = new Random();
            ViewBag.price = Math.Round( random.Next(100, 500)/5.0)*5;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user?.Id;
            ViewBag.Address = user?.Address;
            return PartialView("_ShowOrder", orderRecipeDetails); 
        }
        [HttpPost]
        [Authorize]
        public IActionResult Order ([FromForm]Order order)
        {
            if(ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                context.Orders.Add(order);
                context.SaveChanges();
                return RedirectToAction("Index","Recipe");
            }
            return RedirectToAction("Order", "Recope", new {id=order.Id});
        }
    }
}
