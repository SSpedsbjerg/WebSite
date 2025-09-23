using Controller.Structs;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Controller {

    public enum DatabaseProcessCode {
        Success,
        UserNotUnique,
        UserDoesNotExist,
        InvalidPassword,
        FailedToReadDatabase,
        FailedToCreateTable
    }

    public static class DatabaseController {
        private static DatabaseConnection? database;
        private const int saltLength = 10;

        public static DatabaseConnection Database { get { return database; } set { database = value; } }

        public static DatabaseProcessCode InitAllTables() {
            if (InitUserTable() == DatabaseProcessCode.Success && InitPostTable() == DatabaseProcessCode.Success) return DatabaseProcessCode.Success;
            else return DatabaseProcessCode.FailedToCreateTable;

        }

        public static DatabaseProcessCode InitUserTable() {
            database.Connect();
            try {
                string query =
                @"CREATE TABLE users IF NOT EXIST (
                id INT AUTO_INCREMENT PRIMARY KEY,
                username VARCHAR(255) NOT NULL UNIQUE,
                password VARBINARY(255) NOT NULL,
                admin BOOLEAN NOT NULL DEFAULT FALSE
            );
            ";
                MySqlCommand command = new MySqlCommand(query, database.Connection);
                command.ExecuteNonQuery();
                return DatabaseProcessCode.Success;
            }
            catch(Exception e) { 
                Console.WriteLine(e.ToString());
                return DatabaseProcessCode.FailedToCreateTable;
            }

        }

        public static DatabaseProcessCode InitPostTable() {
            database.Connect();
            try {
                string query = @"CREATE TABLE posts IF NOT EXIST (
                id INT AUTO_INCREMENT PRIMARY KEY,
                author VARCHAR(255) NOT NULL,
                post_date VARCHAR(255) NOT NULL,
                description VARCHAR(255),
                content VARCHAR(4096) NOT NULL,
                title VARCHAR(255) NOT NULL
            );
            ";
                MySqlCommand command = new MySqlCommand(query, database.Connection);
                command.ExecuteNonQuery();
                return DatabaseProcessCode.Success;
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return DatabaseProcessCode.FailedToCreateTable;
            }
        }

        public static DatabaseProcessCode CreateUser(string username, string password, bool adminPriveleges) {
            bool uniqueUsername = false;
            IsUserUnique(username, out uniqueUsername);
            if (uniqueUsername) return DatabaseProcessCode.UserNotUnique;
            SHA256 sha = SHA256.Create();
            Random random = new Random(username.GetHashCode());
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string salt = new(Enumerable.Repeat(chars, saltLength).Select(s => s[random.Next(s.Length)]).ToArray());
            byte[] passwordHash = sha.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            Database.Connect();
            string query = @"INSERT INTO users (username, password, admin) VALUES (@username, @password, @admin);";
            using MySqlCommand command = new MySqlCommand(query, Database.Connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@admin", adminPriveleges);
            command.ExecuteNonQuery();
            return DatabaseProcessCode.Success;
        }

        //TEST ME! Unsure the stability of this function, test to ensure this functions as expected
        public static DatabaseProcessCode IsUserUnique(string username, out bool isUnique) {
            database.Connect();
            string query = @"SELECT username FROM users WHERE username = @username;";
            MySqlCommand command = new MySqlCommand(query, connection: database.Connection);
            command.Parameters.AddWithValue("@username", username);
            using var reader = command.ExecuteReader();
            isUnique = username != reader.GetString("username");
            return DatabaseProcessCode.Success;
        }

        public static bool ExistUser(string username) {
            bool unique = false;
            IsUserUnique(username, out unique);
            return !unique;
        }

        private static byte[] GetUserPassword(string username) {
            database.Connect();
            string query = @"SELECT password FROM users WHERE username = @username";
            MySqlCommand command = new MySqlCommand(query, connection: database.Connection);
            command.Parameters.AddWithValue("@username", username);
            using var reader = command.ExecuteReader();
            return (byte[])reader["password"];
        }

        public static DatabaseProcessCode Login(string username, string password) {
            if (!ExistUser(username)) return DatabaseProcessCode.UserDoesNotExist;
            SHA256 sha = SHA256.Create();
            Random random = new Random(username.GetHashCode());
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string salt = new(Enumerable.Repeat(chars, saltLength).Select(s => s[random.Next(s.Length)]).ToArray());
            byte[] passwordHash = sha.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            if (GetUserPassword(username) == passwordHash) return DatabaseProcessCode.Success;
            else return DatabaseProcessCode.InvalidPassword;
        }

        public static DatabaseProcessCode GetUser(string username, out User? user) {
            database.Connect();
            string query = @"SELECT id, username, password, admin FROM users WHERE username = @username";
            MySqlCommand command = new MySqlCommand(query, connection: database.Connection);
            command.Parameters.AddWithValue("@username", username);
            using var reader = command.ExecuteReader();
            user = null;
            if (!reader.Read()) return DatabaseProcessCode.FailedToReadDatabase;
            user = new User { 
                Id = reader.GetInt32("id"),
                Name = reader.GetString("username"),
                Admin = reader.GetBoolean("admin")
            };
            return DatabaseProcessCode.Success;
        }

        public static DatabaseProcessCode GetAllPosts(out ICollection<Post> posts) {
            Database.Connect();
            posts = [];
            string query = "SELECT * FROM posts;";
            MySqlCommand command = new MySqlCommand(query, database.Connection);
            using var reader = command.ExecuteReader();
            while (reader.Read()) {
                posts.Add(new Post { Id = reader.GetInt32("id"), Title = reader.GetString("title"), Description = reader.GetString("description"), Author = reader.GetString("author"), Content = reader.GetString("content"), PostDate = reader.GetString("post_date") });
            }
            return DatabaseProcessCode.Success;
        }

        public static bool AddPost(Post post) {
            
            return false;
        }
    }
}
