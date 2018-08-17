using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbSis_TesteMvc.Infra.Repository
{
    public interface IRepository<T>
    {
        void Remove(int id);

        T Find(int id);

        void Save();

        bool Add(T entidade);

        List<T> ListData
        {
            get;
        }
    }
}
