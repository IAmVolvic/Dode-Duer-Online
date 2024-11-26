using DataAccess.Models;
using DataAccess.Types.Enums;
using Service.Services.Interfaces;

namespace API.Automation;

public class AdminUser(IUserService userService)
{
   /*This creates an admin account if none exists.*/

   public void RegisterAdminUser()
   {
      var user = new User
      {
         Name = Environment.GetEnvironmentVariable("ADMIN_NAME"),
         Email =Environment.GetEnvironmentVariable("ADMIN_EMAIL"),
         Phonenumber = Environment.GetEnvironmentVariable("ADMIN_PHONENUMBER"),
         Passwordhash = Environment.GetEnvironmentVariable("ADMIN_PASSWORD"),
         Enrolled = UserEnrolled.True,
         Balance = 5000,
         Status = UserStatus.Active,
         Role = UserRole.Admin,
      };
      
      userService.NewAdmin(user);
   }
}