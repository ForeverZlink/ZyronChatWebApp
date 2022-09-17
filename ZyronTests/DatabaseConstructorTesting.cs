using ZyronChatWebApp;
using Microsoft.EntityFrameworkCore;
using ZyronChatWebApp.Data;

public class DatabaseConstructorTesting
{
    private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";
    private const string ConnectionStringMvcIndependentObjectsOfProductsContext = @"Server=(localdb)\mssqllocaldb;Database=TestMvcIndependentObjectsOfProductsContext;Trusted_Connection=True";
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