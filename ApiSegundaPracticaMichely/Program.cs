using ApiSegundaPracticaMichely.Data;
using ApiSegundaPracticaMichely.Helpers;
using ApiSegundaPracticaMichely.Repositories;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
HelperOAuthServices helper =
    new HelperOAuthServices(builder.Configuration);
//ESTA INSTANCIA DEL HELPER DEBEMOS INCLUIRLA DENTRO 
//DE NUESTRA APLICACION SOLAMENTE UNA VEZ, PARA QUE 
//TODO LO QUE HEMOS CREADO DENTRO NO SE GENERE DE NUEVO
builder.Services.AddSingleton<HelperOAuthServices>(helper);

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});


SecretClient secretClient =
builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret secret =
    await secretClient.GetSecretAsync("SqlAzure");
string connectionString = secret.Value; 
builder.Services.AddTransient<RepositoryCubo>();
builder.Services.AddDbContext<CubosContext>(options => options.UseSqlServer(connectionString));
//HABILITAMOS LOS SERVICIOS DE AUTHENTICATION QUE HEMOS 
//CREADO EN EL HELPER CON Action<>
builder.Services.AddAuthentication
    (helper.GetAuthenticateSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());
builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Api Examen",
            Description = "Estamos realizando una Api de Series",
            Version = "v1",
            Contact = new OpenApiContact()
            {
                Name = "Michely"
            }
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(
options =>
{
    options.SwaggerEndpoint(
        url: "/swagger/v1/swagger.json", name: "Api v1");
    options.RoutePrefix = "";
});

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
