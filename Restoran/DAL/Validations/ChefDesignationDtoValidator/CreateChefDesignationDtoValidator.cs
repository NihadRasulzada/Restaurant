using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Restoran.DAL.Contexts;
using Restoran.DTOs.MemberDesignationDtos;

namespace Restoran.DAL.Validatos.ChefDesignationDtoValidator
{
    public class CreateChefDesignationDtoValidator : AbstractValidator<CreateChefDesignationDto>
    {
        public CreateChefDesignationDtoValidator(AppDbContext context)
        {
            RuleFor(x => x.Name)
                 .NotEmpty()
                 .NotNull()
                 .MustAsync(async (name, cancellation) =>
                 {
                     bool exists = await context.ChefDesignations.AnyAsync(md => md.Name == name);
                     return !exists;
                 });
        }
    }

}
