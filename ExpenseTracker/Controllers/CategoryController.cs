using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return _context.Categories != null ? 
            View(await _context.Categories.ToListAsync()) : Problem("Entity set 'ApplicationDbContext.Categories is null'");
    }

    public IActionResult Create()
    {
        return View(new Category());
    }
    
    

}