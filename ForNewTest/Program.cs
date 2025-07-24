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
using Azure.Identity;
using Microsoft.OpenApi.Models;
using ForNewTest.Swagger;
using ForNewTest.GraphQL;

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
builder.Services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddAuthorization()
                .AddProjections()
                .AddFiltering()
                .AddSorting();
builder.Services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<AplicationDBContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();
// Add services to the container.
//builder.Services.AddOutputCache();
builder.Services.AddStackExchangeRedisOutputCache(opc=>
{
    opc.Configuration = builder.Configuration.GetConnectionString("redis");
});

builder.Services.AddScoped<IGeneroRepositorio, GeneroRepositorio>();
builder.Services.AddScoped<IActorRepositorio, ActorRepositorio>();
builder.Services.AddScoped<IPersonaRepositorio, PersonaRepositorio>();
builder.Services.AddScoped<IPeliculaRepositorio, PeliculaRepositorio>();
builder.Services.AddScoped<IComentarioRepositorio, ComentarioRepositorio>();
builder.Services.AddScoped<IErrorRepositorio, ErrorRepositorio>();
//builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosAzure>();
builder.Services.AddScoped<IAlmacenadorArchivos, AlmacenadorArchivosLocal>();
builder.Services.AddTransient<IServiciosUsuarios, ServiciosUsuarios>();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1",new OpenApiInfo
    {
        Title="Api de Peliculas",
        Description = "Probando mi api",
        Contact= new OpenApiContact
        {
            Email="paulsergioc72@gmail.com",
            Name="Paul Sergio",
            Url=new Uri("https://github.com/poolsergio123")            
        },
        License=new OpenApiLicense
        {
            Name="MIT",
            Url=new Uri("https://opensource.org/license/mit/")
        }
    });
    x.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
        Name="Authorization",
        Type=SecuritySchemeType.ApiKey,
        Scheme="Bearer",
        BearerFormat = "JWT",
        In=ParameterLocation.Header
    });
    x.OperationFilter<FIltroAutorizacion>();
    
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer(opt => {
    opt.MapInboundClaims = false;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //IssuerSigningKey = Llaves.ObtenerLlave(builder.Configuration).First(),
        IssuerSigningKeys = Llaves.ObtenerTodasLasLlaves(builder.Configuration),
        ClockSkew = TimeSpan.Zero
    };
}
);
builder.Services.AddAuthorization(auth => {
    auth.AddPolicy("esadmin",politica =>politica.RequireClaim("esadmin"));
});


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
    var exception = exceptionHandlerFeature?.Error!;

    var error = new ErrorModel();
    error.StackTrace = exception.StackTrace!;
    error.MensajeError = exception.Message;
    error.Fecha = DateTime.UtcNow;

    var repositorio = context.RequestServices.GetRequiredService<IErrorRepositorio>();
    await repositorio.CrearError(error);
    await TypedResults.BadRequest(new {Type = "Error",Message = "Ha ocurrido un error, revisa los parametros y vuelve a intentarlo.",Status =500}).ExecuteAsync(context);
}));



app.UseStatusCodePages();


app.UseOutputCache();
app.UseStaticFiles();
app.UseCors();


app.UseAuthorization();
app.MapGraphQL();
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
app.MapGroup("/personas").MapPersona();



app.Run();
