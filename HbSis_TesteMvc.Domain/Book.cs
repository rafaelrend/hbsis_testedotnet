using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HbSis_TesteMvc.Domain
{
    public class Book
    {
		   
	     public int Id  {get; set;}
	  
	  
	     [Required(ErrorMessage = "O Título é obrigatório!")]
	     public string Title  {get; set;}
	  

	     public string Author  {get; set;}
	  
	  
	     public string ISBN  {get; set;}



        [Required(ErrorMessage = "Informe a situação do livro!")]
        public BookState State  {get; set;}

        [Required(ErrorMessage = "A data de cadastro é obrigatória!")]
        public DateTime DateInserted  {get; set;}
	  
	  
	     public DateTime? LastUpdate  {get; set; }
         public int QtdeEstoque { get; set; }




    }
}
