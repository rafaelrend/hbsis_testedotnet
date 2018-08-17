
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using HbSis_TesteMvc.Domain;

namespace HbSis_TesteMvc.Infra.Repository
{
    public class BookRepository : IRepository<Book>
    {
        private bool disposed = false;
        private DataContext context;
		
		
	
		
		
        public BookRepository(DataContext context)
        {
			
            this.context = context;
        }

        public void Remove(int id)
        {
            Book entidade = context.Books.Find(id);
            context.Books.Remove(entidade);
        }

        public Book Find(int id)
        {
            return context.Books.Find(id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public bool Add(Book entidade)
        {
            if (entidade.Id <= 0)
            {
                context.Books.Add(entidade);
                return true;
            }
            else
            {
                return false;
            }
        }


        public List<Book> ListData
        {
            get { return context.Books.ToList();}
        }



        public List<Book> Listar(string searchString, string state,  string Order = "")
        {
            var consulta = context.Books.Where(s => s.Id > 0);


            if (!String.IsNullOrEmpty(searchString))
            {
                consulta = consulta.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()) || s.Author.ToUpper().Contains(searchString.ToUpper()));

                // consulta.Where(s => s.ReuniaoNome.Nome.ToUpper().Contains(searchString.ToUpper()));
            }


            if (!String.IsNullOrEmpty(state))
            {
                try
                {
                    int stat = Convert.ToInt32(state);

                    if ( stat == 1)
                    {

                        consulta = consulta.Where(s => s.State == BookState.Novo);
                    }
                    if (stat == 2)
                    {

                        consulta = consulta.Where(s => s.State == BookState.Usado);
                    }

                }
                catch { }
            }


       
            if (Order != String.Empty)
            {
                consulta = consulta.OrderBy(Order);
            }

            return consulta.ToList();

        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

    }
}
