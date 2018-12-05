using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using final.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text;

namespace final.Repositories
{
    public class WebsiteDBRepository : IWebsiteRepository
    {
        private readonly string _CachePrefix = "WebsiteCacheRepo";
        private string _CacheListKey { get { return $"{_CachePrefix}_List"; } }
        private IMemoryCache _Cache;
        private DatabaseSettings dbsettings;
        public WebsiteDBRepository(IOptions<DatabaseSettings> databaseConfig, IMemoryCache cache)
        {
            _Cache = cache;
            dbsettings = databaseConfig.Value;
        }

        public string GetConnectionString()
        {
            return dbsettings.ConnectionStrings["externalDB"];
        }

        public List<Website> GetList()
        {
            List<Website> websites = (List<Website>)_Cache.Get(_CacheListKey);

            if (websites == null)
            {
                websites = new List<Website>();
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Website_GetList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Website website = new Website();
                                website.ID = (int)reader["ID"];
                                website.Name = reader["Name"].ToString();
                                website.URL = reader["URL"].ToString();
                                website.Username = reader["Username"].ToString();
                                website.PasswordValue = reader["PasswordValue"].ToString();
                                website.UserID = (int)reader["UserID"];
                                websites.Add(website);
                            }
                        }
                    }

                    _Cache.Set(_CacheListKey, websites);
                }
            }

            return websites;
        }

        public void Insert(Website item)
        {
            item.PasswordValue = Convert.ToBase64String(Encoding.ASCII.GetBytes(item.PasswordValue));
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Website_Insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Username", item.Username);
                    cmd.Parameters.AddWithValue("@URL", item.URL);
                    cmd.Parameters.AddWithValue("@PasswordValue", item.PasswordValue);
                    cmd.Parameters.AddWithValue("@UserID", item.UserID);
                    cmd.ExecuteNonQuery();
                }
            }
            _Cache.Remove(_CacheListKey);
        }

        public void Update(Website item)
        {
            item.PasswordValue = Convert.ToBase64String(Encoding.ASCII.GetBytes(item.PasswordValue));
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Website_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    cmd.Parameters.AddWithValue("@ID", item.ID);
                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Username", item.Username);
                    cmd.Parameters.AddWithValue("@URL", item.URL);
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
                using (SqlCommand cmd = new SqlCommand("Website_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            _Cache.Remove(_CacheListKey);
        }

        public Website GetItem(int id)
        {
            List<Website> items = new List<Website>();

            items = GetList();

            return items.Where(m => m.ID == id).FirstOrDefault();
        }
    }
}
