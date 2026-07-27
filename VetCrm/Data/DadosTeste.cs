using VetCrm.Models;

namespace VetCrm.Data
{
    // ============================================================
    // DADOS DE TESTE (fictícios)
    // ============================================================
    // Serve só pra encher as listas e conseguir testar os filtros.
    // É chamado pelo Program.cs quando o app inicia.
    //
    // Regra de ouro deste arquivo: cada item só é inserido SE AINDA NÃO EXISTIR.
    // Por isso você pode rodar o app quantas vezes quiser que nada duplica.
    //
    // Pra DESLIGAR: comente a linha "DadosTeste.Semear(db);" no Program.cs.
    // Pra APAGAR os dados depois: exclua pelas telas ou pelo banco.
    // ============================================================
    public static class DadosTeste
    {
        public static void Semear(VetCrmContext db)
        {
            // A ordem importa: quem é dependência vem primeiro.
            SemearEspecies(db);
            SemearRacas(db);            // precisa de Espécie
            SemearEspecialidades(db);
            SemearProprietarios(db);
            SemearVeterinarios(db);     // precisa de Especialidade
            SemearPacientes(db);        // precisa de Proprietário + Raça (e a Espécie vem da Raça)
        }

        // ------------------------------------------------------------
        // 1) ESPÉCIES
        // ------------------------------------------------------------
        private static void SemearEspecies(VetCrmContext db)
        {
            var nomes = new[]
            {
                "Cachorro", "Gato", "Ave", "Coelho", "Hamster",
                "Tartaruga", "Peixe", "Furão", "Porquinho-da-índia"
            };

            foreach (var nome in nomes)
            {
                // Any(...) pergunta pro banco: "já existe alguma espécie com esse nome?"
                if (!db.Especies.Any(e => e.Nome == nome))
                {
                    db.Especies.Add(new Especie { Nome = nome });
                }
            }

            // SaveChanges aqui é obrigatório: as Raças abaixo precisam do Id das Espécies.
            db.SaveChanges();
        }

        // ------------------------------------------------------------
        // 2) RAÇAS (cada uma ligada a uma espécie)
        // ------------------------------------------------------------
        private static void SemearRacas(VetCrmContext db)
        {
            // Cada linha é: (nome da espécie, nome da raça)
            var racas = new[]
            {
                ("Cachorro", "Labrador"),
                ("Cachorro", "Golden Retriever"),
                ("Cachorro", "Poodle"),
                ("Cachorro", "Bulldog Francês"),
                ("Cachorro", "Shih Tzu"),
                ("Cachorro", "Pastor Alemão"),
                ("Cachorro", "Beagle"),
                ("Cachorro", "Yorkshire"),
                ("Cachorro", "Pinscher"),
                ("Cachorro", "Border Collie"),
                ("Cachorro", "Rottweiler"),
                ("Cachorro", "Vira-lata (SRD)"),
                ("Gato", "Siamês"),
                ("Gato", "Persa"),
                ("Gato", "Maine Coon"),
                ("Gato", "Angorá"),
                ("Gato", "Bengal"),
                ("Gato", "Sphynx"),
                ("Gato", "Sem raça definida"),
                ("Ave", "Calopsita"),
                ("Ave", "Periquito Australiano"),
                ("Ave", "Canário Belga"),
                ("Ave", "Agapornis"),
                ("Coelho", "Mini Lop"),
                ("Coelho", "Angorá Inglês"),
                ("Hamster", "Sírio"),
                ("Hamster", "Anão Russo"),
                ("Tartaruga", "Tigre d'água"),
                ("Peixe", "Betta"),
                ("Furão", "Furão Doméstico"),
                ("Porquinho-da-índia", "Abissínio")
            };

            foreach (var (nomeEspecie, nomeRaca) in racas)
            {
                // Busca a espécie que já foi salva no passo 1 pra pegar o Id dela.
                var especie = db.Especies.FirstOrDefault(e => e.Nome == nomeEspecie);
                if (especie == null) continue; // segurança: se não achou, pula

                if (!db.Racas.Any(r => r.Nome == nomeRaca))
                {
                    db.Racas.Add(new Raca { Nome = nomeRaca, EspecieId = especie.Id });
                }
            }

            db.SaveChanges();
        }

        // ------------------------------------------------------------
        // 3) ESPECIALIDADES (usadas pelos veterinários)
        // ------------------------------------------------------------
        private static void SemearEspecialidades(VetCrmContext db)
        {
            var nomes = new[]
            {
                "Clínica Geral", "Cirurgia", "Dermatologia", "Cardiologia",
                "Ortopedia", "Oftalmologia", "Odontologia", "Anestesiologia",
                "Oncologia", "Animais Silvestres"
            };

            foreach (var nome in nomes)
            {
                if (!db.Especialidades.Any(e => e.Nome == nome))
                {
                    db.Especialidades.Add(new Especialidade { Nome = nome });
                }
            }

            db.SaveChanges();
        }

