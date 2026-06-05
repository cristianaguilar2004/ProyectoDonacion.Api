using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ProyectoDonacion.Services.Auth;
using ProyectoDonacion.Services.Categorias;
using ProyectoDonacion.Services.EstadoArticulos;
using ProyectoDonacion.Services.FireBase;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
        return Task.CompletedTask;
    });
});

// Autenticacion JWT
// Le decimos a al app que el esquema de auth es JWT Bearer
// Bearer significa que el token ciaja en el header: Authorization: Bearer <token>
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Verificar que el token lo emitimos nosotros (app)
        ValidateIssuer = true,
        // Verificar que el token est para la misma app
        ValidateAudience = true,
        // Verificar que el token no ha expirado
        ValidateLifetime = true,
        // Verificar que la firma es valida
        ValidateIssuerSigningKey = true,
        // Verificar que estos valores coincidan con los que usamos para generar el token
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// AddAuthorization, habilitar el uso del [Authorize] en los controllers
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODExNjM1MjAwIiwiaWF0IjoiMTc4MDEyNjAwMiIsImFjY291bnRfaWQiOiIwMTllNzdjNmNkZWY3ODAwOTk3NjZmZDA0YmI5NDc3MiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa3N2d2ViZXE0dGE2eXNzMGR2a2d0NWRhIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.WiDqb2CyHQOXQts8e0oWxybp34Aqn1k5Q5aQ-3zcM6cCYmfJ66tmrpmg2MiLedaLbXuOHg3JB26OrUaJwhZkXBDd2mxqq6xnMZAh-5a6rjvhZ_ss7d1aAxxR-r5GTxc8CLErkG-kxK5N72tJRKS7MYgX6Gp1xAgvqE-1KESQtwr22lhsLiicDeTFOxT70AkmzA6I6smVM4ylGy8J4V_9SrWabQl_W-uRdZvs00Iz4CypsN4RU_ooIZUVFuFAR1anmMoQulTcSQklPsayIyolRpqxyx31eXiBpU5TjSLNNLhrpnEBesdFkMvosRYg9buLIy-ouPaGpl_f8E8w0tZvTA", typeof(Program));

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<AuthService>()
    .AddTransient<RolService>()
    .AddTransient<FirebaseService>()
    .AddScoped<UsuarioAutenticadoService>()
    .AddTransient<CategoriaService>()
    .AddTransient<EstadoArticuloService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Sistema Emergencias API")
               .WithTheme(ScalarTheme.DeepSpace)
               .WithClassicLayout()
               .ExpandAllTags()
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.Http)
               .EnablePersistentAuthentication();
    });
}

app.UseHttpsRedirection();

// IMPORTANTE: UseAuthentication DEBE ir ANTES de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();