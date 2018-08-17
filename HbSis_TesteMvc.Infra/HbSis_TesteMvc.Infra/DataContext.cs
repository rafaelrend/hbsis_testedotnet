using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Core.Objects;
using HbSis_TesteMvc.Domain;
using System.Data.Entity;

namespace HbSis_TesteMvc.Infra
{
    public class DataContext : DbContext, IObjectContextAdapter
    {

        public ObjectContext ObjectContext;

        public DataContext() : base("DataContext")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 14000;
        }


        public DbSet<Book> Books { get; set; }
        public DbSet<GridConfiguration> GridConfigurations { get; set; }
    }
}
