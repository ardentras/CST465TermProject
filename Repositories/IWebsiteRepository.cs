using final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.Repositories
{
    public interface IWebsiteRepository
    {
        List<Website> GetList();
        void Insert(Website item);
        void Update(Website item);
        void Delete(int id);
        Website GetItem(int id);
    }
}
