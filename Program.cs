using Microsoft.OpenApi;
using ProyectoDonacion.Services.Auth;
using ProyectoDonacion.Services.FireBase;
using Scalar.AspNetCore;

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

builder.Services.AddTransient<AuthService>()
    .AddTransient<FirebaseService>();

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
app.UseAuthorization();
app.MapControllers();
app.Run();