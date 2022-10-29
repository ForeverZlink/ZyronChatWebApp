﻿using System;
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

        public DbSet<UserScheduleListOfContacts> UserScheduleListOfContacts { get; set; }
        public DbSet<ContactInformations> ContactInformations { get; set; }
        public DbSet<ChatMessages> ChatMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //One to one
            modelBuilder.Entity<UserModelCustom>()
                .HasOne<UserScheduleListOfContacts>(s => s.UserScheduleListOfContacts)
                .WithOne(ad => ad.User)
                .HasForeignKey<UserScheduleListOfContacts>(ad => ad.UserId);


            //One to many
            modelBuilder.Entity<UserScheduleListOfContacts>()
                .HasMany(x => x.ContactsInformations)
                .WithOne(x => x.UserScheduleListOfContacts)
                .HasForeignKey(x=>x.IdUserScheduleListOfContacts);


            modelBuilder.Entity<ChatMessages>(

                entity =>
                {

                    entity.HasOne(x => x.UserReceiver).WithMany(x => x.UserReceiver)
                        .HasForeignKey(x=>x.IdUserReceiver).OnDelete(DeleteBehavior.NoAction);
                    entity.HasOne(x => x.UserSender).WithMany(x => x.UsersSender)
                        .HasForeignKey(x=>x.IdUserSender).OnDelete(DeleteBehavior.NoAction);
                });


            modelBuilder.Entity<Messages>(

                entity =>
                {

                    entity.HasOne(x => x.ChatMessages).WithMany(x => x.MessagesList).HasForeignKey(x=>x.ChatMessagesId);
                    
                });


            base.OnModelCreating(modelBuilder);

        }

    }

}