        // ------------------------------------------------------------
        // 4) PROPRIETÁRIOS
        // ------------------------------------------------------------
        // Sobrenomes repetidos de propósito (vários "Silva", vários "Ana"),
        // pra dar pra testar o filtro trazendo VÁRIOS resultados de uma vez.
        private static void SemearProprietarios(VetCrmContext db)
        {
            // (nome, CPF) — CPFs fictícios, no mesmo formato que a máscara da tela gera
            var proprietarios = new[]
            {
                ("Ana Paula Silva",        "111.222.333-01"),
                ("Bruno Almeida Silva",    "111.222.333-02"),
                ("Carla Souza",            "111.222.333-03"),
                ("Daniel Oliveira",        "111.222.333-04"),
                ("Eduarda Silva Ramos",    "111.222.333-05"),
                ("Fernando Costa",         "111.222.333-06"),
                ("Gabriela Martins",       "111.222.333-07"),
                ("Henrique Souza Lima",    "111.222.333-08"),
                ("Isabela Ferreira",       "111.222.333-09"),
                ("João Pedro Silva",       "111.222.333-10"),
                ("Karina Rodrigues",       "111.222.333-11"),
                ("Lucas Mendes",           "111.222.333-12"),
                ("Mariana Alves",          "111.222.333-13"),
                ("Ana Clara Nogueira",     "111.222.333-14"),
                ("Otávio Barbosa",         "111.222.333-15"),
                ("Patrícia Gomes",         "111.222.333-16"),
                ("Rafael Silva Torres",    "111.222.333-17"),
                ("Sabrina Duarte",         "111.222.333-18"),
                ("Thiago Moreira",         "111.222.333-19"),
                ("Ana Lúcia Cardoso",      "111.222.333-20"),
                ("Vinícius Rocha",         "111.222.333-21"),
                ("Yasmin Pereira",         "111.222.333-22"),
                ("Wagner Silva",           "111.222.333-23"),
                ("Beatriz Campos",         "111.222.333-24"),
                ("Marcos Antônio Dias",    "111.222.333-25")
            };

            var diasAtras = 0;

            foreach (var (nome, cpf) in proprietarios)
            {
                diasAtras += 5;

                // Aqui a checagem é pelo CPF, que é único de verdade
                // (dois clientes podem ter o mesmo nome, mas não o mesmo CPF).
                if (!db.Proprietarios.Any(p => p.CPF == cpf))
                {
                    db.Proprietarios.Add(new Proprietario
                    {
                        Nome = nome,
                        CPF = cpf,
                        DataCadastro = DateTime.Today.AddDays(-diasAtras)
                    });
                }
            }

            db.SaveChanges();
        }

        // ------------------------------------------------------------
        // 5) VETERINÁRIOS
        // ------------------------------------------------------------
        private static void SemearVeterinarios(VetCrmContext db)
        {
            // (nome, CRMV, nome da especialidade)
            var veterinarios = new[]
            {
                ("Dra. Amanda Ribeiro",      "CRMV-SP 10001", "Clínica Geral"),
                ("Dr. Bernardo Tavares",     "CRMV-SP 10002", "Cirurgia"),
                ("Dra. Camila Fontes",       "CRMV-SP 10003", "Dermatologia"),
                ("Dr. Diego Marques",        "CRMV-MG 10004", "Cardiologia"),
                ("Dra. Elisa Prado",         "CRMV-MG 10005", "Ortopedia"),
                ("Dr. Felipe Andrade",       "CRMV-RJ 10006", "Oftalmologia"),
                ("Dra. Giovana Lopes",       "CRMV-RJ 10007", "Odontologia"),
                ("Dr. Hugo Nascimento",      "CRMV-SP 10008", "Anestesiologia"),
                ("Dra. Isadora Bastos",      "CRMV-PR 10009", "Oncologia"),
                ("Dr. Jonas Ferreira",       "CRMV-PR 10010", "Animais Silvestres"),
                ("Dra. Letícia Aguiar",      "CRMV-SP 10011", "Clínica Geral"),
                ("Dr. Murilo Castro",        "CRMV-SC 10012", "Cirurgia")
            };

            foreach (var (nome, crmv, nomeEspecialidade) in veterinarios)
            {
                var especialidade = db.Especialidades.FirstOrDefault(e => e.Nome == nomeEspecialidade);

                if (!db.Veterinarios.Any(v => v.CRMV == crmv))
                {
                    db.Veterinarios.Add(new Veterinario
                    {
                        Nome = nome,
                        CPF = "999.888.777-" + crmv.Substring(crmv.Length - 2),
                        CRMV = crmv,
                        EspecialidadeId = especialidade?.Id // ? = se não achou, fica sem especialidade
                    });
                }
            }

            db.SaveChanges();
        }

