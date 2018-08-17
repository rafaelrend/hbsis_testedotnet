//Funcionalidades para o cadastro de livro.
var obj_book = {


    editar: function (id) {
        url = document.getElementById("linkedit").href + "/" + id.toString();

        $.colorbox({ iframe: true, innerWidth: "85%", height: "90%", innerHeight: "99%", href: url, scrolling: false });
        
    },

    deletar: function (id) {

        var url = document.getElementById("linkdelete").href;

        var fn_sim = function (result) {

            if (result.value) {
                $.ajax({
                    type: "POST",
                    url: url,
                    data: { id: id },
                    success: function (data) {

                        if (data.success) {

                            obj_alert.show("Sucesso", "Livro excluído com sucesso!", "success");
                            obj_book.pesquisar(null, window.event);
                        } else {

                            obj_alert.show("Error", "Não foi possível excluir" + data.msg, "error");
                        }
                    },
                    error: function (data) {
                        obj_alert.show("Error","Houve um erro ao tentar excluir: " + data, "error");
                    }
                });
            }

        }

        obj_alert.confirm("Atenção", "Deseja realmente excluir este livro?", "question", fn_sim);


       // url = document.getElementById("linkdelete").href + "/" + id.toString();

       // $.colorbox({ iframe: true, innerWidth: "85%", height: "90%", innerHeight: "99%", href: url, scrolling: false });

    },



    pesquisar: function (obj, ev) {


        gridshared.call_form("grid0", "refresh", "", ev);

    },

    save: function (id_form, evt) {

        var buttonDomElement = evt.target;

        var old_text = $(buttonDomElement).text();

        $(buttonDomElement).attr('disabled', true).text("Aguarde Salvando");

        var form = document.getElementById(id_form);

        if (form.DateInserted.value == "") {

            form.DateInserted.value = form.hdData.value; //se aguém tentar salvar vazio, o javascript já deixa resolvido a partir daqui.
        }


        var valido_form = true;
        var validator = null;
        try {

            validator = $("#" + id_form).validate();
            console.log(validator);
            console.log(validator.form());

            valido_form = false;

            if (validator != null && validator.form()) {
                valido_form = true;
            }

        } catch (Exp) {

        }
        if (validator == null) {

            valido_form = true;
        }



        if (valido_form) {


            var url_salvar = "/Book/Save";

            if (document.getElementById("linksave") != "") {
                url_salvar = document.getElementById("linksave").href;
                console.log("obtendo url linksave: " + url_salvar);
            }

            var obj_tela = document.getElementById("obj_tela");
            var executar = false;

            // return false;
            $.post(url_salvar,
                $("#" + id_form).serialize(),
                function (data) {

                    
                    console.log(data);

                    if (data.success) {
                        obj_alert.show("Sucesso", "Livro " + document.getElementById(id_form).Title.value + " salvo com sucesso!", "success",
                            function (result) { parent.obj_book.pesquisar(null, evt); parent.$.colorbox.close(); });

                    } else {
                        obj_alert.show("Atenção", "Houve um erro ao tentar salvar", "warning");
                    }


                    $(buttonDomElement).attr('disabled', false).text(old_text);
              }

                

            ).fail(function (response) {


                var form = document.getElementById(id_form);
                form.action = url_salvar;
                form.target = "_blank";
                form.submit();

            });
        } else {
            console.log("ERRO na forma de testar o form: "); console.log(validator);
            $(buttonDomElement).attr('disabled', false).text(old_text);
        }

     

    },

    novo: function (obj, evt) {

        var url = document.getElementById("linknovo").href;
        $.colorbox({ iframe: true, innerWidth: "85%", height: "90%", innerHeight: "99%", href: url, scrolling: false });

    },

    exportarXls: function (obj, evt) {

        var qtde = parseInt(document.getElementById("grid0_grid_num_rows").value);

        if (qtde <= 0) {

            obj_alert.show("Atenção", "Não há registros para exportar", "warning");
        } else {

            var frm_consulta = document.frm_consulta;
            var old_action = frm_consulta.action;
            var old_target = frm_consulta.target;


            var linkexportaexcel = document.getElementById("linkexportaexcel");
            frm_consulta.target = "_blank";
            frm_consulta.action = linkexportaexcel.href;
            frm_consulta.submit();

            frm_consulta.target = old_target;
            frm_consulta.action = old_action;



        }
    }

    


}