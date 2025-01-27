using Microsoft.EntityFrameworkCore;
using ForNewTest.Contexto;
using ForNewTest.EndPoints;
using ForNewTest.IRepositorio;
using ForNewTest.Repositorio;
using ForNewTest.Servicios;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using ForNewTest.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ForNewTest.Utilidades;

var builder = WebApplication.CreateBuilder(args);

var origen = builder.Configuration.GetValue<string>("origin");

builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("auth", policy =>
    {
        policy.WithOrigins(origen).AllowAnyHeader();
    });
    opciones.AddDefaultPolicy( policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddDbContext<AplicationDBContext>(op => op.UseSqlServer("name=DefaultConnection"));
builder.Services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<AplicationDBContext>().AddDefaultTokenProviders();


builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();
// Add services to the container.
builder.Services.AddOutputCache();
builder.Services.AddScoped<IGeneroRepositorio, GeneroRepositorio>();
builder.Services.AddScoped<IActorRepositorio, ActorRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IComentarioRepositorio, ComentarioRepositorio>();
builder.Services.AddScoped<IErrorRepositorio, ErrorRepositorio>();
//builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();
builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer(opt => opt.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    //IssuerSigningKey = Llaves.ObtenerLlave(builder.Configuration).First(),
    IssuerSigningKeys = Llaves.ObtenerTodasLasLlaves(builder.Configuration),
    ClockSkew = TimeSpan.Zero
});
builder.Services.AddAuthorization();


var app = builder.Build();




// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseExceptionHandler(exp =>exp.Run(async context =>
{
    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
    var except = exceptionHandlerFeature?.Error;

    var error = new ErrorModel();
    error.Fecha = DateTime.UtcNow;
    error.MensajeError = except.Message;
    error.StackTrace = except.StackTrace;

    var repositorio = context.RequestServices.GetRequiredService<IErrorRepositorio>();
    await repositorio.CrearError(error);
    await TypedResults.BadRequest(new { Type = "Error", Message = "Ha ocurrido un error.", status=500 }).ExecuteAsync(context);
}));


app.UseStatusCodePages();


app.UseOutputCache();
app.UseStaticFiles();
app.UseCors();


app.UseAuthorization();
//Este codigo puesto con el app.UseExceptionHandler();, devuelve un json muy abrumador.
//app.mapget("/", () =>
//{
//    throw new invalidoperationexception("error");
//});
app.MapGroup("/generos").MapGenero();
app.MapGroup("/actores").MapActor();
app.MapGroup("/peliculas").MapPelicula();
app.MapGroup("/pelicula/{peliculaid:int}/comentarios").MapComentario();
app.MapGroup("/usuarios").MapUsuarios();



app.Run();
