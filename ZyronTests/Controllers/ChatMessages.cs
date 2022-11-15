using Microsoft.AspNetCore.Identity;
using ZyronChatWebApp.Controllers;
using ZyronChatWebApp.Controllers.Account;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector.InProcDataCollector;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Security.Principal;
using System.Linq.Expressions;
using System.Net.Mime;
using Microsoft.AspNetCore.Server.IIS;

namespace ZyronTests.Controllers
{
    public class ChatMessages
    {
        static public Mock<UserManager<UserModelCustom>> mockedUserManager = new Mock<UserManager<UserModelCustom>>(
           Mock.Of<IUserStore<UserModelCustom>>(), null, null, null, null, null, null, null, null);

        static public Mock<SignInManager<UserModelCustom>> mockedSigninManager = new Mock<SignInManager<UserModelCustom>>(
            mockedUserManager.Object,
              Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<UserModelCustom>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<ILogger<SignInManager<UserModelCustom>>>(),
                Mock.Of<IAuthenticationSchemeProvider>(),
                Mock.Of<IUserConfirmation<UserModelCustom>>()

        );

        
        static public UserContext context = new DatabaseConstructorTesting().CreateContext();
        public ChatMessagesController controller = new ChatMessagesController( context,null);
        [Fact]
        public void ChatWithASpecificUser__UsersIsAuthenticate__UserToSendandUserNotAreNull__ChatNotIsNull__AllOccursWithSucess()
        {

            //this test need of that the user is authenticate, because of this its needed create a new instance of 
            // the class and override the value of field.
            string nameOfUser = "Carlos";
            var IdentityAlteredWithIsAuthenticateTrue = new IdentityAltered() { IsAuthenticated=true,Name=nameOfUser};

            var User= new UserModelCustom() { UserName=nameOfUser,Email="carlos@gmail.com" };
            
            var UserToSend = new UserModelCustom() { UserName="Testa",Email="Hello@gmail.com"};
            context.Add(User);
            context.Add(UserToSend);
            context.SaveChanges();
            

            var chat = new ZyronChatWebApp.Models.ChatMessages() {IdUserSender=User.Id,IdUserReceiver=UserToSend.Id};
            context.Add(chat);
            context.SaveChanges();

            this.controller.Context = context;

           
            var claim = new Claim(ClaimTypes.Name, User.UserName);
            var identityOfUser = new ClaimsIdentity(new[] { claim }, "hello");
            
            // this uses basic auth
            var ClaimsPrincipalInstance = new ClaimsPrincipal(identityOfUser);
             
            var HttpContextDefault =new DefaultHttpContext();

            HttpContextDefault.User = ClaimsPrincipalInstance;
            this.controller.ControllerContext.HttpContext = HttpContextDefault;  

            
            
        }




    }
}


//This class its for alter the field, because the IIdentity interface dont allow setup this field.
public class IdentityAltered:IIdentity
{

    public bool IsAuthenticated { get; set; }

    public string Name { get; set; }

    public string? AuthenticationType => throw new NotImplementedException();
}


//Has the objetive of simulate the ClaimsPrincipal class, because the field User of controler, has 
//the type ClaimsPrincipal.
public class ClaimsPrincipalAltered : ClaimsPrincipal
{
    public ClaimsPrincipalAltered(IdentityAltered identity)
    {

    }

}
//this class its for simulate ChatMessagesController, because of this manner its possible change many keywords and 
//acessors and visibility of code. 
public class ChatMessageAlteredController : ChatMessagesController
{
    public ChatMessageAlteredController(ILogger<ChatMessagesController> logger, UserContext dbContext) :base(logger,dbContext,null,null)
    {
        
    }

  

}