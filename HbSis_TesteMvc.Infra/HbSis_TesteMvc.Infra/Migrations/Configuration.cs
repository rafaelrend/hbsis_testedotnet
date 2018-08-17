using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HbSis_TesteMvc.Domain;
using HbSis_TesteMvc.Infra;

namespace HbSis_TesteMvc.Infra.Migrations
{

    public sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(DataContext context)
        {


            if (! context.Books.Any())
            {

                string[] authors_example = new string[] { "Rafael Rend", "Fiódor Dostoiévski", "Augusto Cury", "Paulo Coelho", "Silvio Santos", "Stephen King" };
                Random rnd = new Random();
                //Vou cadastrar alguns livros para termos algo pra observar.
                for (int i = 1; i < 15; i++)
                {
                    var livro = new Book();
                    livro.Title = "Livro " + i.ToString() + " - TESTE ";
                    int random = rnd.Next(0, authors_example.Length);
                    livro.Author = authors_example[random];
                    livro.DateInserted = DateTime.Now;
                    livro.State = BookState.Novo;
                    livro.QtdeEstoque = rnd.Next(1, 50);

                    if (rnd.Next(1, 3) > 1)
                        livro.State = BookState.Usado;

                    livro.ISBN = "AA" + i.ToString().PadLeft(10, '0');
                    context.Books.AddOrUpdate(livro);
                }
              
            }


        }


    }

}
