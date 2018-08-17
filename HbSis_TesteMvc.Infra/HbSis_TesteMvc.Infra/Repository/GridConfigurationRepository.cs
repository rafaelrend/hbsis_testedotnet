using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HbSis_TesteMvc.Domain;

namespace HbSis_TesteMvc.Infra.Repository
{
    public class GridConfigurationRepository : IRepository<GridConfiguration>
    {
        private bool disposed = false;
        private DataContext context;


        public GridConfigurationRepository(DataContext context)
        {

            this.context = context;
        }


        public List<GridConfiguration> ListData
        {
            get { return context.GridConfigurations.ToList(); }
        }

        public bool Add(GridConfiguration entidade)
        {
            if (entidade.Id <= 0)
            {
                context.GridConfigurations.Add(entidade);
                return true;
            }
            else
            {
                return false;
            }
        }


        public GridConfiguration FindGrids(string identificacao, string funcionalidade, string UsuarioCadastroId)
        {
            var query = from p in this.context.GridConfigurations
                        where (p.Identificacao == identificacao
                              && p.Funcionalidade == funcionalidade
                              && p.UsuarioCadastroId == UsuarioCadastroId)
                        select new { p };

            var lst = query.ToList();

            if (lst.Count > 0)
            {
                return lst[0].p;
            }

            GridConfiguration obj = new GridConfiguration();
            obj.UsuarioCadastroId = UsuarioCadastroId;
            obj.Identificacao = identificacao;
            obj.Funcionalidade = funcionalidade;

            return obj;
        }

        public GridConfiguration Find(int id)
        {
            return context.GridConfigurations.Find(id);
        }

        public void Remove(int id)
        {
            GridConfiguration entidade = context.GridConfigurations.Find(id);
            context.GridConfigurations.Remove(entidade);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
