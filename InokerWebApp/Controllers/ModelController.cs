using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InokerWebApp.Data;
using InokerWebApp.Models;
using InokerWebApp.Models.InokerViewModels;
using PagedList;
using Microsoft.AspNetCore.Authorization;

namespace InokerWebApp.Controllers
{
    [Authorize(Roles = "boss, admin")]
    public class ModelController : Controller
    {
        private readonly InokerwebappdbContext db;

        public ModelController(ApplicationDbContext context, InokerwebappdbContext _db)
        {
            db = _db;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var models = db.Models.Include(m => m.Collection).Include(m => m.Products);
            IQueryable<ModelViewModel> modelViews = models.Select(m =>
                new ModelViewModel
                {
                    ModelId = m.ModelId,
                    Name = m.Name,
                    Collection = m.Collection.Name,
                    Photo1 = Convert.ToBase64String(m.Photo1),
                    Photo2 = Convert.ToBase64String(m.Photo2)
                });

            //JumboImg jumboImg = await db.JumboImgs.FindAsync(new Random().Next() * (db.JumboImgs.Count() - 1));
            //ViewBag.Jumbotron = Convert.ToBase64String(jumboImg.Photo);
            List<ModelViewModel> viewModels = await modelViews.ToListAsync();
            return View( viewModels.ToPagedList(1, 10));
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await db.Models
                .Include(p => p.Collection)
                .Include(p => p.Products)
                .SingleOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return NotFound();
            }

            ModelViewModel modelViewModel = new ModelViewModel
            {
                ModelId = model.ModelId,
                Collection = model.Collection.Name,
                Photo1 = Convert.ToBase64String(model.Photo1),
                Photo2 = Convert.ToBase64String(model.Photo2)
            };

            return View(modelViewModel);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create(string message)
        {
            ViewBag.Message = message;
            await FillDataListViewBags();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ModelViewModel modelViewModel)
        {
            if (ModelState.IsValid)
            {
                Model model = new Model
                {
                    CollectionId = await GetOrCreateCollection(modelViewModel.Collection),
                    Name = modelViewModel.Name,
                    Photo1 = Convert.FromBase64String(modelViewModel.Photo1.Split(",")[1]),
                    Photo2 = Convert.FromBase64String(modelViewModel.Photo2.Split(",")[1])
                };
                try
                {
                    db.Models.Add(model);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Create), new { message = $"Errors in adding model. Error message: {ex.Message}" });
                }               
                return RedirectToAction(nameof(Index));
            }

            await FillDataListViewBags();
            return View(modelViewModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id, string message)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await db.Models
                .Include(m => m.Products)
                .Include(m => m.Collection)
                .SingleOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.Message = message;
            await FillDataListViewBags();

            ModelViewModel modelViewModel = new ModelViewModel
            {
                ModelId = model.ModelId,
                Name = model.Name,
                Collection = model.Collection.Name,
                Photo1 = Convert.ToBase64String(model.Photo1),
                Photo2 = Convert.ToBase64String(model.Photo2)
            };

            ViewBag.Products = model.Products;
            return View(modelViewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ModelViewModel modelViewModel)
        {
            if (ModelState.IsValid)
            {
                Model model = db.Models.Find(modelViewModel.ModelId);
                if (model == null)
                    return NotFound();


                model.CollectionId = await GetOrCreateCollection(modelViewModel.Collection);
                model.Name = modelViewModel.Name;
                model.Photo1 = Convert.FromBase64String(modelViewModel.Photo1.Split(",")[1]);
                model.Photo2 = Convert.FromBase64String(modelViewModel.Photo2.Split(",")[1]);
                try
                {
                    db.Models.Update(model);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return RedirectToAction(nameof(Edit), new { id= modelViewModel.ModelId, message = $"Errors in editing product. Error message: {ex.Message}" });
                }               
                return RedirectToAction(nameof(Index));
            }

            await FillDataListViewBags();
            return View(modelViewModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await db.Models
                .Include(p => p.Collection)
                .Include(p => p.Products)
                .SingleOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await db.Models.SingleOrDefaultAsync(m => m.ModelId == id);
            db.Models.Remove(model);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        [HttpGet]
        public FileContentResult GetModelTileImg(int? id)
        {
            if (id == null)
            {
                return null;
            }
            byte[] sl = db.Models.Find(id).Photo1;
            if (sl == null)
            {
                return null;
            }
            return File(sl, "image/gif");
        }

        [AllowAnonymous]
        [HttpGet]
        public string GetModelName(int? id)
        {
            if (id == null)
            {
                return null;
            }
            string name = db.Models.Find(id).Name;
            if (name == null)
            {
                return null;
            }
            return name;
        }

        public  IActionResult CreateJumboImg(string message)
        {
            ViewBag.Models = db.Models.Select(m => m.Name);
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJumboImg(JumboImgViewModel model)
        {
            if (ModelState.IsValid)
            {
                Model PrModel = await db.Models.SingleOrDefaultAsync(m => model.Product == m.Name);
                if (PrModel == null)
                {
                    ViewBag.Models = db.Models.Select(m => m.Name);
                    ViewBag.Message = "Select model";
                    return View(model);
                }
                JumboImg jumboImg = new JumboImg
                {
                    Photo = Convert.FromBase64String(model.Photo.Split(",")[1]),
                    ModelId = PrModel.ModelId
                };
                try
                {
                    await db.JumboImgs.AddAsync(jumboImg);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(CreateJumboImg), new { message = $"Image for {PrModel.Name} successfuly created" });
                }
                catch (Exception ex)
                {
                    ViewBag.Models = db.Models.Select(m => m.Name);
                    ViewBag.Message = $"Error msg: {ex.Message}... Inner msg: {ex.InnerException.Message}";
                    return View(model);
                }
            }
            ViewBag.Models = db.Models.Select(m => m.Name);
            return View(model);

        }

        private async Task FillDataListViewBags()
        {
            ViewBag.Collections = await db.Collections.Select(c => c.Name).ToListAsync();
            //ViewBag.Models = await db.Models.Select(m => m.Name).ToListAsync();
        }

        private async Task<int> GetOrCreateCollection(string collectionName)
        {
            Collection collection = await db.Collections.SingleOrDefaultAsync(c => c.Name == collectionName);
            
            if(collection == null) // Create new
            {
                collection = new Collection { Name = collectionName };
                try
                {
                    var result = await db.Collections.AddAsync(collection);
                    await db.SaveChangesAsync();
                    return result.Entity.CollectionId;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return collection.CollectionId;
        }
    }
}
