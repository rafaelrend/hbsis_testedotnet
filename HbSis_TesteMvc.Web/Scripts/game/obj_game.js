var obj_game = {

    vencedor: null,
    ar_rodadas: new Array(),

    add_player: function () {

        var tbody_game = document.getElementById("tbody_game");
        var div_template = document.getElementById("div_template");
        var hd_qtde_jogadores = document.getElementById("hd_qtde_jogadores");


        var intQtde = parseInt(hd_qtde_jogadores.value);
        intQtde++;

        var strid = intQtde.toString();

        var str = div_template.innerHTML;
        str = str.replace("template_jogada", "jogada_" + strid).replace("template_jogada", "jogada_" + strid);

        var tr = document.createElement("tr");
        tr.id = "tr_jogador_" + strid;

        var td1 = document.createElement("td");
        td1.innerHTML = "<input type='text' name='nome_jogador" + strid + "' id='nome_jogador" + strid + "' maxlength='100' value='Jogador " + strid + "' style='width: 220px' >"

        var td2 = document.createElement("td");
        td2.innerHTML = str;

        var td3 = document.createElement("td");
        td3.innerHTML = "<a href='#!' onclick='obj_game.remover(" + strid + ");'> <i class='fa fa-remove'></i> </a>"

        tr.appendChild(td1);
        tr.appendChild(td2);
        tr.appendChild(td3);

        tbody_game.appendChild(tr);

        obj_game.seta_class();


        hd_qtde_jogadores.value = intQtde.toString();
    },

    remover: function (id) {

        var tbody_game = document.getElementById("tbody_game");
        var tr = document.getElementById("tr_jogador_" + id);

        tbody_game.removeChild(tr);

        obj_game.seta_class();
    },

    seta_class: function () {

        var tbody_game = document.getElementById("tbody_game");

        var trs = tbody_game.getElementsByTagName("tr");

        var conta = 0;
        var conta_jogadores = 0;

        for (var i = 0; i < trs.length; i++) {
            conta_jogadores++;
            conta++;

            trs[i].className = "";
            if (conta >= 3) {
                trs[i].className = "tr_cinza";

                if (conta == 4) {
                    conta = 0; //reinicia essa conta..
                }
            }

        }
        document.getElementById("hd_qtde_jogadores_correto").value = conta_jogadores.toString();

    },
    //Se alto estiver errado, vou lançar uma exceção para poder atender ao estilo de erros proposto no enunciado..
    getArrayPlayers: function () {

        //WrongNumberOfPlayersError.        var intqtde_jogadores =
            parseInt(document.getElementById("hd_qtde_jogadores_correto").value);

        if (intqtde_jogadores == 0) {
            throw new Error('WrongNumberOfPlayersError|Número insuficiente de jogadores');
        }
        var n = intqtde_jogadores;

        if (n === 0 || !!(n && (n % 2))) {
            //É impar, mas vou deixar continuar assim mesmo..
            // throw new Error( 'WrongNumberOfPlayersError|O número de jogadores deve ser par' );
        }
        //vou buscar se há pares de jogares com o mesmo jogo...

        var tbody_game = document.getElementById("tbody_game");

        var trs = tbody_game.getElementsByTagName("tr");

        var rodadas = parseFloat(intqtde_jogadores) / parseFloat(2);
        var int_rodadas = parseInt(rodadas);

        console.log("total jogadores: " + intqtde_jogadores);
        console.log("rodadas double " + rodadas);
        console.log("rodadas int " + int_rodadas);
        if (rodadas > int_rodadas) {
            int_rodadas++;
        }

        var arr = new Array();

        for (var i = 1; i <= int_rodadas; i++) {

            var indx_primeiro_jogador = (i - 1) * 2 + 0;
            var indx_segundo_jogador = (i - 1) * 2 + 1;

            var arr_sub = new Array();

            var jogada_um = trs[indx_primeiro_jogador].children[1].children[0].value;
            var nome_jogador_1 = trs[indx_primeiro_jogador].children[0].children[0].value;
            arr_sub[0] = { player: nome_jogador_1, choice: jogada_um };

            if (trs[indx_segundo_jogador] != null) {
                var jogada_dois = trs[indx_segundo_jogador].children[1].children[0].value;

                var nome_jogador_2 = trs[indx_segundo_jogador].children[0].children[0].value;
                arr_sub[1] = { player: nome_jogador_2, choice: jogada_dois };
            }


            arr[arr.length] = arr_sub;
        }


        //cheguei aqui e nao emiti nenhuma exceção? Então posso jogar....
        return arr; //Retorna o array de jogadores para que seja analizado o campeão.
    },
    //Hora da verdade..
    obtem_vencedor(player1, player2) {

        if (player2 == null) {
            return player1; //player 1 vai esperar a vez ou já será o campeão..
        }

        if (player1.choice == player2.choice) {
            return player1; //Os dois escolheram a mesma coisa, o primeiro ganha..
        }

        if (player1.choice == "P") {
            //Player 1 escolheu papel.
            if (player2.choice == "B") {
                //pedra? Então play1 vence.
                return player1;
            }
            return player2;//Não há outra opção, só restou tesoura que corta o papel.
        }

        if (player1.choice == "B") {
            //Player 1 escolheu pedra..
            if (player2.choice == "S") {
                //Tesoura? Então play1 vence.
                return player1;
            }
            return player2;//Não há outra opção, só restou papel que envolve a pedra.
        }
        if (player1.choice == "S") {
            //Player 1 escolheu tesoura..
            if (player2.choice == "P") {
                //Papel? Então play1 vence.
                return player1;
            }
            return player2;//Não há outra opção, só restou pedra que quebra a tesoura.
        }

        return player1; //Algum situação inusitada, temos o player1.
    },

    //Pegamos um array, e vamos retornar outro array até que não tenhamos mais itens pra avaliar.
    get_winner_fromDoubleArray(arr, index) {

        if (this.vencedor != null) {
            return true;
        }

        var vencedores = new Array();
        var msg_rodada = "";

        for (var i = 0; i < arr.length; i++) {
            var ar_sub = arr[i];

            
            var vencedor = this.obtem_vencedor(ar_sub[0], ar_sub[1]);
            vencedores[vencedores.length] = vencedor;


            if (ar_sub[0] != null && ar_sub[1] != null) {
                msg_rodada += "<br>" + ar_sub[0].player + " (" + this.descreveEscolha(ar_sub[0].choice) + ") VS " + ar_sub[1].player +
                    " (" + this.descreveEscolha(ar_sub[1].choice) + ") ";

                msg_rodada += " = Vencedor: " + vencedor.player + " ( " + this.descreveEscolha(vencedor.choice) + " ) ";
            } else {
                msg_rodada += "<br> " + ar_sub[0].player + " aguarda um oponente. ";
            }



        }
        this.ar_rodadas[this.ar_rodadas.length] = msg_rodada;

        if (vencedores.length > 1) {
            index++;
            var proxima_rodada = this.get_array_proxima_rodada(vencedores, index);
            this.get_winner_fromDoubleArray(proxima_rodada);
        } else {
            //Temos o vencedor!!!
            this.vencedor = vencedores[0];
        }


    },

    get_array_proxima_rodada: function (vencedores) {
        var metade = parseFloat(vencedores.length) / parseFloat(2);
        var int_metade = parseInt(metade);

        if (metade > int_metade)
            int_metade++;

        var arr = new Array();

        for (var i = 1; i <= int_metade; i++) {

            var indx_primeiro_jogador = (i - 1) * 2 + 0;
            var indx_segundo_jogador = (i - 1) * 2 + 1;

            var arr_sub = new Array();

            var jogada_um = vencedores[indx_primeiro_jogador].choice;
            var nome_jogador_1 = vencedores[indx_primeiro_jogador].player;

            arr_sub[0] = { player: nome_jogador_1, choice: jogada_um };

            if (vencedores[indx_segundo_jogador] != null) {
                var jogada_dois = vencedores[indx_segundo_jogador].choice;

                var nome_jogador_2 = vencedores[indx_segundo_jogador].player;
                arr_sub[1] = { player: nome_jogador_2, choice: jogada_dois };
            }


            arr[arr.length] = arr_sub;
        }

        return arr;
    },

    rps_game_winner: function () { //puta nome feio pra uma função. Enfim, isso aqui vai trazer o vencedor.
        this.vencedor = null;
        this.ar_rodadas = new Array();

        var players = Array();

        try {

            players = this.getArrayPlayers();
            var indx = 1;
            this.get_winner_fromDoubleArray(players);
            this.escreveRodadas();

            if (this.vencedor != null) {
                obj_alert.show("Vencedor", "O vencedor do jogo é " + this.vencedor.player + " com a jogada " + this.descreveEscolha(this.vencedor.choice), "success");
            }

            //console.log(this.ar_rodadas);
            //console.log(this.vencedor);

           // console.log(players);
        } catch (exp) {

            var ar = exp.message.split('|');

            obj_alert.show(ar[0], ar[1], "error");
        }

    },

    descreveEscolha: function( tip){

        if (tip == "P")
            return "Papel";


        if (tip == "B")
            return "Pedra";

        return "Tesoura";

    },
    escreveRodadas: function () {

        var HTML = "";
        for (var i = 0; i < this.ar_rodadas.length; i++) {
            var rodada = i + 1;
            HTML += "<div class='well'><h3>Rodada " + rodada.toString() + " </h3>" +
                this.ar_rodadas[i] + "</div>";
        }
        HTML += "<div class='well'><h3>Vencedor!</h3>" + this.vencedor.player + " (" +
            this.descreveEscolha(this.vencedor.choice) + ") </div>";

        $("#div_resultado").html(HTML);
        
    }




}