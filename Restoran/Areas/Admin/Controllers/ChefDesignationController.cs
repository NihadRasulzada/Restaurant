using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Restoran.DAL.Contexts;
using Restoran.DTOs.MemberDesignationDtos;
using Restoran.Models;

namespace Restoran.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ChefDesignationController : Controller
    {
        #region CTOR
        readonly AppDbContext _context;
        readonly IValidator<CreateChefDesignationDto> _createChefDesignationDtoValidator;
        readonly IValidator<UpdateChefDesignationDto> _updateChefDesignationDtoValidator;

        public ChefDesignationController(AppDbContext context, IValidator<CreateChefDesignationDto> createChefDesignationDtoValidator, IValidator<UpdateChefDesignationDto> updateChefDesignationDtoValidator)
        {
            _context = context;
            _createChefDesignationDtoValidator = createChefDesignationDtoValidator;
            _updateChefDesignationDtoValidator = updateChefDesignationDtoValidator;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChefDesignations.Select(x => new ChefDesignationDto
            {
                Id = x.Id,
                Name = x.Name,
                IsDeactive = x.IsDeactive,
            }).ToListAsync());
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateChefDesignationDto dto)
        {
            ValidationResult result = await _createChefDesignationDtoValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                foreach (ValidationFailure item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View(dto);
            }

            await _context.ChefDesignations.AddAsync(new ChefDesignation
            {
                Name = dto.Name,
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ChefDesignation? dbChefDesignation = await _context.ChefDesignations.FirstOrDefaultAsync(x => x.Id == id);
            if (dbChefDesignation == null)
            {
                return NotFound();
            }
            return View(new UpdateChefDesignationDto
            {
                Name = dbChefDesignation.Name,
                Id = dbChefDesignation.Id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateChefDesignationDto dto)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ChefDesignation? dbChefDesignation = await _context.ChefDesignations.FirstOrDefaultAsync(x => x.Id == id);
            if (dbChefDesignation == null)
            {
                return NotFound();
            }

            ValidationResult result = await _updateChefDesignationDtoValidator.ValidateAsync(dto);
            if (!result.IsValid)
            {
                foreach (ValidationFailure item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
                return View(dto);
            }

            dbChefDesignation.Name = dto.Name;
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
            ChefDesignation? dbChefDesignation = await _context.ChefDesignations.FirstOrDefaultAsync(x => x.Id == id);
            if (dbChefDesignation == null)
            {
                return NotFound();
            }

            dbChefDesignation.IsDeactive = !dbChefDesignation.IsDeactive;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        #endregion
    }
}
