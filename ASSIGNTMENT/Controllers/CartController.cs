using Microsoft.AspNetCore.Mvc;
using ASSIGNTMENT.Models;
using ASSIGNTMENT.Helpers;

public class CartController : Controller
{
    private readonly FastFoodDbContext _context;

    public CartController(FastFoodDbContext context)
    {
        _context = context;
    }

    // ➕ ADD TO CART (lưu ID)
    public IActionResult Add(int id)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<int>>("cart")
                   ?? new List<int>();

        if (!cart.Contains(id))
        {
            cart.Add(id);
        }

        HttpContext.Session.SetObjectAsJson("cart", cart);

        return RedirectToAction("Index", "Home");
    }

    // 🛒 VIEW CART
    public IActionResult Index()
    {
        var cartIds = HttpContext.Session.GetObjectFromJson<List<int>>("cart")
                      ?? new List<int>();

        var foods = _context.Foods
            .Where(x => cartIds.Contains(x.Id))
            .ToList();

        return View(foods);
    }

    // ❌ REMOVE ITEM
    public IActionResult Remove(int id)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<int>>("cart")
                   ?? new List<int>();

        cart.Remove(id);

        HttpContext.Session.SetObjectAsJson("cart", cart);

        return RedirectToAction("Index");
    }
}