        // ------------------------------------------------------------
        // 6) PACIENTES
        // ------------------------------------------------------------
        private static void SemearPacientes(VetCrmContext db)
        {
            // (nome do bicho, nome da raça, sexo, idade, peso)
            // Repare: NÃO informo a espécie aqui. Ela vem da própria Raça,
            // que já sabe a qual espécie pertence — assim não tem risco de
            // cadastrar um "Gato da raça Labrador".
            var pacientes = new[]
            {
                ("Rex",       "Labrador",             "Macho", 5,  32.5m),
                ("Thor",      "Pastor Alemão",        "Macho", 3,  38.0m),
                ("Mel",       "Golden Retriever",     "Fêmea", 7,  28.4m),
                ("Nina",      "Poodle",               "Fêmea", 2,  6.2m),
                ("Bob",       "Beagle",               "Macho", 4,  12.8m),
                ("Luna",      "Border Collie",        "Fêmea", 1,  17.0m),
                ("Fred",      "Bulldog Francês",      "Macho", 6,  11.5m),
                ("Amora",     "Shih Tzu",             "Fêmea", 9,  7.1m),
                ("Zeus",      "Rottweiler",           "Macho", 4,  45.2m),
                ("Pipoca",    "Yorkshire",            "Fêmea", 3,  3.4m),
                ("Bolinha",   "Pinscher",             "Macho", 8,  4.0m),
                ("Toby",      "Vira-lata (SRD)",      "Macho", 10, 19.6m),
                ("Maya",      "Vira-lata (SRD)",      "Fêmea", 2,  15.3m),
                ("Simba",     "Siamês",               "Macho", 3,  4.8m),
                ("Nala",      "Persa",                "Fêmea", 5,  4.1m),
                ("Frajola",   "Sem raça definida",    "Macho", 6,  5.5m),
                ("Mimi",      "Angorá",               "Fêmea", 4,  3.9m),
                ("Garfield",  "Maine Coon",           "Macho", 7,  8.7m),
                ("Cleo",      "Bengal",               "Fêmea", 1,  3.2m),
                ("Salem",     "Sphynx",               "Macho", 2,  4.4m),
                ("Mingau",    "Sem raça definida",    "Macho", 11, 6.0m),
                ("Loki",      "Calopsita",            "Macho", 2,  0.1m),
                ("Kiwi",      "Periquito Australiano","Fêmea", 1,  0.05m),
                ("Piu",       "Canário Belga",        "Macho", 3,  0.03m),
                ("Zazu",      "Agapornis",            "Macho", 2,  0.06m),
                ("Coelhinho", "Mini Lop",             "Macho", 1,  2.1m),
                ("Flor",      "Angorá Inglês",        "Fêmea", 2,  1.8m),
                ("Ted",       "Sírio",                "Macho", 1,  0.15m),
                ("Nuvem",     "Anão Russo",           "Fêmea", 1,  0.04m),
                ("Cascão",    "Tigre d'água",         "Macho", 4,  0.9m),
                ("Nemo",      "Betta",                "Macho", 1,  0.01m),
                ("Bandit",    "Furão Doméstico",      "Macho", 3,  1.2m),
                ("Chico",     "Abissínio",            "Macho", 2,  0.8m)
            };

            // Pega a lista de donos uma vez só, pra ir distribuindo os bichos entre eles.
            var donos = db.Proprietarios.OrderBy(p => p.Id).ToList();
            if (donos.Count == 0) return; // sem dono não dá pra cadastrar paciente

            for (var i = 0; i < pacientes.Length; i++)
            {
                var (nome, nomeRaca, sexo, idade, peso) = pacientes[i];

                var raca = db.Racas.FirstOrDefault(r => r.Nome == nomeRaca);
                if (raca == null) continue;

                if (db.Pacientes.Any(p => p.Nome == nome)) continue;

                // O % (resto da divisão) faz a lista de donos "dar a volta":
                // paciente 0 -> dono 0, paciente 1 -> dono 1 ... e quando acaba, volta pro 0.
                var dono = donos[i % donos.Count];

                db.Pacientes.Add(new Paciente
                {
                    Nome = nome,
                    Idade = idade,
                    Sexo = sexo,
                    Peso = peso,
                    DataCadastro = DateTime.Today.AddDays(-i * 3),
                    ProprietarioId = dono.Id,
                    RacaId = raca.Id,
                    EspecieId = raca.EspecieId // a espécie vem da raça
                });
            }

            db.SaveChanges();
        }
    }
}
