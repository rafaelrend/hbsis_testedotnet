//Nunca me lembro das chamadas da biblioteca sweetalert, então vou jogar um facilitador aqui.
var obj_alert = {


        confirm: function(title, msg, tip, fn_final ){

            if (tip == null) {
                tip = "question"; //warning
            }

            swal({
                title: title,
                text: msg,
                type: tip,
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sim'
            }).then(
                function (result) {

                        if (fn_final != null) {
                            fn_final(result);
                        }
                }

            );
    },


    show: function(title, msg, tip, fn_final) {

        if (tip == null) {
            tip = "info"; //warning
        }

        swal({
            title: title,
            text: msg,
            type: tip,
            confirmButtonText: 'OK'
        }).then(
            function (result) {

            if (fn_final != null) {
                fn_final(result);
            }
          }
        );
    }

}