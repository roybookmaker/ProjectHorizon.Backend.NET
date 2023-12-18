using System;

using MailKit.Net.Smtp;
using MailKit;
using ProjectHorizon.Shared.Library.Common;
using MimeKit;

namespace ProjectHorizon.Shared.Library.Service
{
    public class EmailService
    {
        public static async Task<CommandResult> EmailServiceData(string emailtype, string recipientEmail)
        {
            try
            {
                string emailSubject = "";
                string emailBody = "";

                switch (emailtype)
                {
                    case "register":
                        emailSubject = "Welcome to Project Horizon";
                        emailBody = "Thank you for registering with us. We hope you enjoy your stay.";
                        break;
                    case "recovery":
                        string https = "https://localhost:44300/Account/Recovery?token="; //need to add the recovery string here
                        emailSubject = "Password Recovery";
                        emailBody = $@"<div class='container' style='width: 100%;'>" +
                                        "<div class='header' style='padding:20px 0; background-color:#161616; text-align: center; font-family: \"Roboto\", sans-serif; font-weight:bolder; font-size:14px; color:#fff;'><b>PROJECT/HORIZON.RECOVERY</b></div>" +
                                        "<div class='body' style='padding:20px 15px; background-color:#131313; text-align: center; font-family: \"Roboto\", sans-serif; font-size:12px; color:#fff;'>" +
                                            "<p>You receive this email because we receive a request to reset your account password.<br>" +
                                            "<b style='color:red;'>In case you did not request it, please simply ignore this email.</b></p>" +

                                            "<p>To reset your password, please click the link below.</p>" +
                                            "<a href='" + https + "'>Reset your password</a>" +

                                            "<br><br><br><br>" +
                                            "<hr style='height:1px; border:none; background-color:#333; margin:5px 0;'>" +
                                            "<p>Thank you.<br>" +
                                            "Best Regards</p>" +
                                            "<b style='color:#3498db;'>Arnold</b>" +
                                            "<p>Project/Horizon Developer</p>" +
                                        "</div>" +
                                    "</div>";
                        break;
                    default:
                        emailSubject = "Test Email";
                        emailBody = "This is a test email sent using .NET";
                        break;
                }

                await Task.Run(() => SendEmail(recipientEmail, emailSubject, emailBody));
                
                var result = new CommandResult
                {
                    IsSuccessful = true,
                    Message = "Email sent successfully!"
                };

                return result;
            }
            catch (Exception ex)
            {
                var result = new CommandResult
                {
                    IsSuccessful = true,
                    Message = ex.Message
                };

                return result;
            }
            
        }

        private static void SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Arnold", "flesi.arnoldi@gmail.com"));
                message.To.Add(new MailboxAddress("Roy", "reversed.horizon.studio@gmail.com"));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;

                message.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    smtp.Authenticate("flesi.arnoldi@gmail.com", "@Arnold1991");

                    smtp.Send(message);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error: {ex.Message}");
            }
        }
    }
}
