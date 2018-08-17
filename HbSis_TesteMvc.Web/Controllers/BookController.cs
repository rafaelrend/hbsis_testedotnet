using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HbSis_TesteMvc.Infra;
using HbSis_TesteMvc.Domain;
using HbSis_TesteMvc.Web.Servico;
using HbSis_TesteMvc.Web.Models;
using PagedList;
using Rend.Utilities;

namespace HbSis_TesteMvc.Web.Controllers
{
    public class BookController : Controller
    {
        private UnitOfWork unitOfWork = new ServicoUnitOfWork().GetUnitOfWork();
        private int regid = 0;

        private void load_bags()
        {


            ViewBag.bagListState = SimpleIdName.createByString("1||Novo,2||Usado");

        }

        public ActionResult Create()
        {
            load_bags();
            var book = new Book();
            book.DateInserted = DateTime.Now;

            return PartialView("~/Views/Book/FormCadastro.cshtml", book);
        }

        //
        // POST/GET: /Hemocentro/Delete/5
        public ActionResult Delete(int id)
        {
            if ( id > 0)
            {
                try
                {

                    unitOfWork.BookRepository.Remove(id);
                    unitOfWork.Save();

                    return Json(new { success = true });
                }catch(Exception exp){

                    return Json(new { success = false, msg = exp.Message });
                }

            }

            return Json(new { success = false, blank = true, msg = "ID vazia" });
            
        }

        [HttpPost]
        public ActionResult Save(Book entidadeModel)
        {
          
             if (
                   unitOfWork.BookRepository.ListData.Exists(
                       w => w.Title.ToUpper().Equals(entidadeModel.Title.ToUpper()) && w.Id != entidadeModel.Id))
             {
                 ModelState.AddModelError("Título", "Já existe um livro cadastrado com este título!");
             }

            var registro = new Book();
            if (entidadeModel.Id > 0)
            {
                registro = unitOfWork.BookRepository.Find(entidadeModel.Id);

            }


            if (ModelState.IsValid)
            {
                registro.Title = entidadeModel.Title;

                registro.Author = entidadeModel.Author;

                registro.ISBN = entidadeModel.ISBN;

                registro.State = entidadeModel.State;
                registro.QtdeEstoque = entidadeModel.QtdeEstoque;

                //registro.DateInserted = entidadeModel.DateInserted;

                if (registro.DateInserted == DateTime.MinValue)
                    registro.DateInserted = DateTime.Now;

                registro.LastUpdate = DateTime.Now;

                if (unitOfWork.BookRepository.Add(registro))
                {
                    unitOfWork.Save();
                }
                else
                {
                    unitOfWork.Update(registro);
                }
                return Json(new { success = true });
            }

            load_bags();
            return PartialView("~/Views/Book/FormCadastro.cshtml", registro);
        }

        /// <summary>
        /// Edição
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            load_bags();
            return PartialView("~/Views/Book/FormCadastro.cshtml", unitOfWork.BookRepository.Find(id));
        }


        // GET: Book
        public ActionResult Index()
        {
            

            load_bags();

            var modelGrid = getModelGrid("grid0", "book", true);

            int pageSize = 10;

            ViewBag.bagSearchString = Request["searchString"];
            ViewBag.bagState = Request["State"];
         
            ViewBag.bagModelGrid = modelGrid;


            return View();
        }
        public ActionResult Export()
        {

            string tmp = Server.MapPath("~/tmp");
            string path_completo = tmp + "\\" + Session.SessionID.Substring(0, 6) + "_livros.xls";
            string nomeXls = "livros.xls";


            var modelGrid = getModelGrid("grid0", "book", false );
           

            GridModelExport.createXls(modelGrid, "livros",true, path_completo);


            byte[] arquivo = System.IO.File.ReadAllBytes(path_completo);

            try
            {

                //Vou deletar o arquivo original porque não precisamos mais dele..
                System.IO.File.Delete(path_completo);
            }
            catch { }

            return File(arquivo, "application/vnd.ms-excel", nomeXls); //Eis aqui a sua planilha no formato excel..

        }


