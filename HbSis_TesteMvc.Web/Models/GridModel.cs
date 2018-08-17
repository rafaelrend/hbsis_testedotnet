using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using HbSis_TesteMvc.Domain;
using HbSis_TesteMvc.Infra;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace HbSis_TesteMvc.Web.Models
{
 


        /// <summary>
        /// Grid Compartilhado que pode ser usado em qualquer funcionalidade.
        /// Autor: Rafael Rend
        /// </summary>
        public class GridModel
        {
            public Type TypeCurrent;
            private string identificador;
            private string funcionalidade;


            public string tableId = "table_id";
            public string tableClasseName = "table table-condensed table-bordered table-striped";


            /// <summary>
            /// Parâmetros extras, pode ser usado de acordo com a necessidade
            /// </summary>
            public string Parametros = "";

            /// <summary>
            /// Permite que seja identificada a tela em que o usuário se encontra.
            /// </summary>
            public string Identificador
            {
                get
                {
                    return identificador;
                }

                set
                {
                    identificador = value;
                }
            }

            public string Funcionalidade
            {
                get
                {
                    return funcionalidade;
                }

                set
                {
                    funcionalidade = value;
                }
            }


            /// <summary>
            /// Div container ID.
            /// </summary>
            public string DivContainerID = "";

            /// <summary>
            /// Url de atualização do GRID
            /// </summary>
            public string UrlAtualizaGrid = "";

            List<IGridColumn> listColumns;
            public List<IGridColumn> ListColumns
            {
                get
                {
                    return listColumns;
                }

                set
                {
                    listColumns = value;
                }
            }

            List<IGridButton> listButtons;
            public List<IGridButton> ListButtons
            {
                get
                {
                    return listButtons;
                }

                set
                {
                    listButtons = value;
                }
            }

            public void setaVisibleColumn(string NomeColuna, bool visible)
            {
                var lst = this.listColumns;

                for (int i = 0; i < lst.Count; i++)
                {
                    var item = lst[i];

                    if (item.Name == NomeColuna)
                    {
                        item.Visible = visible;
                        break;
                    }
                }
            }

            public void setaVisibleButton(string NomeButton, bool visible)
            {
                var lst = this.listButtons;

                for (int i = 0; i < lst.Count; i++)
                {
                    var item = lst[i];

                    if (item.Name == NomeButton)
                    {
                        item.Visible = visible;
                        break;
                    }
                }
            }


            private object listData;
            public int pageSize = 10;
            public int pageNumber = 1;
            public int pageCount = 0;

            public int totalRows = 0;

            /// <summary>
            /// Nome do formulário que o grid deve obter os valores do filtro. Caso não esteja setado, o default será document.forms[0]
            /// </summary>
            public string form_name = "";
            public string ColumnOrder = "";

            public void setListData<T>(List<T> list, int PageNumber, int PageSize)
            {
                this.listData = list;
                this.pageNumber = PageNumber;
                this.pageSize = PageSize;
            }

            public List<T> getListData<T>()
            {
                return (List<T>)listData;
            }

            public IPagedList<T> getListPagedData<T>()
            {
                return ((List<T>)listData).ToPagedList(this.pageNumber, this.pageSize);
            }

            public List<IGridColumn> getVisibleColumns()
            {
                ///Situação para o caso do usuário não ter isso salvo no perfil dele..
                return this.listColumns.Where(x => x.Visible == true && x.ColunaFixa == false).OrderBy(x => x.Order).ToList();

            }

            public List<IGridColumn> getlistaColunaFixa()
            {
                ///Situação para o caso do usuário não ter isso salvo no perfil dele..
                return this.listColumns.Where(x => x.ColunaFixa == true).OrderBy(x => x.Order).ToList();

            }

            public List<IGridColumn> getAllColumns()
            {
                ///Situação para o caso do usuário não ter isso salvo no perfil dele..
                return this.listColumns;

            }
            /// <summary>
            /// Colunas fixas não será mostrada para o usuário ativar ou desativar a exibição.
            /// </summary>
            /// <returns></returns>
            public List<IGridColumn> getAllColumnsAnyLessFixa()
            {
                ///Situação para o caso do usuário não ter isso salvo no perfil dele..
                return this.listColumns.Where(w => !w.ColunaFixa).ToList();

            }
            //public List<IGridColumn> getSelectedColuns()
            //{

            //}

            public string getValueItem(object item, string property)
            {
                if (property.IndexOf(".") > -1)
                {
                    string[] ar = property.Split('.');
                    property = ar[0];
                }

                Type type = item.GetType();

                //

                PropertyInfo info = type.GetProperty(property);
                // System.Dynamic.InvokeGet(item, property);
                object val = "";

                if (info != null)
                {
                    val = info.GetValue(item, null).ToString();



                }
                else
                {
                        try
                        {

                            //Dapper..
                            var data_row = (IDictionary<string, object>)item;

                            data_row.TryGetValue(property, out val);
                        }
                        catch { }

                }

                if (val != null && val.ToString() != String.Empty)
                {


                    return val.ToString();
                }

                return string.Empty;
            }
            public string getValueItem(object item, IGridColumn coluna)
            {
                string property = coluna.Name;

                if (property.IndexOf(".") > -1)
                {
                    string[] ar = property.Split('.');
                    property = ar[0];
                }

                Type type = item.GetType();

                //

                PropertyInfo info = type.GetProperty(property);
                // System.Dynamic.InvokeGet(item, property);
                object val = "";

                if (info != null)
                {
                    val = info.GetValue(item, null).ToString();



                }
                else
                {
                        try
                        {
                            //Dapper..
                            var data_row = (IDictionary<string, object>)item;

                            data_row.TryGetValue(property, out val);
                        }
                        catch { }

                }

                if (val != null && val.ToString() != String.Empty)
                {
                    if (coluna.TypeColumn == typeof(DateTime) && coluna.Format != String.Empty)
                    {
                        return Convert.ToDateTime(val).ToString(coluna.Format);

                    }
                    else if (coluna.TypeColumn.BaseType.Name.Equals("Enum"))
                    {
                        //  return EnumHelper.GetValueFromName<coluna.TypeColumn>(Enum.Parse(coluna.TypeColumn, val.ToString()));
                        //var a = EnumHelper.GetValueFromName(coluna.TypeColumn, val.ToString());
                        try
                        {
                            Type tipo = GetEnumType(coluna.TypeColumn.FullName);
                            var valor = Enum.Parse(coluna.TypeColumn, val.ToString());
                            var member = tipo.GetMember(valor.ToString())[0];

                            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);

                            var attribute = (DisplayAttribute)attributes[0];
                            return attribute.GetName();
                        }
                        catch (Exception ex)
                        {

                            return val.ToString();
                        }

                        //    var enumerador = Enum.Parse(coluna.TypeColumn, val.ToString()).Get(val.ToString());
                        //     return (Enum)enumerador.GetDisplayName();
                        // return val.ToString();
                    }
                    else
                    {
                        return val.ToString();
                    }


                }

                return string.Empty;
            }

            public string getValueItemImagem(object item, IGridColumn coluna)
            {
                string property = coluna.Name;
                string propertydata = "Data";
                if (property.IndexOf(".") > -1)
                {
                    string[] ar = property.Split('.');
                    property = ar[0];
                }

                Type type = item.GetType();

                //

                PropertyInfo info = type.GetProperty(propertydata);
                // System.Dynamic.InvokeGet(item, property);
                object val = "";

                if (info != null)
                {
                    val = info.GetValue(item, null).ToString();



                }
                else
                {
                    //Dapper..
                    var data_row = (IDictionary<string, object>)item;

                    data_row.TryGetValue(propertydata, out val);

                }

                if (val != null && val.ToString() != String.Empty)
                {
                    if (coluna.TypeColumn == typeof(DateTime) && coluna.Format != String.Empty)
                    {
                        return Convert.ToDateTime(val).ToString(coluna.Format);

                    }
                    else if (coluna.TypeColumn.BaseType.Name.Equals("Enum"))
                    {
                        //  return EnumHelper.GetValueFromName<coluna.TypeColumn>(Enum.Parse(coluna.TypeColumn, val.ToString()));
                        //var a = EnumHelper.GetValueFromName(coluna.TypeColumn, val.ToString());
                        try
                        {
                            Type tipo = GetEnumType(coluna.TypeColumn.FullName);
                            var valor = Enum.Parse(coluna.TypeColumn, val.ToString());
                            var member = tipo.GetMember(valor.ToString())[0];

                            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);

                            var attribute = (DisplayAttribute)attributes[0];
                            return attribute.GetName();
                        }
                        catch (Exception ex)
                        {

                            return val.ToString();
                        }

                        //    var enumerador = Enum.Parse(coluna.TypeColumn, val.ToString()).Get(val.ToString());
                        //     return (Enum)enumerador.GetDisplayName();
                        // return val.ToString();
                    }
                    else
                    {
                        return val.ToString();
                    }


                }

                return string.Empty;
            }
            public static Type GetEnumType(string name)
            {
                return
                 (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                  let type = assembly.GetType(name)
                  where type != null
                     && type.IsEnum
                  select type).FirstOrDefault();
            }
            public void updateColumnsVisible(string values)
            {
                System.Collections.ArrayList arr = new System.Collections.ArrayList(values.Split(','));

                for (int i = 0; i < this.listColumns.Count; i++)
                {
                    //var item = this.listColumns[i];

                    //item.Visible = arr.Contains(this.listColumns[i].Name);

                    //this.listColumns[i] = item;



                    this.listColumns[i].Visible = arr.Contains(this.listColumns[i].Name);

                }
            }

            public void updateButtonsVisible(string values)
            {
                System.Collections.ArrayList arr = new System.Collections.ArrayList(values.Split(','));

                for (int i = 0; i < this.listButtons.Count; i++)
                {
                    this.listButtons[i].Visible = arr.Contains(this.listButtons[i].Name);
                }
            }

            public void salvarConfiguracao(string identificacao,
                 HttpRequestBase Request, UnitOfWork unitOfWork)
            {
                if (Request[identificacao + "_ispostback"] == "1")
                {
                    string acao = Request[identificacao + "_acao"];
                    string values = Request[identificacao + "_values"];

                    if (acao == "salva_colunas" && values != String.Empty)
                    {
                        var obj = unitOfWork.GridConfigurationRepository.FindGrids(this.Identificador, this.Funcionalidade,
                              HttpContext.Current.Session.SessionID );

                        if (obj.DataCadastro == null)
                        {
                            obj.DataCadastro = DateTime.Now;
                        }

                        obj.ColunasVisivel = values;
                        

                        if (unitOfWork.GridConfigurationRepository.Add(obj))
                        {
                            unitOfWork.Save();
                        }
                        else
                        {
                            unitOfWork.Update(obj);
                        }

                        updateColumnsVisible(values);
                    }


                    if (acao == "salva_ordem" && values != String.Empty)
                    {
                        var obj = unitOfWork.GridConfigurationRepository.FindGrids(this.Identificador, this.Funcionalidade,
                              HttpContext.Current.Session.SessionID);

                        if (obj.DataCadastro == null)
                        {
                            obj.DataCadastro = DateTime.Now;
                        }

                        obj.ColunaOrdenacao = values;
                  

                        if (unitOfWork.GridConfigurationRepository.Add(obj))
                        {
                            unitOfWork.Save();
                        }
                        else
                        {
                            unitOfWork.Update(obj);
                        }

                        this.ColumnOrder = values;
                    }

                }

            }

            public void loadPostBackValues(HttpRequestBase Request,
                UnitOfWork unitOfWork)
            {
                if (Request[this.Identificador + "_ispostback"] == "1")
                {
                    if (Request[this.Identificador + "_grid_colunas_name"] != "")
                    {
                        updateColumnsVisible(Request[this.Identificador + "_grid_colunas_name"]);
                    }
                    if (Request[this.Identificador + "_grid_botoes_visivel"] != "")
                    {
                        updateButtonsVisible(Request[this.Identificador + "_grid_botoes_visivel"]);
                    }
                    if (Request[this.Identificador + "_grid_parametros"] != "")
                    {
                        this.Parametros = Request[this.Identificador + "_grid_parametros"];
                    }



                    if (Request[this.Identificador + "_grid_page"] != "")
                    {
                        try
                        {

                            this.pageNumber = Convert.ToInt32(Request[this.Identificador + "_grid_page"]);
                        }
                        catch { }
                    }
                    if (Request[this.Identificador + "_grid_order"] != "")
                    {
                        try
                        {

                            this.ColumnOrder = Request[this.Identificador + "_grid_order"];
                        }
                        catch { }
                    }

                }

                // _grid_page
                // _grid_order


            }

            public void loadDefaultValues(HttpRequestBase Request,
                UnitOfWork unitOfWork
                   )
            {
                if (Request[this.Identificador + "_ispostback"] != "1")
                {
                    var obj = unitOfWork.GridConfigurationRepository.FindGrids(this.Identificador, this.Funcionalidade,
                            HttpContext.Current.Session.SessionID);

                    if (!String.IsNullOrEmpty(obj.ColunasVisivel))
                    {
                        this.updateColumnsVisible(obj.ColunasVisivel);
                    }

                    if (!String.IsNullOrEmpty(obj.ColunaOrdenacao))
                    {
                        this.ColumnOrder = obj.ColunaOrdenacao;
                    }

                }
            }


            public string getLINQOrdenationString()
            {
                string coluna_ordenacao = "Id";

                if (!String.IsNullOrEmpty(this.ColumnOrder))
                {
                    string[] frags = this.ColumnOrder.Split('|');
                    coluna_ordenacao = frags[0];

                    if (frags[1] == "desc")
                    {
                        coluna_ordenacao += " descending";
                    }
                }

                return coluna_ordenacao;

                //if (!String.IsNullOrEmpty(Request["orberby"]))
                //{
                //    model.ColumnOrder = Request["orberby"];

                //    string[] frags = Request["orberby"].Split('|');
                //    coluna_ordenacao = frags[0];

                //    if (frags[1] == "desc")
                //    {
                //        coluna_ordenacao += " descending";
                //    }
                //}


            }

            //public static IQueryable Sort(this IQueryable collection, string sortBy, bool reverse = false)
            //{
            //    return collection.OrderBy(sortBy + (reverse ? " descending" : ""));
            //}

        }
        public class IGridColumn
        {
            public string Name;
            public string Title;
            public string Format;
            public string Icone;
            public bool Visible;
            public Type TypeColumn;
            public int Order;
            public string OnClick;
            public bool ColunaFixa;

            public IGridColumn(string name, string title, string format, string icone, bool visible,
                        Type typeColumn, int order)
            {
                this.Name = name;
                this.Title = title;
                this.Format = format;
                this.Icone = icone;
                this.Visible = visible;
                this.TypeColumn = typeColumn;
                this.Order = order;
            }
            public IGridColumn(string name, string title, string format, string icone, bool visible,
                   Type typeColumn, int order, string onClick, bool colunaFixa)
            {
                this.Name = name;
                this.Title = title;
                this.Format = format;
                this.Icone = icone;
                this.Visible = visible;
                this.TypeColumn = typeColumn;
                this.Order = order;
                this.OnClick = onClick;
                this.ColunaFixa = colunaFixa;
            }
        }

        public class IGridButton
        {
            public string Name;
            public string Title;
            public string OnClick;
            public string ClasseName;
            public string Icone;
            public bool Visible;
            public string Complemento;
            /// <summary>
            /// /input com opção de checkbox para ativar ou inativar um registro
            /// </summary>
            public bool ButtonAtivarEInativar { get; set; }
            /// <summary>
            /// Se na hora de exibir este botão, ele precisa de algum indicativo que esta visível ou não. 
            /// Se estiver preenchido, a cada resultado da consulta é necessário conter um parametro indicado, e contendo true or false para que se o mesmo deve 
            /// aparecer ou não.
            /// </summary>
            public string ParametroVisivelLinha;


            public IGridButton(string name, string title, string onclick, string classeName, string icone,
                 bool visible = true, string complemento = "", string parametro_visivel_linha = "")
            {
                this.Name = name;
                this.Title = title;
                this.OnClick = onclick;
                this.ClasseName = classeName;
                this.Icone = icone;
                this.Visible = visible;
                this.Complemento = complemento;
                this.ParametroVisivelLinha = parametro_visivel_linha;
            }


            public IGridButton(string name, string title, string onclick, string classeName, string icone,
                 bool visible = true, bool ButtonAtivarEInativar = false, string complemento = "", string parametro_visivel_linha = "")
            {
                this.Name = name;
                this.Title = title;
                this.OnClick = onclick;
                this.ClasseName = classeName;
                this.Icone = icone;
                this.Visible = visible;
                this.ButtonAtivarEInativar = ButtonAtivarEInativar;
                this.Complemento = complemento;
                this.ParametroVisivelLinha = parametro_visivel_linha;

            }
        }


        /// <summary>
        /// Classe que pode englobar vários GRIDsCompartilhados. Caso um view possua mais de um.
        /// </summary>
        public class ListGridModel
        {
            private List<GridModel> grids;

            public List<GridModel> Grids
            {
                get
                {
                    return grids;
                }

                set
                {
                    grids = value;
                }
            }
        }

    }

