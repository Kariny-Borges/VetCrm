using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<VetCrmContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Permissoes.ProprietarioVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.ProprietarioVer));
    options.AddPolicy(Permissoes.PacienteVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.PacienteVer));
    options.AddPolicy(Permissoes.ContatoVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.ContatoVer));
    options.AddPolicy(Permissoes.EspecieVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.EspecieVer));
    options.AddPolicy(Permissoes.RacaVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.RacaVer));
    options.AddPolicy(Permissoes.EstabelecimentoVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.EstabelecimentoVer));
    options.AddPolicy(Permissoes.UsuarioVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.UsuarioVer));
    options.AddPolicy(Permissoes.AgendaVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.AgendaVer));
    options.AddPolicy(Permissoes.VeterinarioVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.VeterinarioVer));
    options.AddPolicy(Permissoes.EspecialidadeVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.EspecialidadeVer));
    options.AddPolicy(Permissoes.ConsultaVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.ConsultaVer));
    options.AddPolicy(Permissoes.InternacaoVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.InternacaoVer));
    options.AddPolicy(Permissoes.ProntuarioVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.ProntuarioVer));
    options.AddPolicy(Permissoes.ExameVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.ExameVer));
    options.AddPolicy(Permissoes.VacinaVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.VacinaVer));
    options.AddPolicy(Permissoes.ProdutoVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.ProdutoVer));
    options.AddPolicy(Permissoes.CategoriaVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.CategoriaVer));
    options.AddPolicy(Permissoes.ServicoVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.ServicoVer));
    options.AddPolicy(Permissoes.FormaPagamentoVer, p => p.RequireClaim(Permissoes.TipoClaim, Permissoes.FormaPagamentoVer));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VetCrmContext>();
    if (!db.Usuarios.Any(u => u.Login == "admin"))
    {
        db.Usuarios.Add(new Usuario
        {
            Nome = "Administrador",
            CPF = "00000000000",
            Telefone = "(00) 00000-0000",
            Email = "admin@vetcrm.com",
            Login = "admin",
            Senha = "admin123", // texto puro só pra estudo - em produção: hash com BCrypt
            Perfil = PerfilUsuario.Gerente
        });
        db.SaveChanges();
    }

    // Dados fictícios pra encher as listas e testar os filtros.
    // Comente esta linha quando não precisar mais (ver Data/DadosTeste.cs).
    DadosTeste.Semear(db);
}

app.Run();