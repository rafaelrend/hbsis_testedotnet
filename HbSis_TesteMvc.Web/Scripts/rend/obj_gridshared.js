var gridshared = {
                  
                on_after : null, //Um evento que pode ser chamado após a paginação e ordenação.

                pagina: function (pag, div_id, identificador, url_atualizagrid, form_name, ev){

                    document.getElementById(identificador + "_grid_page").value = pag;
                    this.call_form(identificador, "pagging", form_name, ev);



                },
                mouse_show(e){

                    var tmp_id = "mouse_loading";

                    var s = document.getElementById(tmp_id);
                    var insert = false;

                    if (s == null) {

                        s = document.createElement('div');
                        insert = true;
                    }
                        s.style.position = 'absolute';
                        s.style.margin = '0';
                        s.style.padding = '5px';

                        s.id = tmp_id;

                        s.innerHTML = "<img src='Content/img/FhHRx.gif'>";

                        var e = e || window.event;
                       // s.style.left = (e.clientX - 5) + 'px';
                       // s.style.top = (e.clientY - 5) + 'px';

                        s.style.left = (e.clientX + $(document).scrollLeft()) + 'px';
                        s.style.top = (e.clientY + $(document).scrollTop()) + 'px';

                        if (insert) {

                            document.body.appendChild(s);
                        } else {

                            s.style.display = "";
                        }



                },
                mouse_hide: function () {

                    if (document.getElementById("mouse_loading") != null) {

                        document.getElementById("mouse_loading").style.display = "none";
                    }

                },

                ordena: function (identificador, coluna, tipo, form_name, ev) {
                    var id_grid = document.getElementById(identificador + "_grid_order");

                    id_grid.value = coluna + "|" + tipo;


                    var obj_acao = document.getElementById(identificador + "_acao");
                    obj_acao.value = "salva_ordem";

                    var obj_ordem = document.getElementById(identificador + "_values");
                    obj_ordem.value = coluna + "|" + tipo;


                    this.call_form(identificador, "order", form_name, ev);
                },
                call_form: function (identificador, funcao_str, form_name, e) {
                 //   console.log("oi");
                   // console.log(identificador);
                    console.log(identificador + "_grid_url");
                    var url = document.getElementById(identificador + "_grid_url").value;
                   
                    var pag = document.getElementById(identificador + "_grid_page").value;
                   
                    var ordem = document.getElementById(identificador + "_grid_order").value;
                 //   console.log(ordem);
                    var div_id = document.getElementById(identificador + "_grid_div_retorno").value;
                   

                    var frm = document.forms[0];
                   // console.log(document.forms[0]);
                    if (form_name != null && form_name != undefined && form_name != "") {
                        frm = document.getElementById(form_name);
                    }



                    var data = this.load_data_form(frm);


                    var e = e || window.event;
                    if (e != null) {

                        this.mouse_show(e);
                    }

                    data.append("page", pag);
                    data.append("identificador", identificador);
                    data.append("orberby", ordem);
                    console.log(div_id);
                    $.ajax({
                        type: "POST",
                        url: url,
                        contentType: false,
                        processData: false,
                        data: data,
                        success: function (retorno) {
                            
                            if (funcao_str == "salva_colunas") {


                                $('#' + identificador + "_modal_close").click(); //Fechando o modal primeiro..

                                $("body").removeClass("modal-open");
                            }
                          //  console.log(retorno);
                            console.log(div_id);
                            document.getElementById(div_id).innerHTML = retorno;

                           
                          
                            if (this.on_after != null) {
                                this.on_after(funcao_str);
                            }

                            gridshared.mouse_hide();
                        },
                        error: function (xhr, status, p3, p4) {
                            var err = "Error " + " " + status + " " + p3 + " " + p4;
                            if (xhr.responseText && xhr.responseText[0] == "{")
                                err = JSON.parse(xhr.responseText).Message;


                            console.log(err);
                            gridshared.mouse_hide();
                           // document.getElementById(div_id).innerHTML = err;

                        }
                    }).fail(function (response) {


                        //document.getElementById("span_title_file").innerHTML = old_text;
                        //var form = document.getElementById("FormCadastro");
                        frm.action = url + "?page=" + pag + "&identificador=" + identificador+"&orberby="+ordem;
                        frm.target = "_blank";
                          frm.submit();
                          console.log(response);
                          gridshared.mouse_hide();


                    });


                },
                load_data_form: function (form) {

                    var data = new FormData();
                   
                    for (var y = 0; y < form.elements.length; y++) {
                        if (form.elements[y].type != "file") {

                            data.append(form.elements[y].name, form.elements[y].value);
                        }
                    }

                    return data;
                },

                salvar_colunas: function (identificador, form_name) {



                    var obj_acao = document.getElementById(identificador + "_acao");
                    obj_acao.value = "salva_colunas";

                    var obj_qtde = document.getElementById(identificador + "_qtdcolunas_total");

                    var qtde_total_colunas = parseInt(obj_qtde.value);
                    var qtde_colunas = 0;
                    var str = "";

                    for (var i = 0; i < qtde_total_colunas; i++) {


                        var nome_check = identificador + "_chk_colvisible_" + i.toString();

                        var p_check = document.getElementById(nome_check);
                   //     console.log(nome_check);
                        if (p_check != null) {


                            if (p_check.checked) {
                                qtde_colunas++;

                                if (str != "")
                                    str += ",";

                                str += p_check.value;
                            }
                        }
                    }

                    if (qtde_colunas <= 0) {
                        obj_alert.show("Atenção", "Selecione ao menos uma coluna para ser exibida na tabela.", "warning");
                        //alert("Selecione ao menos uma coluna para ser exibida na tabela.");
                        return false;
                    }

                    var obj_values = document.getElementById(identificador + "_values");
                    obj_values.value = str;

                    this.call_form(identificador, "salva_colunas", form_name);
                },

               //Função para exportar o excel..
                exportarExcel: function (id_form){

                    var frm_consulta = document.getElementById(id_form);
                    var old_action = frm_consulta.action;
                    var old_target = frm_consulta.target;


                    var linkexportaexcel = document.getElementById("linkexportaexcel");
                    frm_consulta.target = "_blank";
                    frm_consulta.action = linkexportaexcel.href;
                    frm_consulta.submit();

                    frm_consulta.target = old_target;
                    frm_consulta.action = old_action;

                },



};   


