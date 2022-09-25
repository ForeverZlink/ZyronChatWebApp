using ZyronChatWebApp;
using Microsoft.EntityFrameworkCore;
using ZyronChatWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public class DatabaseConstructorTesting
{
    private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=TestingUser;Trusted_Connection=True";
    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public DatabaseConstructorTesting()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();


                }

                _databaseInitialized = true;


            }
        }
    }

    public UserContext CreateContext()
    {
        return new UserContext(new DbContextOptionsBuilder<UserContext>()
                .UseSqlServer(ConnectionString)
                .Options);

    }
    


}