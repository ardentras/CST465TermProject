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

namespace final.Repositories
{
    public class NoteDBRepository : INoteRepository
    {
        private readonly string _CachePrefix = "NoteCacheRepo";
        private string _CacheListKey { get { return $"{_CachePrefix}_List"; } }
        private IMemoryCache _Cache;
        private DatabaseSettings dbsettings;
        public NoteDBRepository(IOptions<DatabaseSettings> databaseConfig, IMemoryCache cache)
        {
            _Cache = cache;
            dbsettings = databaseConfig.Value;
        }

        public string GetConnectionString()
        {
            return dbsettings.ConnectionStrings["externalDB"];
        }

        public List<Note> GetList()
        {
            List<Note> notes = (List<Note>)_Cache.Get(_CacheListKey);

            if (notes == null)
            {
                notes = new List<Note>();
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("Note_GetList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Note note = new Note();
                                note.ID = (int)reader["ID"];
                                note.Name = reader["Name"].ToString();
                                note.Text = reader["Text"].ToString();
                                note.UserID = (int)reader["UserID"];
                                notes.Add(note);
                            }
                        }
                    }
                }

                _Cache.Set(_CacheListKey, notes);
            }

            return notes;
        }

        public void Insert(Note item)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Note_Insert", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Text", item.Text);
                    cmd.Parameters.AddWithValue("@UserID", item.UserID);
                    cmd.ExecuteNonQuery();
                }
            }
            _Cache.Remove(_CacheListKey);
        }

        public void Update(Note item)
        {
            Console.WriteLine("HIHGIOSHDOIHSIOGDh");
            Console.WriteLine(item.ID);
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Note_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    cmd.Parameters.AddWithValue("@ID", item.ID);
                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Text", item.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            _Cache.Remove(_CacheListKey);
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("Note_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            _Cache.Remove(_CacheListKey);
        }

        public Note GetItem(int id)
        {
            List<Note> items = new List<Note>();

            items = GetList();

            return items.Where(m => m.ID == id).FirstOrDefault();
        }
    }
}
