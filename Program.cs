using System.Text.Json;
using EventsApi.Data;
using EventsApi.Middleware;
using EventsApi.Repositorio;
using EventsApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configurar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionDevilServ")));

// Configurar AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configurar servicios
builder.Services.AddControllers();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<EventoService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IInscriptionRepository, InscripcionRepository>();
builder.Services.AddScoped<InscripcionService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<PasswordService>();

// Configurar JwtTokenHelper como singleton
builder.Services.AddSingleton<JwtTokenHelper>(provider =>
{
    string secretKey = builder.Configuration["Jwt:Key"]!;
    return new JwtTokenHelper(secretKey);
});

// Configuración de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor inserte el jwt despues de la palabra bearer de esta forma \"<strong>bearer {JWT}</strong>\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
            }
        });
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Events API",
        Version = "v1",
        Description = "API para gestionar eventos",
        Contact = new OpenApiContact
        {
            Name = "Adrián Monsalve",
            Email = "97.amonsalve@gmail.com",
        }
    });
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Events API",
        Version = "v2",
        Description = "API para gestionar eventos - Segunda versión",
        Contact = new OpenApiContact
        {
            Name = "Adrián",
            Email = "97.amonsalve@gmail.com",
        }
    });
});

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });


// Configuración de autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "EventsApi",
            ValidAudience = "EventsApi",
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Jwt:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Capturar el encabezado "Authorization"
                string? authorization = context.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorization))
                {
                    // Si no contiene "Bearer", se toma como el token directamente
                    if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        context.Token = authorization; // Asignar el token directamente
                    }
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Error de autenticación: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validado correctamente.");
                foreach (var claim in context.Principal!.Claims)
                {
                    Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
                }
                return Task.CompletedTask;
            }
        };
    });

WebApplication app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Events API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Events API v2");
    });
}

app.UseHttpsRedirection();

// Aplicar la política CORS global
app.UseCors("AllowAll");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
