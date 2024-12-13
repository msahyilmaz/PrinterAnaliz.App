using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using PrinterAnaliz.Application.Extensions;
using PrinterAnaliz.Application.Interfaces.Repositories;

namespace PrinterAnaliz.Application.Features.Auth.Command.ForgetPassword
{
    public class ForgetPasswordHandler : IRequestHandler<ForgetPasswordRequest, ForgetPasswordResponse>
    {
        private readonly IUserRepository rpUser; 
        private readonly IConfiguration configuration;

        public ForgetPasswordHandler(IUserRepository rpUser,   IConfiguration configuration)
        {
            this.rpUser = rpUser;  
            this.configuration = configuration;
        }
        public async Task<ForgetPasswordResponse> Handle(ForgetPasswordRequest request, CancellationToken cancellationToken)
        {
             
            var qUser = await rpUser.FirstOrDefaultAsync(w => (w.UserName == request.EmailOrUserName || w.Email == request.EmailOrUserName) && !w.IsDeleted);
            if (qUser == null)
                throw new ValidationException("Kullanıcı bilgilerinizi kontrol ediniz.");
            if (qUser.ForgetPasswordQueryDate >= DateTime.Now)
                throw new ValidationException("Yeni şifre yenileme talebi için beklemeniz gereken süre bitmiş değil.");


            var forgetPasswordKey = $"{qUser.Id}|{qUser.UserName}|{DateTime.Now.AddHours(1)}";
            var forgetPasswordSecretKey = PasswordEncryptor.Encrypt(forgetPasswordKey);

            string cfgFromMail = configuration["MailConfig:FromMail"];
            string cfgFromMailPassword = configuration["MailConfig:FromMailPassword"];
            string cfgSmtpServer = configuration["MailConfig:SmtpServer"];
            string cfgSmtpDomain = configuration["MailConfig:SmtpDomain"];
            string cfgTitle = configuration["MailConfig:Title"]; 
            _ = int.TryParse(configuration["MailConfig:SmtpPort"], out int cfgSmtpPort);

            var Subject = $"{cfgTitle} Şifre Yenileme E-Postası."; 
            var Body = await createBodyForgetPasswordEMail(request.PasswordResetUrl, forgetPasswordSecretKey);

          
            var To = new List<string>() { qUser.Email};
             
            if (SendMailExtension.SendEmail(cfgFromMail, To, Subject, Body, cfgFromMail, cfgFromMailPassword, cfgSmtpServer, true, cfgSmtpPort))
            {
                qUser.ForgetPasswordQueryDate = DateTime.Now;
                await rpUser.UpdateAsync(qUser);
                return new() { StatusMessage = $"{String.Join(", ", To.ToArray())} adresine şifre yenileme için e-posta gönderilmiştir." };
            }
            else 
            {
                throw new Exception("E-Posta gönderilemedi.");
            }

        }

        private async Task<string> createBodyForgetPasswordEMail(string resetUrl, string resetToken)
        {
            string forgetPasswordEmailTemplate = Path.Combine(Directory.GetCurrentDirectory(), configuration["MailConfig:ForgetPasswordEmailTemplate"]);
            StreamReader EpostaTemplateOku = new StreamReader(forgetPasswordEmailTemplate);
            string EpostaTemplate = EpostaTemplateOku.ReadToEnd();
            EpostaTemplateOku.Close();
            EpostaTemplate = EpostaTemplate.Replace("[RP_PASSWORD_RESET_LINK]", $"{resetUrl}?token={resetToken}");

            return EpostaTemplate;
        }
    }
}
