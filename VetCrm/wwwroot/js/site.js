// ========================================
// MASCARAS DE INPUT
// ========================================
// Funcao generica que aplica uma mascara a um campo de texto.
// "padrao" eh uma string como "###.###.###-##" onde # representa um digito.
// A cada tecla digitada, a funcao:
//   1. Remove tudo que nao eh numero do valor digitado
//   2. Percorre o padrao caractere por caractere
//   3. Se o caractere do padrao eh #, coloca o proximo digito
//   4. Se nao eh # (ponto, traco, barra), coloca o caractere fixo
// Resultado: o usuario digita so numeros e a mascara formata automaticamente.
function aplicarMascara(input, padrao) {
    input.addEventListener('input', function () {
        // Remove tudo que nao eh digito (0-9)
        var digitos = this.value.replace(/\D/g, '');
        var resultado = '';
        var posDigito = 0;

        // Percorre cada caractere do padrao
        for (var i = 0; i < padrao.length; i++) {
            // Se ja usou todos os digitos digitados, para
            if (posDigito >= digitos.length) break;

            if (padrao[i] === '#') {
                // # = lugar de um digito, coloca o proximo digito
                resultado += digitos[posDigito];
                posDigito++;
            } else {
                // Caractere fixo (ponto, traco, barra, parentese)
                resultado += padrao[i];
            }
        }

        this.value = resultado;
    });
}

// Mascara dinamica: verifica a classe do input no momento da digitacao.
// Isso permite que a mascara mude quando a classe eh trocada (ex: CPF -> CNPJ).
function aplicarMascaraDinamica(input) {
    input.addEventListener('input', function () {
        var padrao = '';

        if (this.classList.contains('mask-cpf')) {
            padrao = '###.###.###-##';
        } else if (this.classList.contains('mask-cnpj')) {
            padrao = '##.###.###/####-##';
        } else if (this.classList.contains('mask-cep')) {
            padrao = '#####-###';
        } else if (this.classList.contains('mask-telefone')) {
            padrao = '(##) #####-####';
        }

        if (!padrao) return;

        var digitos = this.value.replace(/\D/g, '');
        var resultado = '';
        var posDigito = 0;

        for (var i = 0; i < padrao.length; i++) {
            if (posDigito >= digitos.length) break;

            if (padrao[i] === '#') {
                resultado += digitos[posDigito];
                posDigito++;
            } else {
                resultado += padrao[i];
            }
        }

        this.value = resultado;
    });
}

// ========================================
// BUSCA CEP (API ViaCEP)
// ========================================
// Modelo baseado no exemplo do proprio site viacep.com.br
// Usa getElementById para pegar cada campo pelo id.
// Quando o usuario sai do campo CEP, busca o endereco na API e preenche os campos.
function buscarCep() {
    // Pega o campo CEP pelo id
    var campoCep = document.getElementById('cep');
    if (!campoCep) return;

    campoCep.addEventListener('blur', function () {
        // Pega so os numeros
        var cep = this.value.replace(/\D/g, '');

        // CEP precisa ter 8 digitos
        if (cep.length !== 8) return;

        // Chama a API do ViaCEP
        fetch('https://viacep.com.br/ws/' + cep + '/json/')
            .then(function (resposta) { return resposta.json(); })
            .then(function (dados) {
                if (dados.erro) return;

                // Preenche cada campo pelo id
                document.getElementById('logradouro').value = dados.logradouro || '';
                document.getElementById('complemento').value = dados.complemento || '';
                document.getElementById('bairro').value = dados.bairro || '';
                document.getElementById('cidade').value = dados.localidade || '';
                document.getElementById('uf').value = dados.uf || '';
            });
    });
}

// Quando a pagina termina de carregar
document.addEventListener('DOMContentLoaded', function () {

    // Mascaras
    var inputsMascara = document.querySelectorAll('.mask-cpf, .mask-cnpj, .mask-cep, .mask-telefone');
    inputsMascara.forEach(function (input) {
        aplicarMascaraDinamica(input);
    });

    // Busca CEP
    buscarCep();
});
