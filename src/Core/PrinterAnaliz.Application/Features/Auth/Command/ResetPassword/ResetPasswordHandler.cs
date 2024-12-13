using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.Repositories;
using PrinterAnaliz.Domain.Entities;

namespace PrinterAnaliz.Application.Features.Auth.Command.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, ResetPasswordResponse>
    {
        private readonly IUserRepository rpUser;
        private readonly IConfiguration configuration;

        public ResetPasswordHandler(IUserRepository rpUser, IConfiguration configuration)
        {
            this.rpUser = rpUser;
            this.configuration = configuration;
        }
        public async Task<ResetPasswordResponse> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {

            var resetPasswordToken = PasswordEncryptor.Decrypt(request.ResetToken); 
            var resetPasswordSplit = resetPasswordToken.Split("|");

            _= Int64.TryParse(resetPasswordSplit[0],out long requestUserId);
            string requestUserName = resetPasswordSplit[1] ?? "";
            DateTime requestEndDate = DateTime.Parse(resetPasswordSplit[2]);

            if (requestUserId == 0)
                throw new ValidationException("Kullanıcı bilgisi bulunamadı.");
            if (requestEndDate< DateTime.Now)
                throw new ValidationException("Şifre yenileme bağlantısının süresi sona ermiş.");
            var qUser = await rpUser.FirstOrDefaultAsync(w => w.Id == requestUserId && !w.IsDeleted);
            if (qUser == null)
                throw new ValidationException("Kullanıcı bulunamadı.");
            if (qUser.ForgetPasswordQueryDate is null || qUser.ForgetPasswordQueryDate>= requestEndDate)
                throw new ValidationException("Şifre yenileme bağlantısının daha önce kullanılmış.");

            var newPassword = PasswordEncryptor.Encrypt(request.Password);

            qUser.ForgetPasswordQueryDate = null;
            qUser.Password = newPassword;
            

             await rpUser.UpdateAsync(qUser);
            return new() { StatusMessage = "Şifre yenileme başarılı." };

        }

    }
}
