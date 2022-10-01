using Microsoft.AspNetCore.Identity;
using ZyronChatWebApp.Controllers;
using ZyronChatWebApp.Controllers.Account;
using ZyronChatWebApp.Data;
using ZyronChatWebApp.Models;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace ZyronTests.Controllers
{
    public class User
    {
        static public UserManager<UserModelCustom> UserManagerInstance{ get; set; }
        static public SignInManager<UserModelCustom> SignInManager { get; set; }
        static public UserContext context = new DatabaseConstructorTesting().CreateContext();

        public string SucessReturnControllerActionRedirection = "RedirectToAction";

        [Fact]
        public  void RegisterUser()
        {
            //Data to creation 
            var UserModel = new UserModelCustom() { UserName = "carlos", Email = "carlos@gmail.com" };
            string password = "St;111jklfald";

            //Mock config to when CreateAsync is called
            //The type of parameter of CreateAsync must be a It.IsAny<Type Of Model Required(like UserModelCustom)> 
            //because this manners possibility that any instance of the type choised can be passed as argument and will 
            //work. Case the type configured of CreateAsync mock been a instance, if another instance, same of type, not will work

            //the return of CreateAsync in this case musb be true in Succeeded property of IdentityResult, because of this a create a 
            //class to handle of this. Sucessed its necessary because RegisterUser action have a logic  structure  to verify the property

            var mockManageUsers = new Mock<UserManager<UserModelCustom>>(Mock.Of<IUserStore<UserModelCustom>>(), null, null, null, null, null, null, null, null);
            mockManageUsers.Setup(x => x.CreateAsync(It.IsAny<UserModelCustom>(), It.IsAny<string>())).ReturnsAsync( new IdentityResultMock());

            var control = new UserController(context,null, mockManageUsers.Object);
            var result = control.RegisterUser(UserModel.UserName,password,UserModel.Email);
           
            //verify if the action can got the user. True result is wanted in this test.
            Assert.Contains(this.SucessReturnControllerActionRedirection, result.Result.ToString());

            mockManageUsers.Setup(x => x.CreateAsync(It.IsAny<UserModelCustom>(), It.IsAny<string>())).ReturnsAsync(new IdentityResultMock(false));
            control = new UserController(context, null, mockManageUsers.Object);

            var FailedOfCreateUserBecauseSuccededIsFalse= control.RegisterUser(UserModel.UserName, password, UserModel.Email);
            Assert.Contains("ViewResult", FailedOfCreateUserBecauseSuccededIsFalse.Result.ToString());
        }

    }


    public class FakeUserManager: UserManager<UserModelCustom>
    {
        public FakeUserManager() : base(
                Mock.Of<IUserStore<UserModelCustom>>(),Mock.Of<IOptions<IdentityOptions>>(), 
                Mock.Of<IPasswordHasher<UserModelCustom>>(), Mock.Of<IEnumerable<IUserValidator<UserModelCustom>>>(),
                Mock.Of<IEnumerable<IPasswordValidator<UserModelCustom>>>(),Mock.Of<ILookupNormalizer>() ,
                Mock.Of<IdentityErrorDescriber>(), Mock.Of<IServiceProvider>(), Mock.Of<ILogger<UserManager<UserModelCustom>>>()
            ){ }

        public Mock<FakeUserManager> MockedVersionObject()
        {
            return new Mock<FakeUserManager>();
        }
        
        
    }
    
    //Class to change Succeeded property of IdentityResult. Very useful to mock values.
    public class IdentityResultMock : IdentityResult
    {
        public IdentityResultMock(bool succeeded = true)
        {
            this.Succeeded = succeeded;
        }
    }

    
}