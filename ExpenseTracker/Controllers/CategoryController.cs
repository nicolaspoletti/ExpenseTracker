﻿using ExpenseTracker.Models;
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
    
    
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        else
            return View(category);
    }

    public IActionResult Edit(int id)
    {
        var category = _context.Categories.Find(id);
        return View(category);
    }
    
    [HttpPost]
    public IActionResult Edit(int id, Category category)
    {
        category.CategoryId = id;

        if (ModelState.IsValid)
        {
            var categoryToUpdate = _context.Categories.Find(id);

            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            categoryToUpdate.Title = category.Title;
            categoryToUpdate.Type = category.Type;
            categoryToUpdate.Icon = category.Icon;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(category);
    }

    public IActionResult SaveCategory()
    {
        
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details()
    {
        throw new NotImplementedException();
    }

    public IActionResult Delete()
    {
        throw new NotImplementedException();
    }
}