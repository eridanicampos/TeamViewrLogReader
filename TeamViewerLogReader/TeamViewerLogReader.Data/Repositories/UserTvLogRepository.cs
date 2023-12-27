using System.Data.SqlClient;
using TeamViewerLogReader.Data.Context;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Domain.Repositories;

namespace TeamViewerLogReader.Data.Repositories
{
    public class UserTvLogRepository : IUserTvLogRepository
    {
        private readonly DataContext _context;

        public UserTvLogRepository(DataContext context)
        {
            _context = context;
        }

        public UserTvLog Create(UserTvLog user)
        {
            string query = @"
                            INSERT INTO UserTvLog 
                            (Id, Email, PasswordHash, PhoneNumber, Username, DateCreated, 
                                IsDeleted, Company, Position, Name, Surname)
                            VALUES 
                            (@Id, @Email, @PasswordHash, @PhoneNumber, @Username, @DateCreated, 
                                @IsDeleted, @Company, @Position, @Name, @Surname);";

            using (var command = new SqlCommand(query, _context.Connection))
            {
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@PhoneNumber", (object)user.PhoneNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@DateCreated", user.DateCreated);
                command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);
                command.Parameters.AddWithValue("@Company", (object)user.Company ?? DBNull.Value);
                command.Parameters.AddWithValue("@Position", (object)user.Position ?? DBNull.Value);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Surname", user.Surname);

                command.ExecuteNonQuery();
            }

            return user;
        }


        public UserTvLog Login(UserTvLog userTvLog)
        {
            UserTvLog user = null;

            string query = @"
                            SELECT 
                                Id, Email, PasswordHash, PhoneNumber, Username, 
                                DateCreated, IsDeleted, Company, Position, Name, Surname
                            FROM 
                                UserTvLog 
                            WHERE 
                                Username = @Username AND PasswordHash = @PasswordHash AND IsDeleted = 0;";

            using (var command = new SqlCommand(query, _context.Connection))
            {
                command.Parameters.AddWithValue("@Username", userTvLog.Username);
                command.Parameters.AddWithValue("@PasswordHash", userTvLog.PasswordHash);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserTvLog
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                            PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                            IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                            Company = reader.IsDBNull(reader.GetOrdinal("Company")) ? null : reader.GetString(reader.GetOrdinal("Company")),
                            Position = reader.IsDBNull(reader.GetOrdinal("Position")) ? null : reader.GetString(reader.GetOrdinal("Position")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Surname = reader.GetString(reader.GetOrdinal("Surname"))
                        };
                    }
                }
            }

            return user;
        }

        public UserTvLog Update(UserTvLog user)
        {
            string query = @"
                                UPDATE UserTvLog 
                                SET 
                                    Email = @Email, 
                                    PasswordHash = @PasswordHash, 
                                    PhoneNumber = @PhoneNumber, 
                                    Username = @Username, 
                                    DateCreated = @DateCreated, 
                                    IsDeleted = @IsDeleted, 
                                    Company = @Company, 
                                    Position = @Position,
                                    Name = @Name,
                                    Surname = @Surname
                                WHERE Id = @Id;";

            using (var command = new SqlCommand(query, _context.Connection))
            {
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@PhoneNumber", (object)user.PhoneNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@DateCreated", user.DateCreated);
                command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);
                command.Parameters.AddWithValue("@Company", (object)user.Company ?? DBNull.Value);
                command.Parameters.AddWithValue("@Position", (object)user.Position ?? DBNull.Value);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Surname", user.Surname);

                command.ExecuteNonQuery();
            }

            return user;
        }


        public string GetUserHashedPassword(string username)
        {
            string query = "SELECT PasswordHash FROM UserTvLog WHERE Username = @Username;";

            using (var command = new SqlCommand(query, _context.Connection))
            {
                command.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetString(reader.GetOrdinal("PasswordHash"));
                    }
                }
            }

            return null;
        }

    }
}
