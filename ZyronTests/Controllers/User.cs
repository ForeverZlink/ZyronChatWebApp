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

namespace ZyronTests.Controllers
{
    public class User
    {
        static public UserManager<UserModelCustom> UserManagerInstance{ get; set; }
        static public SignInManager<UserModelCustom> SignInManagerInstance { get; set; }
        static public UserContext context = new DatabaseConstructorTesting().CreateContext();

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
        public UserController controller = new UserController(context, mockedSigninManager.Object, mockedUserManager.Object);
        public UserModelCustom UserModel = new UserModelCustom() { UserName = "carlos", Email = "carlos@gmail.com" };
        string password = "St;111jklfald";
     

        [Fact]
        public async void LoginUser__UserExistsInDatabaseAndNotHaveAnyErrors__AllOccursAsExpected()
        {
            //mocking  
            var mockManageUsers = new Mock<UserManager<UserModelCustom>>(Mock.Of<IUserStore<UserModelCustom>>(), null, null, null, null, null, null, null, null);

            

            //setup when PasswordSignInAsync its called 
            mockedSigninManager.Setup(x => x.PasswordSignInAsync(It.IsAny<UserModelCustom>(),this.password , true, false)).ReturnsAsync(
                new SigninResultMock());

            //Creation of controller of testing
            //Here im are adding the UserModel in context, because of this manner will have a entity 
            //to the controller search in context when necessary
            context.Users.Add(UserModel);
            context.SaveChanges();

            this.controller.Context = context;
            this.controller.SignInManager = mockedSigninManager.Object;


            //Testing. In this test, the method dont have any errors and can log the user 
            //without problems
            var resultSucessOfLogin = await this.controller.LoginUser(this.UserModel.UserName, password);
            var ResultOfLoginTyped =Assert.IsType<RedirectToActionResult>(resultSucessOfLogin);

            //Verify if true are in the field.
            Assert.True(this.controller.ViewBag.LoginWithSucess);

            //Verify if the controller redirect its the expected
            Assert.Contains("DashManagement", ResultOfLoginTyped.ControllerName);
        }


        [Fact]
        public async void LoginUser__UsernamePassedAsArgumentIsNull__TheActionReturnAViewResult()
        {
            //Arrange
           
            //Not its necessary pass data or a instance of anything, because the action will verify 
            //if username its null and will return a view result.

            var resultUsernameNullReturnView = await  this.controller.LoginUser(null, password);
            
            var viewResultUsernameNull= Assert.IsType<ViewResult>(resultUsernameNullReturnView);


           
           
            
        }

        [Fact]
        public async void LoginUser__UsernameNotExistsInDatabase__ViewResultExpect()
        {
            
            var resultUsernameNotAreInContext = await this.controller.LoginUser("Carlostas", password);
            
            Assert.IsType<ViewResult>(resultUsernameNotAreInContext);

        }

        [Fact]
        public async void LoginUser__UsernameExistsInDatabaseButThePasswordItsNull()
        {
            var resultUserNameExistInDatabaseButPasswordIsNull = await this.controller.LoginUser(UserModel.UserName, null);
            Assert.IsType<ViewResult>(resultUserNameExistInDatabaseButPasswordIsNull);
        }

        [Fact]
        public  async void LoginUser__PasswordNotIsCorrect()
        {
            string passwordIncorrect = "hello";
           
            mockedSigninManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<UserModelCustom>(), It.IsAny<string>(), true, false)).ReturnsAsync(new SigninResultMock(false));
            this.controller.SignInManager = mockedSigninManager.Object;
           

            //Case the username exists in database, although the password its incorrect
            var resultPasswordIsIncorrect = await this.controller.LoginUser(UserModel.UserName, passwordIncorrect);

            Assert.IsType<ViewResult>(resultPasswordIsIncorrect);
        }

        [Fact]
        public async void LogoutUser__AllOccursAsExpected()
        {
            
            var result = this.controller.LogoutUser();
            Assert.NotNull(result);
        }

       
        [Fact]
        public async void RegisterUser__UserCantBeRegistered()
        {
            mockedUserManager.Setup(x => x.CreateAsync(It.IsAny<UserModelCustom>(),It.IsAny<string>())).ReturnsAsync(new IdentityResultMock(false));
            controller.UserManagement = mockedUserManager.Object;

            var ResultUserCantBeRegistered = await controller.RegisterUser(UserModel.UserName, password, UserModel.Email);
            Assert.NotNull(ResultUserCantBeRegistered);

            //Verify type of return 
            var ResultUserCantBeRegisteredTyped= Assert.IsType<ViewResult>(ResultUserCantBeRegistered);

     
            //Verify ViewBag field 
            Assert.False(controller.ViewBag.UserCreatedWithSucess); 
        }
        
    }

    public class SigninResultMock : SignInResult
    {
        public SigninResultMock(bool succeeded = true)
        {
            this.Succeeded=succeeded;
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