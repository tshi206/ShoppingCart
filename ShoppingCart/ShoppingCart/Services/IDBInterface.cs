using ShoppingCart.Services;

namespace ShoppingCart.Services
{
    public interface IDBInterface
    {
        // SQLiteConnection CreateConnection(); establish the connection in shared code
        string GetLocalDBPath(string filename, string extension);
    }
}
