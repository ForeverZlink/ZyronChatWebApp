

using ZyronChatWebApp.Data;
using ZyronChatWebApp.Logics;
using ZyronChatWebApp.Models;


namespace ZyronTests.ModelTesting
{
    public class ChatMessagesLogicTesting
    {
        static public DatabaseConstructorTesting fixture = new DatabaseConstructorTesting();
        static UserContext context = fixture.CreateContext();
        public ChatMessagesLogic chatmessages = new ChatMessagesLogic(context);

        //Definition of user 
        string Email = "carlos@gmail.com";
        

        //Definition of user receiver
        
        string EmailReceiver = "Zelda@gmail.com";
        

        
        [Fact]
        public void GetMessagesOfAmongTwoUsers_All_Ok()
        {
            string Id = Guid.NewGuid().ToString();
            string Username = Guid.NewGuid().ToString();

            string IdReceiver = Guid.NewGuid().ToString();
            string UsernameReceiver = Guid.NewGuid().ToString();
            //Creating the Users
            UserModelCustom UserSender = new UserModelCustom() { UserName = Username, Email = Email, Id = Id };
            UserModelCustom UserReceiver = new UserModelCustom() { UserName=UsernameReceiver,Email=EmailReceiver,Id = IdReceiver};

            //Saving in database 
            context.AddRange(UserSender, UserReceiver);
            context.SaveChanges();

            //Creating new Chat (object to relate users and the messages) 
            var ChatCreatedWithSucess = this.chatmessages.CreateNewChat(Id, IdReceiver);


            //Messages defition
            string Message = "Hello world";

            //Saving the messages
            var SavedWithSucess = this.chatmessages.SaveMessagesChatBetweenTwoUsers(UserSender.Id, UserReceiver.Id, Message);
            Assert.NotNull(SavedWithSucess);

            //Getting the messages
            var MessageList = this.chatmessages.GetMessagesOfAmongTwoUsers(UserSender.Id,UserReceiver.Id);
            //Verify if Messagelist not is null (case be, the method has failed)
            Assert.NotNull(MessageList);

            //Verifying if the message saved in SaveMessagesChatBetweenTwoUsers is same  message returned trough GetMessagesOfAmongTwoUsers
            //This manner its for verify if GetMessagesOfAmongTwoUsers return the corrects messages saved among the users.

            Assert.Contains(Message, MessageList.FirstOrDefault().Message);
            
        }
        [Fact]
        public void GetMessagesOfAmongTwoUsers_Both_Users_Dont_Exists_in_Database()
        {
            string usernameFake = "dont exist";
            string username2Fake = "dont exist";
            var messages = this.chatmessages.GetMessagesOfAmongTwoUsers(usernameFake,username2Fake);
            Assert.Null(messages);
        }


        [Fact]
        public void GetMessagesOfAmongTwoUsers_All_Arguments_Are_Null()
        {

            var messages = this.chatmessages.GetMessagesOfAmongTwoUsers(null, null);
            Assert.Null(messages);
        }
        [Fact]
        public void GetMessagesOfAmongTwoUsers_Dont_Exist_a_ChatMessage_Object_Related_With_The_Both_Users()
        {
            string Id = Guid.NewGuid().ToString();
            string Username = Guid.NewGuid().ToString();

            string IdReceiver = Guid.NewGuid().ToString();
            string UsernameReceiver = Guid.NewGuid().ToString();
            //Creating the Users
            UserModelCustom UserSender = new UserModelCustom() { UserName = Username, Email = Email, Id = Id };
            UserModelCustom UserReceiver = new UserModelCustom() { UserName = UsernameReceiver, Email = EmailReceiver, Id = IdReceiver };

            //Saving in database 
            context.AddRange(UserSender, UserReceiver);
            context.SaveChanges();

            var messages = this.chatmessages.GetMessagesOfAmongTwoUsers(UserSender.Id, UserReceiver.Id);
            Assert.Null(messages);
        }
        [Fact]
        public void Create_New_Chat_All_Ok()
        {
            string Id = Guid.NewGuid().ToString();
            string Username = Guid.NewGuid().ToString();

            string IdReceiver = Guid.NewGuid().ToString();
            string UsernameReceiver = Guid.NewGuid().ToString();
            //Creating the Users
            UserModelCustom UserSender = new UserModelCustom() { UserName = Username, Email = Email, Id = Id };
            UserModelCustom UserReceiver = new UserModelCustom() { UserName = UsernameReceiver, Email = EmailReceiver, Id = IdReceiver };

            //Saving in database 
            context.AddRange(UserSender, UserReceiver);
            context.SaveChanges();

            var ChatCreatedWithSucess = this.chatmessages.CreateNewChat(Id, IdReceiver);
            
            Assert.True(ChatCreatedWithSucess);
            
           
            
           
        }
        [Fact]
        public void Create_New_Chat_But_All_Arguments_Are_null() {
            var NoSucess=this.chatmessages.CreateNewChat(null, null);
            Assert.False(NoSucess);
        }

        [Fact]
        public void Create_New_Chat_User_Receiver_Not_Found_With_The_Id()
        {
            string IdDontExistInDatabase = "2323232332";
            
            string Username = Guid.NewGuid().ToString();

            
            var NoSucessFalseWanted = this.chatmessages.CreateNewChat(Username, IdDontExistInDatabase);
            Assert.False(NoSucessFalseWanted);

        }

        [Fact]
        public void Create_New_Chat_Already_Exists_a_Entity_With_The_Id_of_Both_Users()
        {
            //This verify how is the response of program when its tried 
            //create a object of the type ChatMessages when already exist 
            // a entity with the both users related.
            string Id = Guid.NewGuid().ToString();
            string Username = Guid.NewGuid().ToString();

            string IdReceiver = Guid.NewGuid().ToString();
            string UsernameReceiver = Guid.NewGuid().ToString();
            //Creating the Users
            UserModelCustom UserSender = new UserModelCustom() { UserName = Username, Email = Email, Id = Id };
            UserModelCustom UserReceiver = new UserModelCustom() { UserName = UsernameReceiver, Email = EmailReceiver, Id = IdReceiver };

            //Saving in database 
            context.AddRange(UserSender, UserReceiver);
            context.SaveChanges();
            //Creating the register if dont exist in database 
            var SucessTrueExpected = this.chatmessages.CreateNewChat(Id, IdReceiver);
            Assert.True(SucessTrueExpected);

            //Trying create a object, but he already exists in database the combination.
            var NoSucessFalseWanted = this.chatmessages.CreateNewChat(Id, IdReceiver);
            Assert.False(NoSucessFalseWanted);

        }
    }
    
}