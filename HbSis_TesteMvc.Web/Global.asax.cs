using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HbSis_TesteMvc.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MigrateDatabases(); //Criando a tabela e adicionando registros.
        }

        public static void MigrateDatabases()
        {
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["DataContext"].ToString();

            var config = new HbSis_TesteMvc.Infra.Migrations.Configuration
            {
                TargetDatabase = new DbConnectionInfo(conexao, "System.Data.SqlClient"),
                AutomaticMigrationsEnabled = true,
                AutomaticMigrationDataLossAllowed = true
            };


            var migrator = new DbMigrator(config);
            migrator.Update();

    
        }

    }
 }

