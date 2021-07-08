using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var WanoSivuv3Context = _context.Product.Include(c => c.Category).Where(p => p.Name.Contains(queryN) ||
                                    (queryN == null) || (p.Desc.Contains(queryN)));
            return View("Menu",await WanoSivuv3Context.ToListAsync());
        }
        public async Task<IActionResult> Buttom(string ctN)
        {
            var WanoSivuv3Context = _context.Product.Include(c => c.Category).Where(p => p.Category.Name.Equals(ctN) ||
                                    (ctN == null));
            return View("Menu", await WanoSivuv3Context.ToListAsync());
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
                 Product pro = _context.Product.Include(p => p.myTags).FirstOrDefault(p => p.Id == product.Id);

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
/*
    public IActionResult Statistics()
    {
        if (HttpContext.Session.GetString("userId") != null)
        {
            if (HttpContext.Session.GetString("Type").Equals("Admin"))
            {

                var mostPopulaMonths = _context.Account
           .GroupBy(y => y.weddingDate.Month, (month, records) => new
           {
               Key = month,
               Count = records.Count(),
               Description = month
           })
           .OrderBy(x => x.Key)
           .ToList();

                var dataPoints = new List<BarData>();
                mostPopulaMonths.ForEach(x => dataPoints.Add(new BarData() { name = this.getMonthName(x.Description), value = x.Count }));

                var villasPie = _context.Account
                    .GroupBy(y => y.LocationId, (locationId, records) => new
                    {
                        Key = locationId,
                        Count = records.Count(),
                        Description = locationId
                    })
                    .OrderBy(x => x.Count)
                    .ToList();
                var piedataPoints = new List<PieDataPoint>();
                villasPie.ForEach(x => piedataPoints.Add(new PieDataPoint() { Name = x.Description.ToString(), Value = x.Count }));

                var model = new StatisticsViewModel
                {
                    barChart = new BarChart { dataPoints = dataPoints },
                    pieChart = new PieChart { dataPoints = piedataPoints }
                };
                return View(model);

            }


        }
        return RedirectToAction("Login", "Accounts");
    }*/
}
