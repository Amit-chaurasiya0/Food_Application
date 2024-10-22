namespace Food_Application.Models
{
    public class OrderRecipeDetails
    {
        public string? id { get; set; }
        public string? cooking_time { get; set; }
        public string? image_url { get; set; }
        public string? publisher { get; set; }
        public string? title { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public OrderRecipeDetails()
        {
            Ingredients = new List<Ingredient>();
        }
    }
    public class Ingredient
    {
        public string? description { get; set; }
        public int quantity { get; set; }
        public string? unit { get; set; }
    }
}
