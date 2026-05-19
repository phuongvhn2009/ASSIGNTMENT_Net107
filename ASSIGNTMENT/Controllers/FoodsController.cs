using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASSIGNTMENT.Models;

public class FoodsController : Controller
{
    private readonly FastFoodDbContext _context;

    public FoodsController(FastFoodDbContext context)
    {
        _context = context;
    }

    // GET: FOODS
    public IActionResult Index()
    {
        // 👉 CHẶN KHÔNG CHO CUSTOMER VÀO
        if (HttpContext.Session.GetInt32("RoleId") != 1)
        {
            return RedirectToAction("Index", "Home");
        }

        var foods = _context.Foods.ToList();
        return View(foods);
    }
    public IActionResult Menu()
    {
        // ❌ CHƯA LOGIN → đá về login
        if (HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var foods = _context.Foods.ToList();
        return View(foods);
    }

    public IActionResult SearchPage()
    {
        var foods = _context.Foods.ToList(); // load toàn bộ ban đầu
        return View(foods);
    }

    [HttpGet]
    public IActionResult Search(string name, decimal? minPrice, decimal? maxPrice, string category, string description)
    {
        var query = _context.Foods.AsQueryable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(x => x.Name.Contains(name));

        if (minPrice.HasValue)
            query = query.Where(x => x.Price >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(x => x.Price <= maxPrice);

        if (!string.IsNullOrEmpty(category))
            query = query.Where(x => x.Category != null && x.Category.Name != null && x.Category.Name.Contains(category));

        if (!string.IsNullOrEmpty(description))
            query = query.Where(x => x.Description.Contains(description));

        return View("SearchPage", query.ToList());
    }

    // GET: FOODS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        var food = await _context.Foods.FirstOrDefaultAsync(x => x.Id == id);

        if (food == null)
            return NotFound();

        return View(food);
    }

    // GET: FOODS/Create
    public IActionResult Create()
    {
        return View();
    }


    public async Task<IActionResult> DetailsUser(int? id)
    {
        var food = await _context.Foods.FirstOrDefaultAsync(x => x.Id == id);

        if (food == null)
            return NotFound();

        return View("DetailsUser", food);
    }

    // POST: FOODS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,Image,CategoryId,Category,ComboDetails,OrderDetails")] Food food)
    {
        if (ModelState.IsValid)
        {
            _context.Add(food);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(food);
    }

    // GET: FOODS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var food = await _context.Foods.FindAsync(id);
        if (food == null)
        {
            return NotFound();
        }
        return View(food);
    }

    // POST: FOODS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Price,Description,Image,CategoryId,Category,ComboDetails,OrderDetails")] Food food)
    {
        if (id != food.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(food);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(food.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(food);
    }

    // GET: FOODS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var food = await _context.Foods
            .FirstOrDefaultAsync(m => m.Id == id);
        if (food == null)
        {
            return NotFound();
        }

        return View(food);
    }

    // POST: FOODS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var food = await _context.Foods.FindAsync(id);
        if (food != null)
        {
            _context.Foods.Remove(food);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool FoodExists(int? id)
    {
        return _context.Foods.Any(e => e.Id == id);
    }
}
