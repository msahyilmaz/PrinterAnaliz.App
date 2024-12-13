using FluentValidation;
using PrinterAnaliz.Application.Extensions;

namespace PrinterAnaliz.Application.Features.Users.Queries.GetAllUserRoles
{
    public class GetAllUserRolesQueryValidator:AbstractValidator<GetAllUserRolesQueryRequest>
    {
        public GetAllUserRolesQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().Must(x=>x.ConvertSPIdList()!="").WithMessage("En az bir kullanıcının id bilgisi gereklidir."); ;
        }
    }
}
