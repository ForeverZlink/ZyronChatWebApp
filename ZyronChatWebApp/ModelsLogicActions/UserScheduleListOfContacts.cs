using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Logics
{
    public class UserScheduleListOfContactsLogic
    {
        private UserContext Context;
        public UserScheduleListOfContactsLogic(UserContext userContext)
        {
            this.Context = userContext;
        }

        public bool AddNewContact(string IdUser, string NameOfContact, string Surname)
        {
            //Seach in database the object create when the user was created in the first time. 
            //Here its possible find because when the user was created a new UserScheduleListOfContacts object receive a UserId
            //This Userid its the prove of relationship among both entitys
            //Not its possible exists two UserScheduleListOfContacts with the same UserId
            if(NameOfContact ==null && IdUser==null)
            {
                return false;
            }
            var UserScheduleListOfContactsInstance = this.Context.UserScheduleListOfContacts.FirstOrDefault(x => x.UserId == IdUser);
            if (UserScheduleListOfContactsInstance == null)
            {
                return false;
            } 

            var ContactInformation = this.Context.ContactInformations.FirstOrDefault(
                x => x.IdUserScheduleListOfContacts == UserScheduleListOfContactsInstance.Id
                && x.UsernameOfIdentification == NameOfContact);

            if (ContactInformation == null)
            {
                var ContactInfo = new ContactInformations()
                {
                    UsernameOfIdentification = NameOfContact,
                    Surname = Surname,
                    IdUserScheduleListOfContacts = UserScheduleListOfContactsInstance.Id
                };

                this.Context.Add(ContactInfo);
                this.Context.SaveChanges();

                return true;


            }
            return false;
        }
    }
}