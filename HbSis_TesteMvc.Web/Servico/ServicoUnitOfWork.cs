using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HbSis_TesteMvc.Infra;
using System.Runtime.Caching;

namespace HbSis_TesteMvc.Web.Servico
{
    public class ServicoUnitOfWork : Controller
    {
        public UnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork(Create());
        }


        public DataContext Create()
        {
            return new DataContext();
           
        }

    }
}