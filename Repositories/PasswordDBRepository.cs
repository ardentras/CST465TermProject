using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using final.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

namespace final.Repositories
{
    public class PasswordDBRepository : IPasswordRepository
    {
        private readonly string _CachePrefix = "PasswordCacheRepo";
        private string _CacheListKey { get { return $"{_CachePrefix}_List"; } }
        private IMemoryCache _Cache;
        private DatabaseSettings dbsettings;
        public PasswordDBRepository(IOptions<DatabaseSettings> databaseConfig, IMemoryCache cache)
        {
            _Cache = cache;
            dbsettings = databaseConfig.Value;
        }

        public string GetConnectionString()
        {
            return dbsettings.ConnectionStrings["externalDB"];
        }

        public List<Password> GetList()
        {
            List<Password> passwords = (List<Password>)_Cache.Get(_CacheListKey);

            if (passwords == null)
            {
                passwords = new List<Password>();
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Password_GetList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Password password = new Password();
                                password.ID = (int)reader["ID"];
                                password.Name = reader["Name"].ToString();
                                password.Username = reader["Username"].ToString();
                                password.PasswordValue = reader["PasswordValue"].ToString();
                                password.UserID = (int)reader["UserID"];
                                passwords.Add(password);
                            }
                        }
                    }
                }

                _Cache.Set(_CacheListKey, passwords);
            }

            return passwords;
        }

        public void Insert(Password item)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Password_Insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Username", item.Username);
                    cmd.Parameters.AddWithValue("@PasswordValue", item.PasswordValue);
                    cmd.Parameters.AddWithValue("@UserID", item.UserID);
                    cmd.ExecuteNonQuery();
                }
            }
            _Cache.Remove(_CacheListKey);
        }

        public void Update(Password item)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Password_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    cmd.Parameters.AddWithValue("@ID", item.ID);
                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Username", item.Username);
                    cmd.Parameters.AddWithValue("@PasswordValue", item.PasswordValue);
                    cmd.ExecuteNonQuery();
                }
            }
            _Cache.Remove(_CacheListKey);
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Password_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            _Cache.Remove(_CacheListKey);
        }

        public Password GetItem(int id)
        {
            List<Password> items = new List<Password>();

            items = GetList();

            return items.Where(m => m.ID == id).FirstOrDefault();
        }
    }
}
