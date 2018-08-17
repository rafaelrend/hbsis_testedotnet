using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ExcelLibrary;

namespace HbSis_TesteMvc.Web.Models
{
    /// <summary>
    /// Classe de exportação dos dados de um gridShared8idea
    /// </summary>
    public class GridModelExport
    {
        public static  DataTable getDataTable(GridModel objGrid, string nomeDataTable = "" , bool tituloAmigavel = false )
        {
            DataTable dtSaida = new DataTable();

            for (int i = 0; i < objGrid.ListColumns.Count; i++)
            {
                var column = objGrid.ListColumns[i];

                if ( column.Visible && ! dtSaida.Columns.Contains(column.Name) )
                {
                    dtSaida.Columns.Add(column.Name, typeof(string) );                    
                }
            }

            var list = objGrid.getListData<object>();

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                DataRow dr = dtSaida.NewRow();

                for (int zz = 0; zz < objGrid.ListColumns.Count; zz++)
                {
                    var column = objGrid.ListColumns[zz];
                    if (column.Visible)
                    {
                        var objValue = objGrid.getValueItem(item, column.Name);

                        try
                        {

                            dr[column.Name] = objValue.ToString();
                        }
                        catch { }
                    }
                }

                dtSaida.Rows.Add(dr);
            }

            if (nomeDataTable == "")
            {
                dtSaida.TableName = objGrid.DivContainerID;

            }else
            {

                dtSaida.TableName = nomeDataTable;
            }

            if (tituloAmigavel)
            {
                //Vou por o nome das colunas igual ao título..

                for (int i = 0; i < objGrid.ListColumns.Count; i++)
                {
                    var column = objGrid.ListColumns[i];

                    if (column.Visible && dtSaida.Columns.Contains(column.Name) && column.Title != String.Empty)
                    {
                        try
                        {

                            dtSaida.Columns[column.Name].ColumnName = column.Title; //Alterando o nome da coluna..
                        }
                        catch { }
                    }
                }


            }


            return dtSaida;
        }


        /// <summary>
        /// Gera um dataSet a partir de um list de Grids.
        /// </summary>
        /// <param name="listGrid">List de Grids</param>
        /// <param name="titulos">Títulos de cada planilha do excel</param>
        /// <returns></returns>
        public static DataSet getDataSet(ListGridModel listGrid, List<String> titulos)
        {
            DataSet ds = new DataSet();

            for (int i = 0; i < listGrid.Grids.Count; i++)
            {
                DataTable dt = getDataTable(listGrid.Grids[i], titulos[i], true);

                try
                {
                   // dt.TableName = titulos[i];
                }
                catch { }

                ds.Tables.Add(dt);
            }

            return ds;
        }


        /// <summary>
        /// Cria um arquivo excel a partir de um GridModel
        /// </summary>
        /// <param name="objGrid">Grid</param>
        /// <param name="nome_amigavel">Nome amigavel para o Grid Desejado</param>
        /// <param name="titulo_amigavel">Título amigável para o Grid Desejado</param>
        /// <param name="path">Path onde se encontrará o arquivo</param>
        public static void createXls(GridModel objGrid, string nome_amigavel, bool titulo_amigavel, string path)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(getDataTable(objGrid, nome_amigavel, titulo_amigavel));
            ExcelLibrary.DataSetHelper.CreateWorkbook(path, ds);
        }



        /// <summary>
        /// Cria um arquivo excel a partir de um ListGridModel
        /// </summary>
        /// <param name="objGrid">Grid</param>
        /// <param name="nome_amigavel">Nome amigavel para o Grid Desejado</param>
        /// <param name="titulo_amigavel">Título amigável para o Grid Desejado</param>
        /// <param name="path">Path onde se encontrará o arquivo</param>
        public static void createXls(ListGridModel objList, List<string> titulos, string path)
        {
            DataSet ds = getDataSet(objList, titulos);
            ExcelLibrary.DataSetHelper.CreateWorkbook(path, ds);
        }

        


    }
}