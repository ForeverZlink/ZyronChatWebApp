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
    public class UserContext:IdentityDbContext<UserModelCustom>
    {
        public UserContext(DbContextOptions<UserContext> options):base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //One to one
            modelBuilder.Entity<UserModelCustom>()
                .HasOne<UserScheduleListOfContacts>(s => s.ListOfContacts)
                .WithOne(ad => ad.User)
                .HasForeignKey<UserScheduleListOfContacts>(ad => ad.UserId);

            //One to many
            modelBuilder.Entity<UserScheduleListOfContacts>()
                .HasMany(x => x.ContactsInformations)
                .WithOne(x => x.UserScheduleListOfContacts);
            base.OnModelCreating(modelBuilder);

        }

    }

}
