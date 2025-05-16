using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Restoran.DAL.Contexts;
using Restoran.DTOs.MemberDesignationDtos;

namespace Restoran.DAL.Validatos.MemberDesignationDtoValidator
{
    public class UpdateChefDesignationDtoValidator : AbstractValidator<UpdateChefDesignationDto>
    {
        public UpdateChefDesignationDtoValidator(AppDbContext context)
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
