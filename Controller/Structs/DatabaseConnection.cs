using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Controller.Structs {
    public class DatabaseConnection {
        private string server;
        private string databaseName;
        private string username;
        private string password;
        private string connectionString;

        private MySqlConnection connection;

        public DatabaseConnection() { 
        
        }

        public void Close() { 
            connection.Close();
        }

        public bool Connect() {
            try { 
                if(connection != null) {
                    connectionString = string.Format("Server{0}; database={1}; UID={2}; password={3}", server, databaseName);
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                }
                return true;
            }
            catch(Exception e) {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public bool IsConnected { get; set; }
        public string Server { get { return server; } set { server = value; } }
        public string DatabaseName { get { return databaseName; } set { databaseName = value; } }
        public string Username { get { return username; } set { username = value; } }
        public string Password { get { return password; } set { password = value; } }
        public MySqlConnection Connection { get { return connection; } }
        public string ConnectionString { get { return connectionString; } }
    }
}