        public ActionResult Grid()
        {

            var modelGrid = getModelGrid("grid0", "book", true);
            return View("~/Views/Grid/GridShared.cshtml", modelGrid);
        }

            private GridModel getModelGrid(string Identificador, string Funcionalidade, bool paginacao = true )
        {
            GridModel model = new GridModel();


            model.Identificador = Identificador;
            model.Funcionalidade = Funcionalidade;

            string searchString = Request["searchString"];
            string searchState = Request["State"];



            int? IdUnidade = null;

            int pageSize = 10;
            int pageNumber = 1;

            if (!String.IsNullOrEmpty(Request["page"]))
            {
                pageNumber = Convert.ToInt32(Request["page"]);
            }

            //--------------------- VALORES INICIAIS ------------------------
            model.ColumnOrder = "Id|asc";
            string coluna_ordenacao = "Id";

            List<IGridColumn> coluns = new List<IGridColumn>();
            //name, string title, string format, string icone, bool visible, 
            // Type typeColumn, int order
            coluns.Add(new IGridColumn("Id", "Id", "", "", true, typeof(int), 0));
            coluns.Add(new IGridColumn("Title", "Título do Livro", "", "", true, typeof(string), 1));
            coluns.Add(new IGridColumn("Author", "Autor", "", "", true, typeof(string), 2));
            coluns.Add(new IGridColumn("ISBN", "ISBN", "", "", false, typeof(string), 3));
            coluns.Add(new IGridColumn("QtdeEstoque", "Estoque", "", "", false, typeof(int), 4));
            coluns.Add(new IGridColumn("DateInserted", "Data Cadastro", "dd/MM/yyyy", "", true, typeof(DateTime), 5));
            coluns.Add(new IGridColumn("State", "Situação", "", "", true, typeof(string), 6));
          
            model.ListColumns = coluns;
            // ------------------------------------------------------------------------------
            // -- Botões diversos --- 
            List<IGridButton> buttons = new List<IGridButton>();


            buttons.Add(new IGridButton("BtEdit", "", "obj_book.editar({Id})", "btn-edit", "fa-pencil-square-o", true, string.Empty, "botao_editar"));
            buttons.Add(new IGridButton("btDelete", "", "obj_book.deletar({Id})", "btn-delete", "fa-trash-o", true, string.Empty, "botao_excluir"));
            // buttons.Add(new IGridButton("BtExecutar", "Executar", "", "btn-executar", "fa-commenting", true, string.Empty, "botao_editar"));

            //            buttons.Add(new IGridButton("BtConfigurar", "", "", "", "", true, string.Empty));

            model.ListButtons = buttons;

            //------------------------------------------------------------------------------
            model.loadDefaultValues(this.Request, unitOfWork); //Se é a primeira vez, vou checar se existe algo no banco..

            model.loadPostBackValues(this.Request, unitOfWork);

            //Se há postback, checa se deve salvar alguma coisa..
            model.salvarConfiguracao(model.Identificador, this.Request, unitOfWork);

            coluna_ordenacao = model.ColumnOrder;

            var dados = unitOfWork.BookRepository.Listar(searchString, searchState, coluna_ordenacao.Replace("|", " "));

            model.totalRows = dados.Count;

            if (!paginacao)
            {
                pageSize = dados.Count; //mostrando tudo..
            }

            IPagedList<Book> lstData;

            lstData = dados.ToPagedList(pageNumber, pageSize);



            
            model.setListData<object>(lstData.Cast<object>().ToList(), pageNumber, pageSize);

            model.pageCount = lstData.PageCount;

            model.DivContainerID = "div_grid";
            model.UrlAtualizaGrid = Url.Action("Grid", "Book", new { area = "" });

            return model;

        }
    }
}