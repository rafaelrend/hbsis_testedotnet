using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using HbSis_TesteMvc.Infra.Repository;
using HbSis_TesteMvc.Domain;

namespace HbSis_TesteMvc.Infra
{
    public class UnitOfWork : IDisposable
    {

        public DataContext Context = null;
        public UnitOfWork(DataContext Context)
        {

            this.Context = Context; 
        }
        private BookRepository _BookRepository { get; set; }

        public BookRepository BookRepository
        {
            get { return _BookRepository ?? (_BookRepository = new BookRepository(Context)); }
        }


        private GridConfigurationRepository _GridConfigurationRepository { get; set; }

        public GridConfigurationRepository GridConfigurationRepository
        {
            get { return _GridConfigurationRepository ?? (_GridConfigurationRepository = new GridConfigurationRepository(Context)); }
        }


        public void Save()
        {

            try
            {



                var entidadesdeletadas = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);

                var modifiedEntities = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
     

                Context.SaveChanges();

            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex2)
            {
                throw new Exception(ex2.Message);
            }


        }


        public void Update(object entity)
        {


            //var now = DateTime.UtcNow;

            try
            {



                Context.Entry(entity).State = EntityState.Modified;

                Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this._disposed = true;
        }

    }
}
