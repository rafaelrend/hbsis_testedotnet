using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rend.Utilities
{
    /// <summary>
    /// Classe para manipulações de resultados simples, pequenas listas que não estão no banco de dados.
    /// Autor: Rafael Rend
    /// </summary>
    public class SimpleIdName
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public SimpleIdName()
        {
        }
        public SimpleIdName(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public static IList<SimpleIdName> createByString(string str)
        {
            List<SimpleIdName> lst = new List<SimpleIdName>();

            string[] itens = str.Split(',');

            for (int i = 0; i < itens.Length; i++)
            {
                if (itens[i] == String.Empty)
                    continue;

                var cols = itens[i].Split(new string[] { "||" }, StringSplitOptions.None);

                var itemSimple = new SimpleIdName();
                if (cols.Length > 1)
                {
                    itemSimple.Id = cols[0];
                    itemSimple.Name = cols[1];
                }
                else
                {

                    itemSimple.Id = cols[0];
                    itemSimple.Name = cols[0];
                }

                lst.Add(itemSimple);
            }

            return lst;

        }

        public static string getDescription(IList<SimpleIdName> lista, string str)
        {

            string[] itens = str.Split(',');
            string saida = "";

            for (int i = 0; i < itens.Length; i++)
            {
                if (itens[i] == String.Empty)
                    continue;

                try
                {

                    SimpleIdName it = lista.Where(s => s.Id.ToString() == itens[i]).Single();

                    if (saida != "")
                        saida += ", ";

                    saida += it.Name;
                }
                catch { }


            }

            return saida;
        }

        public override string ToString()
        {
            return this.Id + " - " + this.Name;
        }
    }
}
