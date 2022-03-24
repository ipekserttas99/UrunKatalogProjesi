using Hangfire;
using Hangfire.Server;
using MezuniyetProjesi.Shared.Services;
using System;

namespace MezuniyetProjesi.Jobs
{
    public class SendEmailJob : ISendEmailJob
    {
        public IMailService mailService;

        public SendEmailJob(IMailService mailService)
        {
            this.mailService = mailService;
            
                
        }
        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Fail)] // 5 kez yapmayı dene yapamazsan fail tablosuna at
        public void DoLogInJob(string recipientEmail, string recipientFirstName)
        {
            
            mailService.SendLogInEmailAsync(recipientEmail, recipientFirstName); // mail yollama 

        }

        public void DoRegisterJob(string recipientEmail, string recipientFirstName)
        {
            mailService.SendRegisterEmailAsync(recipientEmail, recipientFirstName);
        }
    }

    
}
