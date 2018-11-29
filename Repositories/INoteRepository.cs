using final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace final.Repositories
{
    public interface INoteRepository
    {
        List<Note> GetList();
        void Insert(Note item);
        void Update(Note item);
        void Delete(int id);
        Note GetItem(int id);
    }
}
