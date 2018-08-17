using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HbSis_TesteMvc.Domain
{
    public enum BookState
    {
        [Display(Name = "Novo")]
        Novo = 1,
        [Display(Name = "Usado")]
        Usado = 2
    }
}
