using FluentValidation;
using Restoran.DTOs.ChefDtos;
using Restoran.DTOs.MemberDtos;

namespace Restoran.DAL.Validatos.MemberDtoValidator
{
    public class CreateChefDtoValidator : AbstractValidator<CreateChefDto>
    {
        public CreateChefDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .Length(5, 200).WithMessage("Full name must be between 5 and 200 characters.");

            //RuleFor(x => x.TwitterUrl)
            //    .Matches(@"^https?://(www\.)?twitter\.com/.*$").WithMessage("Invalid Twitter URL format.");

            //RuleFor(x => x.FacebookUrl)
            //    .Matches(@"^https?://(www\.)?facebook\.com/.*$").WithMessage("Invalid Facebook URL format.");

            //RuleFor(x => x.InstagramUrl)
            //    .Matches(@"^https?://(www\.)?instagram\.com/.*$").WithMessage("Invalid Instagram URL format.");

            RuleFor(x => x.ChefDesignationId)
                .NotNull()
                .NotEmpty()
                .NotEqual(0)
                .MustAsync(async (value, cancelletion) =>
                {
                    if (value < 0)
                    {
                        return await Task.FromResult(false);
                    }
                    return await Task.FromResult(true);
                });

            RuleFor(x => x.Photo)
                .NotNull()
                .NotEmpty()
                .MustAsync(async (photo, cancellation) =>
                {
                    if (photo.ContentType.Contains("image/"))
                    {
                        return await Task.FromResult(true);
                    }
                    return await Task.FromResult(false);
                }).WithMessage("File is not image")
                .MustAsync(async (photo, cancellation) =>
                {
                    if (photo.Length < 10000000)
                    {
                        return await Task.FromResult(true);
                    }
                    return await Task.FromResult(false);
                }).WithMessage("File is large than 10MB");
        }
    }
}
