using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanoSivuv3.Data;
using WanoSivuv3.Models;

namespace WanoSivuv3.Controllers
{
    public class ProductsController : Controller
    {
        private readonly WanoSivuv3Context _context;

        public ProductsController(WanoSivuv3Context context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var WanoSivuv3Context = _context.Product.Include(c => c.Category);
            return View(await WanoSivuv3Context.ToListAsync());
            //return View(await _context.Product.ToListAsync());
        }
        public async Task<IActionResult> Menu()
        {
            var WanoSivuv3Context = _context.Product.Include(c => c.Category);
            return View(await WanoSivuv3Context.ToListAsync());
        }
        public async Task<IActionResult> Search(string queryN)
        {
            var WanoSivuv3Context = _context.Product.Include(c => c.Category).Where(p => p.Name.Contains(queryN) || (queryN == null) || (p.Desc.Contains(queryN)));
            return View("Menu",await WanoSivuv3Context.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(c => c.Category).Include(t => t.myTags)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Categoriess"] = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.Name));
            ViewData["Tagss"] = new SelectList(_context.Tags, nameof(Tags.Id), nameof(Tags.Name));

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Desc,Image,CategoryId,myTags")] Product product, int[] myTags) //po mosifim CategoryId she ze istader
        {
            product.myTags = new List<Tags>();
            product.myTags.AddRange(_context.Tags.Where(x => myTags.Contains(x.Id)));
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Categoriess"] = new SelectList(_context.Category, nameof(Category.Id), nameof(Category.Name));
            ViewData["Tagss"] = new SelectList(_context.Tags, nameof(Tags.Id), nameof(Tags.Name));
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Desc,Image,CategoryId,myTags")] Product product, int[] myTags)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    /* if (product.myTags == null)
                     {
                         product.myTags = new List<Tags>();
                         product.myTags.AddRange(_context.Tags.Where(x => myTags.Contains(x.Id)));
                         _context.Update(product);
                         await _context.SaveChangesAsync();
                     }*/
                    //else
                    // {
                    //_context.Remove(product.myTags);
                    //product = _context.Tags.Include(t => t.Id)
                    /************ Product pro = _context.Product.Include(p => p.myTags).FirstOrDefault(p => p.Id == product.Id);
                 _context.Remove(pro);
                 _context.SaveChanges();*********/
                    /*var harta = _context.Product.
                    Include(p => p.myTags).l*/
                    //product.myTags = pro.myTags.Where(p => myTags.Contains(p.Id)).ToList();
                    /*foreach (var item in product.myTags)
                    {
                        //product.myTags.();
                        _context.SaveChanges();
                    }*/
                    //product.myTags = new List<Tags>();
                    var noder = _context.Product.Where(a => a.myTags.Any(t => t.Id == a.Id)).ToList();
                    var prod = _context.Product.Include(p => p.myTags).FirstOrDefault(p => p.Id == product.Id);
                    var tagi = _context.Tags.Include(t => t.myProducts).FirstOrDefault();//(t =>t.Id.CompareTo(myTags));
                    //var lodea = _context.Product.Include(p => p.myTags).GroupBy(x => new { x.Id, x.myTags});
                    product.myTags = new List<Tags>();
                    product.myTags.AddRange(_context.Tags.Where(x => myTags.Contains(x.Id)));
                    _context.Update(product);
                        /* product.myTags.AddRange(_context.Tags.Where(x => myTags.Contains(x.Id)));
                         _context.Update(product);*/
                        await _context.SaveChangesAsync();
                   // }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }

}
