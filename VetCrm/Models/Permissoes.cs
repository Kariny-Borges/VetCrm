namespace VetCrm.Models
{
    public static class Permissoes
    {
        public const string TipoClaim = "permissao";

        // Cadastros
        public const string ProprietarioVer = "Proprietario.Ver";
        public const string PacienteVer = "Paciente.Ver";
        public const string ContatoVer = "Contato.Ver";
        public const string EspecieVer = "Especie.Ver";
        public const string RacaVer = "Raca.Ver";
        public const string EstabelecimentoVer = "Estabelecimento.Ver";
        public const string UsuarioVer = "Usuario.Ver";

        // Clínica
        public const string AgendaVer = "Agenda.Ver";
        public const string VeterinarioVer = "Veterinario.Ver";
        public const string EspecialidadeVer = "Especialidade.Ver";
        public const string ConsultaVer = "Consulta.Ver";
        public const string InternacaoVer = "Internacao.Ver";
        public const string ProntuarioVer = "Prontuario.Ver";
        public const string ExameVer = "Exame.Ver";
        public const string VacinaVer = "Vacina.Ver";

        // Estoque
        public const string ProdutoVer = "Produto.Ver";
        public const string CategoriaVer = "Categoria.Ver";

        // Financeiro
        public const string ServicoVer = "Servico.Ver";
        public const string FormaPagamentoVer = "FormaPagamento.Ver";

        public static List<string> DoPerfil(PerfilUsuario perfil)
        {
            return perfil switch
            {
                PerfilUsuario.Gerente => new List<string>
                {
                    ProprietarioVer, PacienteVer, ContatoVer, EspecieVer, RacaVer,
                    EstabelecimentoVer, UsuarioVer, AgendaVer, VeterinarioVer,
                    EspecialidadeVer, ConsultaVer, InternacaoVer, ProntuarioVer,
                    ExameVer, VacinaVer, ProdutoVer, CategoriaVer, ServicoVer,
                    FormaPagamentoVer
                },

                PerfilUsuario.Administrativo => new List<string>
                {
                    AgendaVer, ProprietarioVer, PacienteVer, ContatoVer,
                    EspecieVer, RacaVer, EstabelecimentoVer, VeterinarioVer,
                    EspecialidadeVer, ConsultaVer, InternacaoVer, ExameVer,
                    VacinaVer, ProdutoVer, CategoriaVer
                },

                PerfilUsuario.Financeiro => new List<string>
                {
                    ProprietarioVer, ProdutoVer, CategoriaVer, ServicoVer, FormaPagamentoVer
                },

                PerfilUsuario.Recepcao => new List<string>
                {
                    AgendaVer, ProprietarioVer, PacienteVer, ContatoVer,
                    VeterinarioVer, ConsultaVer, InternacaoVer, ExameVer, VacinaVer
                },

                _ => new List<string>()
            };
        }
    }
}
