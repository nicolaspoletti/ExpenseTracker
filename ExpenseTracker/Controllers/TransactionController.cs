using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers;

public class TransactionController : Controller
{
    private readonly ApplicationDbContext _context;

    public TransactionController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Transactions.Include(t => t.Category);
        return View(await applicationDbContext.ToListAsync());
    }

    
    public IActionResult Create(int id)
    {
        PopulateCategories();
        return View(new Transaction());
    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction, int CategoryId)
    {

        var cat = _context.Categories.Find(CategoryId);
        transaction.Category = cat;
        
        Console.WriteLine(">>Bind from view:");
        Console.WriteLine("Amount:" + transaction.Amount);
        Console.WriteLine("CategoryID:" + transaction.CategoryId);
        
        transaction.CategoryId = CategoryId;
        
        if (ModelState.IsValid)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        if (!ModelState.IsValid)
        {
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                if (state.ValidationState == ModelValidationState.Invalid)
                {
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"{key}: {error.ErrorMessage}");
                    }
                }
            }
        }

        PopulateCategories();
        return View(transaction);
    }

    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        if (_context.Transactions == null)
        {
            return Problem("_context.Transactions is empty");
        }

        var transaction = await _context.Transactions.FindAsync(id);
        
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    
    [NonAction]
    public void PopulateCategories()
    {
        var allCategories = _context.Categories.ToList();
        
        Category defaultCategory = new Category()
        {
            CategoryId = 0,
            Title = "Choose a Category"
        };
        
        allCategories.Insert(0,defaultCategory);

        ViewBag.Categories = allCategories;
    }
    
    
}