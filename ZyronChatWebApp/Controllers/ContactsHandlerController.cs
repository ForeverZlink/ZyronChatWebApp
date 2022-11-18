using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Xml.Linq;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Logics;
using ZyronChatWebApp.Models;

namespace ZyronChatWebApp.Controllers
{
    public class ContactsHandlerController : Controller
    {
        public UserContext Context { get; set; }
        public ChatMessagesLogic ChatMessagesLogic { get; set; }
        public ILogger<ContactsHandlerController> Logger { get; set; }
        public UserScheduleListOfContactsLogic UserListOfContactsLogic { get; set; }
        public ContactsHandlerController(UserContext dbContext, ILogger<ContactsHandlerController> logger)
        {
            this.Context = dbContext;
            this.ChatMessagesLogic =  new ChatMessagesLogic(dbContext);
            this.Logger = logger;
            this.UserListOfContactsLogic = new UserScheduleListOfContactsLogic(dbContext);
        }


        public async Task<IActionResult> Index()
        {
            //This action has the mission of show the user alls contacts that he have 
            //At this way, all contacts of the user are diplayed.
            this.Logger.LogInformation("Verifying if the user is logged");
            if (User.Identity.IsAuthenticated) {
                this.Logger.LogInformation("User is logged");

                this.Logger.LogInformation("Getting user object ");
                var user = this.Context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

                this.Logger.LogInformation("Getting the list of contact of the users ");
                var listOfContacts = this.Context.UserScheduleListOfContacts.Include("ContactsInformations").FirstOrDefault(x => x.UserId == user.Id);

                this.Logger.LogInformation("Verifying if the list of contact are valid");
                if (listOfContacts != null)
                {
                    this.Logger.LogInformation("Sucess, list of contact is valid");

                    this.Logger.LogInformation("Returning the view");
                    return View(listOfContacts);
                }
                
                
            }
            this.Logger.LogInformation("User not is authenticated ");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewContact(string NameOfContact, string Surname)
        {
            
            this.Logger.LogInformation("Starting controller",DateTime.UtcNow);
            this.Logger.LogInformation("Verifying if the user is logged", DateTime.UtcNow);
            if (User.Identity.IsAuthenticated)
            {
                this.Logger.LogInformation("User is logged", DateTime.UtcNow);

                this.Logger.LogInformation("Getting user caller username", DateTime.UtcNow);
                var username = User.FindFirstValue(ClaimTypes.Name);

                this.Logger.LogInformation("Searching user caller in database ", DateTime.UtcNow);
                var user = this.Context.Users.FirstOrDefault(x => x.UserName == username);

                this.Logger.LogInformation("Searching user to send in databse ", DateTime.UtcNow);
                var userToAdd = this.Context.Users.FirstOrDefault(x => x.UserName == NameOfContact);

                this.Logger.LogInformation("Verifyning if both exists", DateTime.UtcNow);
                if (user == null || userToAdd==null)
                {
                    this.Logger.LogInformation("One user, or both are not registered", DateTime.UtcNow);
                    return NotFound();
                }
                else
                {
                    this.Logger.LogInformation("Both users exists");

                    this.Logger.LogInformation("Trying create a new contact to relate with the user caller", DateTime.UtcNow);
                    var Sucess =this.UserListOfContactsLogic.AddNewContact(user.Id, NameOfContact, Surname);
                    this.Logger.LogInformation("End of the process of add the contact", DateTime.UtcNow);

                    this.Logger.LogInformation("Verifying if the contact was added", DateTime.UtcNow);
                    if (Sucess == false) {
                        this.Logger.LogInformation("Contact not added with sucess", DateTime.UtcNow);
                        return NotFound();
                    }

                    this.Logger.LogInformation("Creating a new Chat between if contact and the user", DateTime.UtcNow);

                    bool ChatCreatedWithSucess= this.ChatMessagesLogic.CreateNewChat(user.Id, userToAdd.Id);
                    this.Logger.LogInformation("End of the operation of creating a new chat ", DateTime.UtcNow);

                    this.Logger.LogInformation("Verifying if was sucess", DateTime.UtcNow);
                    if (ChatCreatedWithSucess)
                    {
                        this.Logger.LogInformation("Operation was the sucess. Not errors found", DateTime.UtcNow);

                        this.Logger.LogInformation("Setting propertys", DateTime.UtcNow);
                        ViewBag.ContactAddedWithSucess = true;

                        this.Logger.LogInformation("Returning the view ", DateTime.Now);
                        return View();
                    }
                    else
                    {
                        this.Logger.LogInformation("Error, wasnt possible create the object of the type Chat");
                        ViewBag.ContactAlreadyExists = true;
                    }

                    
                }
                    
                
            }
            this.Logger.LogInformation("Returning not found", DateTime.UtcNow);
            return NotFound();


        }
            




    }
}
