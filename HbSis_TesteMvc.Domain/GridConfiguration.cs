using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HbSis_TesteMvc.Domain
{
    public class GridConfiguration
    {


        public int Id { get; set; }


        public DateTime? DataCadastro { get; set; }


        public string Identificacao { get; set; }


        public string Funcionalidade { get; set; }
        public string ColunasVisivel { get; set; }
        public string ColunaOrdenacao { get; set; }

        [Index("ix_UsuarioCadastroId", IsClustered = false, IsUnique = false, Order = -1)]
        public string UsuarioCadastroId { get; set; }

        public string UsuarioCadastroNome { get; set; }
    }
}
