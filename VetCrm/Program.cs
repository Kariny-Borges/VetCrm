using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using VetCrm.Data;
using VetCrm.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<VetCrmContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Autenticação por cookie: o ASP.NET cria um cookie no navegador quando o usuário faz login.
// Esse cookie é a "carteirinha" que prova que ele está autenticado em cada requisição.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";          // se não está logado, redireciona pra cá
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied"; // sem permissão
        options.ExpireTimeSpan = TimeSpan.FromHours(8);     // cookie dura 8h
        options.SlidingExpiration = true;                   // renova a cada uso
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// IMPORTANTE: a ordem importa! Authentication SEMPRE vem antes de Authorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// SEED: garante que o usuário "admin" exista no banco.
// Só insere se ainda não houver um usuário com Login = "admin".
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VetCrmContext>();
    if (!db.Usuarios.Any(u => u.Login == "admin"))
    {
        db.Usuarios.Add(new Usuario
        {
            Nome = "Administrador",
            Documento = "00000000000",
            Telefone = "(00) 00000-0000",
            Email = "admin@vetcrm.com",
            Login = "admin",
            Senha = "admin123", // texto puro só pra estudo - em produção: hash com BCrypt
            TipoPessoa = TipoPessoa.PF,
            Perfil = PerfilUsuario.Gerente
        });
        db.SaveChanges();
    }
}

app.Run();
