using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZyronChatWebApp.Models;


namespace ZyronChatWebApp.Data
{
    public class UserContext:IdentityDbContext<IdentityUser>
    {
        public UserContext(DbContextOptions<UserContext> options):base(options)
        { }
      
    }

}
