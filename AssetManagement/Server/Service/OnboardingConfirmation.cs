using AssetManagement.DataContext;
using AssetManagement.Server.Service;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Server.EmailService;
using System.Security.Cryptography.Xml;
using AssetManagement.Dto.Models;

namespace AssetManagement.Server.Service
{
    public class OnboardingConfirmation
    {
        private AppDbContext _dbcontext;
        private readonly IMailService _mailService;
        private readonly ILogger _logger;
        public OnboardingConfirmation(AppDbContext dbcontext, IMailService mailService, ILogger<OnboardingConfirmation> Logger )
        {
            _dbcontext = dbcontext;
            _mailService = mailService;
            _logger= Logger;
        }
        public async Task OnBoardingquery()
        {
            try
            {
                var employees = _dbcontext.Employee;
                var onboardmail = _dbcontext.EmployeeOnboarding.Where(o => o.TempDateOfJoin.AddDays(1) < DateTime.Now && !string.IsNullOrEmpty(o.Reference) && o.IsReplied == true &&
    (string.IsNullOrEmpty(o.Joinstatus) || o.Joinstatus == "Tentative"));
                foreach (var employee in onboardmail)
                {
                    var subject = $"Onboarding Confirmation Mail for {employee.Name}";

                    if (employee.Reference != "NA")
                    {
                        var url = $"https://localhost:7053/OnBoardingConfirmationDetails/{employee.Id}";
                        var referName = employees.Where(o=>o.EmailId.ToLower() == employee.Reference.ToLower()).Select(o=>o.EmployeeName).FirstOrDefault();
                        string body = $"Hi {referName} ,<br/><br/>" +
                                      $"This is to kindly request your confirmation regarding the candidate <b>{employee.Name}</b>, whom you had referred for employment.<br/>" +
                                      $"Please confirm whether this individual has joined our organization.<br/><br/>" +
                                      $"You can Update the candidate's details and current status by clicking on the following link: " +
                                      $"<a href=\"{url}\">click  here</a>";

                         _mailService.SendEmailAsync(employee.Reference, subject, body);
                    }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in onboarding query: {ex.Message}");
            }
        }


    }
}





//private AppDbContext _dbContext;
//public OnboardingConfirmation(AppDbContext dbContext)
//{
//    _dbContext = dbContext;

//}
//public void OnBoardQuery()
//{
//    try
//    {
//        var onboards = _dbContext.EmployeeOnboarding.Where(o => o.TempDateOfJoin.AddDays(1) < DateTime.Now);
//    }
//    catch (Exception ex)
//    {


//    }

//}