using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restoran.DAL.Contexts;
using Restoran.DTOs.ChefDtos;
using Restoran.DTOs.MemberDtos;
using Restoran.Models;
using Softy_Pinko.Services.Abstraction.Storage;

namespace Restoran.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ChefController : Controller
    {
        #region CTOR
        readonly AppDbContext _context;
        readonly IValidator<CreateChefDto> _createChefDtoValidator;
        readonly IValidator<UpdateChefDto> _updateChefDtoValidator;
        readonly IStorageService _storageService;
        readonly IWebHostEnvironment _env;
        public ChefController(AppDbContext context, IValidator<CreateChefDto> createChefDtoValidator, IValidator<UpdateChefDto> updateChefDtoValidator, IStorageService storageService, IWebHostEnvironment env)
        {
            _context = context;
            _createChefDtoValidator = createChefDtoValidator;
            _updateChefDtoValidator = updateChefDtoValidator;
            _storageService = storageService;
            _env = env;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chefs.Include(x => x.ChefDesignation).Select(x => new ChefDto
            {
                FullName = x.FullName,
                Id = x.Id,
                IsDeactive = x.IsDeactive,
                DesignationName = x.ChefDesignation.Name,
                ImgUrl = x.ImgUrl,                
            }).ToListAsync());
        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ChefDesignation = await _context.ChefDesignations.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateChefDto dto)
        {
            ViewBag.ChefDesignation = await _context.ChefDesignations.ToListAsync();
            ValidationResult result = await _createChefDtoValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                foreach (ValidationFailure item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View(dto);
            }

            (string? fileName, string? path) photoValue = await _storageService.UploadAsync(Path.Combine("Uploads", "Chef"), dto.Photo);

            if (photoValue.fileName == null || photoValue.path == null)
            {
                ModelState.AddModelError("", "Pjoto not uploaded");
                return View(dto);
            }

            await _context.Chefs.AddAsync(new Models.Chef
            {
                FullName = dto.FullName,
                ChefDesignationId = dto.ChefDesignationId,
                FacebookUrl = dto.FacebookUrl,
                ImgUrl = photoValue.fileName,
                InstagramUrl = dto.InstagramUrl,
                TwitterUrl = dto.TwitterUrl,                
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.ChefDesignation = await _context.ChefDesignations.ToListAsync();

            if (id == null)
            {
                return BadRequest();
            }
            Chef? dbChef = await _context.Chefs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbChef == null)
            {
                return NotFound();
            }
            return View(new UpdateChefDto
            {
                FullName = dbChef.FullName,
                Id = dbChef.Id,
                ChefDesignationId=dbChef.Id,
                TwitterUrl=dbChef.TwitterUrl,   
                InstagramUrl=dbChef.InstagramUrl,   
                FacebookUrl=dbChef.FacebookUrl,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateChefDto dto)
        {
            ViewBag.ChefDesignation = await _context.ChefDesignations.ToListAsync();

            if (id == null)
            {
                return BadRequest();
            }
            Chef? dbChef = await _context.Chefs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbChef == null)
            {
                return NotFound();
            }

            ValidationResult result = await _updateChefDtoValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                foreach (ValidationFailure item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View(dto);
            }

            string imgUrl = dbChef.ImgUrl;

            if (dto.Photo != null)
            {
                (string? fileName, string? path) photoValue = await _storageService.UploadAsync(Path.Combine("Uploads", "Chef"), dto.Photo);

                if (photoValue.fileName == null || photoValue.path == null)
                {
                    ModelState.AddModelError("", "Logo not uploaded");
                    return View(dto);
                }

                await _storageService.DeleteAsync(Path.Combine(_env.WebRootPath, "Uploads", "Chef"), dbChef.ImgUrl);
                imgUrl = photoValue.fileName;
            }

            dbChef.FullName = dto.FullName;
            dbChef.ImgUrl = imgUrl;
            dbChef.ChefDesignationId = dto.ChefDesignationId;
            dbChef.TwitterUrl = dto.TwitterUrl;
            dbChef.FacebookUrl = dto.FacebookUrl;
            dbChef.InstagramUrl = dto.InstagramUrl;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Chef? Chef = await _context.Chefs.FirstOrDefaultAsync(x => x.Id == id);
            if (Chef == null)
            {
                return NotFound();
            }

            Chef.IsDeactive = !Chef.IsDeactive;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion

    }
}
