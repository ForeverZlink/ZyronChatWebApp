using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Xml.Linq;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Controllers
{
    public class ContactsHandlerController : Controller
    {
        public UserContext Context { get; set; }
        public ChatMessagesController ChatMessagesController = new ChatMessagesController();
        public ContactsHandlerController(UserContext dbContext)
        {
            this.Context = dbContext;

        }


        public async Task<IActionResult> Index()
        {
            //This action has the mission of show the user alls contacts that he have 
            //At this way, all contacts of the user are diplayed.
            if (User.Identity.IsAuthenticated) {
            
                var user = this.Context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                var listOfContacts = this.Context.UserScheduleListOfContacts.Include("ContactsInformations").FirstOrDefault(x => x.UserId == user.Id);
                return View(listOfContacts);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewContact(string NameOfContact, string Surname)
        {
            if (User.Identity.IsAuthenticated)
            {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = this.Context.Users.FirstOrDefault(x => x.UserName == username);
                var userToAdd = this.Context.Users.FirstOrDefault(x => x.UserName == NameOfContact);
           
                if (user == null || userToAdd==null)
            {
                return NotFound();
            }
            else
            {
                //Seach in database the object create when the user was created in the first time. 
                //Here its possible find because when the user was created a new UserScheduleListOfContacts object receive a UserId
                //This Userid its the prove of relationship among both entitys
                //Not its possible exists two UserScheduleListOfContacts with the same UserId
                var UserScheduleListOfContactsInstance = this.Context.UserScheduleListOfContacts.FirstOrDefault(x => x.UserId == user.Id);


                    var ContactInfo = new ContactInformations() { UsernameOfIdentification = NameOfContact, Surname = Surname, IdUserScheduleListOfContacts = UserScheduleListOfContactsInstance.Id };
                
                this.Context.Add(ContactInfo);
                this.Context.SaveChanges();

                
                return RedirectToAction("Index","DashManagement");
            }
            

            }
            return View();




        }
    }
}