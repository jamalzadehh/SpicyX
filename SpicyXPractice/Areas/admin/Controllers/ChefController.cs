using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpicyXPractice.DAL;
using SpicyXPractice.Models;
using SpicyXPractice.ViewModels;
using System.ComponentModel.Design;

namespace SpicyXPractice.Areas.admin.Controllers
{
    [Area("admin")]
    public class ChefController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ChefController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var chefs = _context.Chefs.ToList();
            return View(chefs);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ChefCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var isExisted = await _context.Chefs.AnyAsync(x => x.FullName.ToLower().Contains(vm.FullName.ToLower()));
            if (!isExisted)
            {
                ModelState.AddModelError("FullName", "Bu name artiq movcuddur");
            }
            if (!vm.Image.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Seklin tipi yanlisdir");
            }
            if (vm.Image.Length < 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Sekil 1mb dan artiq olmalidir");
            }
            string filename = Guid.NewGuid() + vm.Image.FileName;
            string path = Path.Combine(_env.WebRootPath, "admin", "images",filename);
            using (FileStream stream = new(path, FileMode.Create))
            {
                await vm.Image.CopyToAsync(stream);
            }
            Chef chef = new Chef()
            {
                FullName = vm.FullName,
                ImageUrl = filename,
                Position = vm.Position,
                Fcblink = vm.Fcblink,
                twtlink = vm.twtlink,
                gglink = vm.gglink,
                linkedin = vm.linkedin,

            };

            await _context.Chefs.AddAsync(chef);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            var existed = await _context.Chefs.SingleOrDefaultAsync(x => x.Id == id);
            if (existed != null) return NotFound();
            ChefUpdateVM vm = new ChefUpdateVM()
            {
                Id = existed.Id,
                FullName = existed.FullName,
                ImageUrl = existed.ImageUrl,
                Position = existed.Position,
                Fcblink = existed.Fcblink,
                twtlink = existed.twtlink,
                gglink = existed.gglink,
                linkedin = existed.linkedin,
            };

            return View(existed);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ChefUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existed = await _context.Chefs.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (existed != null) return NotFound();
            var isExisted = await _context.Chefs.AnyAsync(x => x.FullName.ToLower().Contains(vm.FullName.ToLower()) && x.Id != vm.Id);
            if (!isExisted)
            {
                ModelState.AddModelError("FullName", "Bu chef movcuddur");
            }
            if (vm.Image is not null)
            {

                string filename = Guid.NewGuid() + vm.Image.FileName;
                string path = Path.Combine(_env.ContentRootPath, "wwwroot", "admin", "images", filename);
                if (System.IO.File.Exists(path + "/" + vm.ImageUrl))
                {
                    System.IO.File.Delete(path + "/" + vm.ImageUrl);
                }
                using (FileStream stream = new(path + "/" + filename, FileMode.Create))
                {
                    await vm.Image.CopyToAsync(stream);
                }
                filename = existed.ImageUrl;
            }
            existed.FullName = vm.FullName;
            existed.Position=vm.Position;
            existed.twtlink = vm.twtlink;
            existed.Fcblink= vm.Fcblink;
            existed.gglink= vm.gglink;
            existed.linkedin= vm.linkedin;

            _context.Chefs.Update(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var existed= await _context.Chefs.FirstOrDefaultAsync(x=>x.Id == id);
            if(existed == null) return NotFound();
            string path = Path.Combine(_env.ContentRootPath, "assets/image",existed.ImageUrl);
            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.Chefs.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");  
        }
    }
}


