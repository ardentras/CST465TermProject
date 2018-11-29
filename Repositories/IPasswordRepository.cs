using final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.Repositories
{
    public interface IPasswordRepository
    {
        List<Password> GetList();
        void Insert(Password item);
        void Update(Password item);
        void Delete(int id);
        Password GetItem(int id);
    }
}
