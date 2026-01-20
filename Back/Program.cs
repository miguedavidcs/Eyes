using Back.Data;
using Back.Data.Seed;
using Back.Models;
using Back.Repositories;
using Back.Repositories.Interfaces;
using Back.Security;
using Back.Security.CurrentUser;
using Back.Security.Permissions;
using Back.Service;
using Back.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// Docker
// =====================================================
builder.WebHost.UseUrls("http://0.0.0.0:8080");

// =====================================================
// Controllers & Swagger
// =====================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// =====================================================
// HttpContext / Current User
// =====================================================
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();

// =====================================================
// Swagger JWT
// =====================================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Enterprice API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
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
            Array.Empty<string>()
        }
    });
});

// =====================================================
// DbContext
// =====================================================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

// =====================================================
// CORS
// =====================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// =====================================================
// AutoMapper
// =====================================================
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// =====================================================
// Repositories
// =====================================================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// =====================================================
// Services
// =====================================================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtService>();

// =====================================================
// Authentication JWT
// =====================================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ),
        ClockSkew = TimeSpan.Zero
    };
});

// =====================================================
// Authorization Policies
// =====================================================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("USERS_VIEW",
        policy => policy.Requirements.Add(new PermissionRequirement("USERS_VIEW")));

    options.AddPolicy("USERS_CREATE",
        policy => policy.Requirements.Add(new PermissionRequirement("USERS_CREATE")));

    options.AddPolicy("USERS_UPDATE",
        policy => policy.Requirements.Add(new PermissionRequirement("USERS_UPDATE")));

    options.AddPolicy("USERS_DELETE",
        policy => policy.Requirements.Add(new PermissionRequirement("USERS_DELETE")));

    options.AddPolicy("ROLES_VIEW",
        policy => policy.Requirements.Add(new PermissionRequirement("ROLES_VIEW")));

    options.AddPolicy("ROLES_CREATE",
        policy => policy.Requirements.Add(new PermissionRequirement("ROLES_CREATE")));

    options.AddPolicy("ROLES_UPDATE",
        policy => policy.Requirements.Add(new PermissionRequirement("ROLES_UPDATE")));
});

builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

// =====================================================
// Build
// =====================================================
var app = builder.Build();

// =====================================================
// SEEDS
// =====================================================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    foreach (var role in RoleSeed.Get())
        if (!context.Roles.Any(r => r.Name == role.Name))
            context.Roles.Add(role);

    context.SaveChanges();

    foreach (var permission in PermissionSeed.Get())
        if (!context.Permissions.Any(p => p.Code == permission.Code))
            context.Permissions.Add(permission);

    context.SaveChanges();

    var adminRole = context.Roles.First(r => r.Name == RoleSeed.Admin);

    var assigned = context.RolePermissions
        .Where(rp => rp.RoleId == adminRole.Id)
        .Select(rp => rp.PermissionId)
        .ToHashSet();

    var missing = context.Permissions
        .Where(p => !assigned.Contains(p.Id))
        .Select(p => new RolePermission
        {
            RoleId = adminRole.Id,
            PermissionId = p.Id,
            
        });
    using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await AccountingSeed.SeedAsync(context);
            await CompanyUserSeed.SeedAsync(context);
        }

    context.RolePermissions.AddRange(missing);
    context.SaveChanges();
}

// =====================================================
// Pipeline
// =====================================================
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("DevCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
