using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InokerWebApp.Models;

namespace InokerWebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly InokerwebappdbContext _context;

        public ProductsController(InokerwebappdbContext context)
        {
            _context = context;
        }

        public async Task<PartialViewResult> _EditProduct(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                ViewBag.PrMessage = "Error";
                return PartialView();
            }
            return PartialView(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> _EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                Product p = await _context.Products.FindAsync(product.ProductId);
                if (p == null)
                {
                    ViewBag.Message = "Couldn't find by id";
                    return PartialView(product);
                }
                p.Height = product.Height;
                p.Width = product.Width;
                p.Stock = product.Stock;
                p.Price = product.Price;
                try
                {                   
                    _context.Products.Update(p);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ViewBag.PrMessage = $"{ex.Message}";
                    return PartialView(product);
                }
                ViewBag.PrMessage = "Produkt uspesno promenjen";
            }
            return PartialView(product);
        }

        public PartialViewResult _CreateProduct(int modelId)
        {
            ViewBag.ModelId = modelId;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> _CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    product = _context.Products.Add(product).Entity;
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ViewBag.PrMessage = $"{ex.Message}";
                    return PartialView();
                }
                ViewBag.Message = "Produkt uspesno kreiran";
                return PartialView(nameof(_EditProduct), product);
            }
            return PartialView(product);        
        }







        //// GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    var inokerwebappdbContext = _context.Products.Include(p => p.Model);
        //    return View(await inokerwebappdbContext.ToListAsync());
        //}

        //// GET: Products/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Model)
        //        .SingleOrDefaultAsync(m => m.ProductId == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// GET: Products/Create
        //public IActionResult Create()
        //{
        //    ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "Name");
        //    return View();
        //}

        //// POST: Products/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ProductId,ModelId,Width,Height,Stock,Price")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "Name", product.ModelId);
        //    return View(product);
        //}

        //// GET: Products/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductId == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "Name", product.ModelId);
        //    return View(product);
        //}

        //// POST: Products/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ProductId,ModelId,Width,Height,Stock,Price")] Product product)
        //{
        //    if (id != product.ProductId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.ProductId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ModelId"] = new SelectList(_context.Models, "ModelId", "Name", product.ModelId);
        //    return View(product);
        //}

        //// GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Products
        //        .Include(p => p.Model)
        //        .SingleOrDefaultAsync(m => m.ProductId == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        // POST: Products/Delete/5
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(m => m.ProductId == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit","Model",new { id = product.ModelId, message = "Varijacija <strong>" + product.Width + "x" + product.Height + "</strong> uspeno obrisana" });
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
