using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WanoSivuv3.Data;
using WanoSivuv3.Models;

namespace WanoSivuv3.Controllers
{
    public class UsersController : Controller
    {
        private readonly WanoSivuv3Context _context;

        public UsersController(WanoSivuv3Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Statistics()
        {
            //statistic 1- how many orders every customer had made, there is only one shopping cart
            ICollection<Stat> statistic1 = new Collection<Stat>();
            var result1 = from c in _context.User.Include(o => o.Type).GroupBy(u => u.Type)
                          select c.Count();
            int countUser = 0, countAdmin = 0;
            foreach(var u in (from c in _context.User select c))
            {
                if (u.Type == UserType.Client)
                    countUser++;
                else
                    countAdmin++;
            }
            statistic1.Add(new Stat("Client", countUser));
            statistic1.Add(new Stat("Admin", countAdmin));

            ViewBag.data = statistic1;
            //finish first statistic
            //statistic 2- which brand the customers prefer to order
            /*ICollection<Stat> statistic2 = new Collection<Stat>();

            int Count;
            var result2 = (from p in _context.Products where (1 < 0) select new ObjectResult()).ToList();//create empty result table
            foreach (var pro in _context.Products.Include(po => po.ProductOrders).ThenInclude(o => o.Order))
            {
                Count = 0;
                if (pro == null)
                    continue;
                foreach (var po in pro.ProductOrders)
                {
                    if (po == null)
                        continue;
                    if (po.Product.Brand == pro.Brand)
                        ++Count;
                }
                result2.Add(new ObjectResult() { Brand = pro.Brand, count = Count });
            }
            foreach (var v in result2)
            {
                if (v.count > 0)
                    statistic2.Add(new Stat(v.Brand, v.count));
            }

            ViewBag.data2 = statistic2;*/

            return View();
        }
        
        internal class ObjectResult
        {
            public string Type { get; set; }
            public int count { get; set; }
            public ObjectResult(string t,int c) { Type = t;count = c; }
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Id,Username,Email,Password,Product")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = from u in _context.User
                        where u.Username == user.Username && u.Password == user.Password
                        select u;
                //var q = _context.User.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
                if (q.Count() > 0)
                {
                    //HttpContext.Session.SetString("Username", q.First().Username);
                    Signin(q.First());
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Wrong username or password.";
                }
            }
            return View(user);
        }
        public async Task<IActionResult> Logout()
        {
            //HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Index), "Home");
        }
        private async void Signin(User account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Type.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }


        // GET: Users/Register
        public IActionResult Register()
        {
            ViewData["Productss"] = new SelectList(_context.Product, nameof(Product.Id), nameof(Product.Name));
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Username,Email,Password,ProductId")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _context.User.FirstOrDefault(u => u.Username == user.Username);
                var p = _context.User.FirstOrDefault(u => u.ProductId == user.ProductId);
                if (q == null && p == null)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    var u = _context.User.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
                    Signin(u);
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Invalid username";
                }
                ViewData["Productss"] = new SelectList(_context.Product, nameof(Product.Id), nameof(Product.Name));
            }
            return View(user);
        }



        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Join()
        {
            var query =
                from user in _context.User
                join info in _context.UserInfo on user.Id equals info.UserId
                select new UserJoin(user,info);
            return View("Join", await query.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FavoriteDish()
        {
            var query =
                from prod in _context.Product
                join user in _context.User on prod.Id equals user.ProductId
                select new FavoriteDish(user, prod);
            return View("FavoriteDish", await query.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        /*
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Email,Password,Type")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        */
        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,Password,Type,Product")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
    public class UserJoin
    {
        public User u { get; set; }
        public UserInfo ui { get; set; }

        public UserJoin(User u, UserInfo ui) { this.u = u; this.ui = ui; }
    }
    public class FavoriteDish
    {
        public User u { get; set; }
        public Product pro { get; set; }

        public FavoriteDish(User u, Product ui) { this.u = u; this.pro = ui; }
    }
    /*public IActionResult Statistics()
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

                 var dataPoints = new List<Charts>();
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
                 var piedataPoints = new List<pCharts>();
                 villasPie.ForEach(x => piedataPoints.Add(new PieDataPoint() { Name = x.Description.ToString(), Value = x.Count }));

                 var model = new StatisticsViewModel
                 {
                     barChart = new BarChart { dataPoints = dataPoints },
                     pieChart = new PieChart { dataPoints = piedataPoints }
                 };
                 return View(model);

         return RedirectToAction("Login", "Accounts");
     }*/
    public class Stat
    {
        public string Key;
        public int Values;
        public Stat(string key, int values)
        {
            Key = key;
            Values = values;
        }
    }
}