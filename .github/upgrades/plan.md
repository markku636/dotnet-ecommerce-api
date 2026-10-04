# .NET 8 → .NET 10 Upgrade Plan
# EC226 E-commerce Platform

**Solution**: `ec-226.sln`  
**Current Target**: .NET 8.0  
**Upgrade Target**: .NET 10.0 (LTS)  
**Branch**: `upgrade-to-NET10`  
**Assessment Date**: 2025-01-XX  
**Total Projects**: 23  
**Total Issues**: 260 (Mandatory: 93, Potential: 157, Optional: 10)

---

## Executive Summary

This plan guides the upgrade of the EC226 e-commerce platform from .NET 8 to .NET 10 (Long Term Support). The solution contains 23 projects including two ASP.NET Core APIs (Member Site and CMS), payment integration, and various supporting libraries.

**Key Statistics**:
- 48 files require changes
- 69 NuGet packages (7 incompatible, 15 require upgrade)
- 2 major technology migrations needed
- Security vulnerabilities to address

**Risk Level**: ⚠️ **Medium** - Multiple API breaking changes, security vulnerabilities, and payment integration dependencies

**Estimated Effort**: 2-3 days (including testing)

---

## Table of Contents

1. [Pre-Upgrade Validation](#1-pre-upgrade-validation)
2. [Package Updates Strategy](#2-package-updates-strategy)
3. [Project Framework Updates](#3-project-framework-updates)
4. [Breaking Changes & Code Fixes](#4-breaking-changes--code-fixes)
5. [Security Vulnerability Fixes](#5-security-vulnerability-fixes)
6. [Docker & Deployment Updates](#6-docker--deployment-updates)
7. [Testing Strategy](#7-testing-strategy)
8. [Rollback Plan](#8-rollback-plan)

---

## 1. Pre-Upgrade Validation

### 1.1 SDK Installation Validation

**Objective**: Ensure .NET 10 SDK is properly installed

**Actions**:
1. Verify .NET 10.0 SDK installation:
   ```powershell
   dotnet --list-sdks
   ```
   Expected: `10.0.100` or higher

2. Check `global.json` compatibility (if exists)
3. Validate IDE/tooling support for .NET 10

**Success Criteria**:
- ✅ .NET 10 SDK detected
- ✅ No SDK version conflicts in global.json
- ✅ Visual Studio supports .NET 10

---

### 1.2 Baseline Testing

**Objective**: Establish current functionality baseline

**Actions**:
1. Run full test suite on .NET 8
   - Unit tests: `EC226.Payment.Test`
   - Integration tests (if applicable)

2. Document current test results:
   - Total tests: ___
   - Passed: ___
   - Failed: ___
   - Skipped: ___

3. Verify critical paths work on .NET 8:
   - Member registration/login
   - Product browsing/cart operations
   - ECPay payment flow
   - Stripe payment flow
   - CMS content management

**Success Criteria**:
- ✅ All tests pass on .NET 8
- ✅ No regressions from previous state
- ✅ Payment flows validated

---

### 1.3 Dependency Analysis

**Objective**: Review project dependencies and identify risks

**Actions**:
1. Document current dependency tree (see Assessment §2.1)
2. Identify projects with payment dependencies:
   - `EC226.Payment` - ECPay, Stripe integrations
   - `EC226.MemberSite.Api` - Payment controllers
3. Flag high-risk packages:
   - `SixLabors.ImageSharp` (security vulnerability)
   - `DataAnnotationsValidator` (incompatible)
   - `Seq.Client.NLog` (incompatible)

**Success Criteria**:
- ✅ All incompatible packages identified
- ✅ Risk mitigation plans documented
- ✅ Payment provider SDK compatibility confirmed

---

## 2. Package Updates Strategy

### 2.1 Incompatible Packages - Immediate Action Required

**These packages MUST be updated or removed**:

#### 2.1.1 Remove Incompatible Package: `DataAnnotationsValidator`

**Current**: 2.2.0  
**Status**: ❌ Incompatible with .NET 10  
**Recommendation**: Remove - functionality exists in .NET 10 built-in validation

**Projects Affected**:
- Check assessment for specific projects

**Action**:
```xml
<!-- REMOVE this from .csproj files -->
<PackageReference Include="DataAnnotationsValidator" Version="2.2.0" />
```

**Migration**:
- .NET 10 includes built-in `DataAnnotations` validation
- Replace custom validators with `IValidatableObject` or FluentValidation
- Review `ValidationAttribute` usage

---

#### 2.1.2 Remove Incompatible Package: `Seq.Client.NLog`

**Current**: 2.3.27  
**Status**: ❌ Incompatible with .NET 10  
**Recommendation**: Use `Seq.Extensions.Logging` (already in project)

**Projects Affected**:
- `EC226.Loging`

**Action**:
```xml
<!-- REMOVE from EC226.Loging.csproj -->
<PackageReference Include="Seq.Client.NLog" Version="2.3.27" />

<!-- KEEP (already compatible) -->
<PackageReference Include="Seq.Extensions.Logging" Version="8.0.0" />
```

**Migration**:
- Migrate from `Seq.Client.NLog` to `Seq.Extensions.Logging`
- Update logging configuration in `NLog.config`

---

#### 2.1.3 Remove/Update: `Microsoft.VisualStudio.Azure.Containers.Tools.Targets`

**Current**: 1.21.0  
**Status**: ❌ Incompatible  
**Recommendation**: Remove or update to latest

**Projects Affected**:
- `EC226.CMS.Api`
- `EC226.MemberSite.Api`

**Action**:
```xml
<!-- Option 1: Remove if not using container tools -->
<!-- <PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.21.0" /> -->

<!-- Option 2: Update to latest (if needed) -->
<PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.22.0" />
```

---

### 2.2 Microsoft Core Packages - Update to 10.0.x

**All Microsoft.* packages should be updated to .NET 10 versions**:

```xml
<!-- ASP.NET Core packages -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.1" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="10.0.1" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation" Version="10.0.1" />

<!-- Entity Framework Core -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.1" />

<!-- Extensions packages -->
<PackageReference Include="Microsoft.Extensions.Caching.Memory" Version="10.0.1" />
<PackageReference Include="Microsoft.Extensions.Configuration" Version="10.0.1" />
<PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="10.0.1" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="10.0.1" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="10.0.1" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="10.0.1" />
<PackageReference Include="Microsoft.Extensions.Hosting.Abstractions" Version="10.0.1" />
<PackageReference Include="Microsoft.Extensions.Options" Version="10.0.2" />

<!-- Code generation -->
<PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="10.0.1" />
```

**Projects Affected**: Most projects (see Assessment for complete list)

---

### 2.3 Security Vulnerability Fixes

#### 2.3.1 UPDATE: `SixLabors.ImageSharp` (CRITICAL)

**Current**: 3.1.4  
**Required**: 3.1.12  
**Severity**: 🔴 **High** - Contains security vulnerability

**Projects Affected**:
- `EC226.MemberSite.Services`
- `EC226.Caching`

**Action**:
```xml
<PackageReference Include="SixLabors.ImageSharp" Version="3.1.12" />
```

**Validation**:
- Test image upload/processing after upgrade
- Verify thumbnail generation works
- Check image resizing in product management

---

### 2.4 Obsolete Packages - Update Recommended

```xml
<!-- Update obsolete packages -->
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="13.0.1" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.4" />

<!-- Remove deprecated packages (replace with .NET 10 built-ins) -->
<!-- Microsoft.AspNetCore.Http.Abstractions 2.2.0 → Remove (built-in) -->
<!-- Microsoft.AspNetCore.Http.Features 5.0.17 → Remove (built-in) -->
<!-- Microsoft.AspNetCore.Mvc.Core 2.2.5 → Remove (built-in) -->
```

---

### 2.5 Package Update Order

**Execute updates in this order to minimize dependency conflicts**:

1. **Phase 1**: Core framework packages
   - Microsoft.Extensions.*
   - Microsoft.AspNetCore.* (non-Mvc)

2. **Phase 2**: Entity Framework
   - Microsoft.EntityFrameworkCore.*

3. **Phase 3**: ASP.NET Core MVC/API
   - Microsoft.AspNetCore.Mvc.*
   - Microsoft.AspNetCore.Authentication.*

4. **Phase 4**: Third-party essential
   - Autofac packages
   - AutoMapper packages

5. **Phase 5**: Security fix
   - SixLabors.ImageSharp

6. **Phase 6**: Remove incompatible
   - DataAnnotationsValidator
   - Seq.Client.NLog
   - Obsolete Microsoft packages

---

## 3. Project Framework Updates

### 3.1 Update Target Framework Moniker (TFM)

**All 23 projects need TFM update**:

#### 3.1.1 Class Library Projects (20 projects)

```xml
<!-- OLD -->
<TargetFramework>net8.0</TargetFramework>

<!-- NEW -->
<TargetFramework>net10.0</TargetFramework>
```

**Projects**:
- EC226.Auth
- EC226.Caching
- EC226.CodeGenerator
- EC226.Configuration
- EC226.Core
- EC226.Filter
- EC226.Loging
- EC226.Mapping
- EC226.MemberSite.IRepository
- EC226.MemberSite.IServices
- EC226.MemberSite.Model
- EC226.MemberSite.Repository
- EC226.MemberSite.Services
- EC226.Middlewares
- EC226.Pagination
- EC226.Payment
- EC226.RedisMQ
- EC226.Swagger
- EC226.Task
- EC226.Utility

---

#### 3.1.2 ASP.NET Core API Projects (2 projects)

```xml
<!-- OLD -->
<TargetFramework>net8.0</TargetFramework>

<!-- NEW -->
<TargetFramework>net10.0</TargetFramework>
```

**Projects**:
- EC226.CMS.Api
- EC226.MemberSite.Api

**Additional Configuration**:
```xml
<PropertyGroup>
  <TargetFramework>net10.0</TargetFramework>
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable> <!-- Consider enabling -->
  <LangVersion>13.0</LangVersion> <!-- C# 13 -->
</PropertyGroup>
```

---

#### 3.1.3 Test Project (1 project)

```xml
<!-- OLD -->
<TargetFramework>net8.0</TargetFramework>

<!-- NEW -->
<TargetFramework>net10.0</TargetFramework>
```

**Project**:
- EC226.Payment.Test

---

### 3.2 Update Order by Dependency Hierarchy

**Update projects in topological order** (dependencies first):

**Tier 1** (No dependencies on other EC226 projects):
1. EC226.Configuration
2. EC226.Pagination
3. EC226.Loging

**Tier 2** (Depends on Tier 1):
4. EC226.Utility
5. EC226.Caching
6. EC226.Core
7. EC226.Auth

**Tier 3** (Business logic):
8. EC226.MemberSite.Model
9. EC226.MemberSite.IRepository
10. EC226.MemberSite.IServices
11. EC226.Payment
12. EC226.Swagger
13. EC226.RedisMQ

**Tier 4** (Implementation):
14. EC226.MemberSite.Repository
15. EC226.MemberSite.Services
16. EC226.CodeGenerator
17. EC226.Filter
18. EC226.Mapping
19. EC226.Task
20. EC226.Middlewares

**Tier 5** (Application):
21. EC226.CMS.Api
22. EC226.MemberSite.Api
23. EC226.Payment.Test

---

## 4. Breaking Changes & Code Fixes

### 4.1 Legacy Cryptography APIs (10 issues)

**Problem**: Legacy cryptography types marked obsolete in .NET 8, removed in .NET 10

**Affected Projects**:
- `EC226.Payment` - Payment security/hashing
- `EC226.Utility` - Encryption utilities
- `EC226.Caching` - Cache key generation

**Breaking Changes**:
- `System.Security.Cryptography.SHA1` → Use `SHA256` or higher
- `System.Security.Cryptography.MD5` → Use `SHA256` or higher
- `HMACSHA1` → Use `HMACSHA256` or higher

---

#### 4.1.1 Fix: Update CheckMac.cs (EC226.Payment)

**File**: `EC226.Payment\CheckMac.cs`

**OLD CODE** (using SHA256 - should still work, verify):
```csharp
using System.Security.Cryptography;

public class CheckMac
{
   public static string GetCheckMacValue(string data, string hashKey, string hashIV)
   {
      // If using SHA256, this is fine
      var sha256 = SHA256.Create();
      // ...
   }
}
```

**VERIFY**: Check if any MD5 or SHA1 usage exists. If yes, update to SHA256:

```csharp
// ❌ BAD - Obsolete
var hash = MD5.Create();

// ✅ GOOD - .NET 10 compatible
var hash = SHA256.Create();
```

**ECPay Compatibility Note**:
- ECPay might require specific hash algorithms
- Check ECPay documentation for .NET 10 support
- Test payment flows after changing hash algorithms

---

#### 4.1.2 Fix: Encryption Utilities (EC226.Utility)

**Files to Review**:
- Search for: `MD5`, `SHA1`, `HMACSHA1`, `TripleDES`

**Migration Pattern**:
```csharp
// ❌ OLD - Obsolete in .NET 10
using (var md5 = MD5.Create())
{
   byte[] hash = md5.ComputeHash(data);
}

// ✅ NEW - .NET 10 compatible
using (var sha256 = SHA256.Create())
{
   byte[] hash = sha256.ComputeHash(data);
}
```

**AES Encryption (Recommended)**:
```csharp
// For symmetric encryption, use AES
using (var aes = Aes.Create())
{
   aes.Key = key;
   aes.IV = iv;
   // ...encryption logic...
}
```

---

### 4.2 IdentityModel & Claims-based Security (61 issues)

**Problem**: Breaking changes in identity and claims APIs

**Affected Projects**:
- `EC226.Auth` - JWT authentication
- `EC226.MemberSite.Api` - Authentication controllers
- `EC226.CMS.Api` - Authentication controllers

**Breaking Changes**:
1. `ClaimsPrincipal.Current` removed
2. `Thread.CurrentPrincipal` deprecated
3. JWT handler configuration changes

---

#### 4.2.1 Fix: Replace ClaimsPrincipal.Current

**OLD CODE**:
```csharp
// ❌ This no longer works in .NET 10
var identity = ClaimsPrincipal.Current?.Identity as ClaimsIdentity;
var userId = ClaimsPrincipal.Current?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
```

**NEW CODE**:
```csharp
// ✅ Inject HttpContext via dependency injection
public class SomeService
{
   private readonly IHttpContextAccessor _httpContextAccessor;

   public SomeService(IHttpContextAccessor httpContextAccessor)
   {
      _httpContextAccessor = httpContextAccessor;
   }

   public void SomeMethod()
   {
      var user = _httpContextAccessor.HttpContext?.User;
      var identity = user?.Identity as ClaimsIdentity;
      var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
   }
}
```

**Registration Required** (in `Program.cs` or `Startup.cs`):
```csharp
builder.Services.AddHttpContextAccessor();
```

---

#### 4.2.2 Fix: JWT Authentication Configuration

**File**: `EC226.MemberSite.Api\Program.cs` or `Startup.cs`

**OLD CODE** (.NET 8):
```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
      options.TokenValidationParameters = new TokenValidationParameters
      {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = Configuration["Jwt:Issuer"],
         ValidAudience = Configuration["Jwt:Audience"],
         IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
      };
   });
```

**NEW CODE** (.NET 10 - same structure, but validate parameters):
```csharp
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
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] 
               ?? throw new InvalidOperationException("JWT key not configured")))
      };

      // Add event handlers for debugging
      options.Events = new JwtBearerEvents
      {
         OnAuthenticationFailed = context =>
         {
            // Log authentication failures
            return Task.CompletedTask;
         }
      };
   });
```

---

#### 4.2.3 Fix: BaseController Claims Access

**File**: `EC226.MemberSite.Api\Controllers\BaseController.cs`

**Review Current Implementation**:
```csharp
public class BaseController : ControllerBase
{
   // ❌ If using ClaimsPrincipal.Current, replace with:
   
   // ✅ Use HttpContext.User instead
   protected string GetCurrentUserId()
   {
      return User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
         ?? throw new UnauthorizedAccessException("User not authenticated");
   }

   protected string GetCurrentUserName()
   {
      return User?.Identity?.Name 
         ?? throw new UnauthorizedAccessException("User not authenticated");
   }

   protected bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
}
```

---

### 4.3 ASP.NET Core API Changes

#### 4.3.1 Review Program.cs / Startup.cs

**Both API Projects**:
- `EC226.MemberSite.Api\Program.cs`
- `EC226.CMS.Api\Program.cs`

**Common .NET 10 Pattern**:
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
   .AddNewtonsoftJson(options =>
   {
      options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
   });

builder.Services.AddRazorPages(); // If using Razor Pages

// Autofac integration
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
   containerBuilder.RegisterModule(new AutofacModuleRegister());
});

// Authentication & Authorization
builder.Services.AddHttpContextAccessor(); // IMPORTANT for claims access
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(/* config */);

builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
   app.UseDeveloperExceptionPage();
}
else
{
   app.UseExceptionHandler("/Error");
   app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // MUST come before UseAuthorization
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages(); // If using Razor Pages

app.Run();
```

---

#### 4.3.2 Autofac Integration Validation

**File**: `EC226.MemberSite.Api\AutoFac\AutofacModuleRegister.cs`

**Verify Autofac 9.0 Compatibility**:
```csharp
public class AutofacModuleRegister : Module
{
   protected override void Load(ContainerBuilder builder)
   {
      // ✅ Ensure this pattern still works with Autofac 9.0

      // Register services
      builder.RegisterType<PaymentAbstractService>()
         .As<IThirdPaymentService>()
         .InstancePerLifetimeScope();

      // Register repositories
      builder.RegisterGeneric(typeof(BaseRepository<>))
         .As(typeof(IBaseRepository<>))
         .InstancePerLifetimeScope();

      // Scan assemblies
      builder.RegisterAssemblyTypes(typeof(CategoryService).Assembly)
         .Where(t => t.Name.EndsWith("Service"))
         .AsImplementedInterfaces()
         .InstancePerLifetimeScope();
   }
}
```

**Test After Upgrade**:
- Verify dependency injection works
- Check service lifetimes are correct
- Ensure no circular dependencies

---

### 4.4 Entity Framework Core Updates

**Version**: 7.0.3 → 10.0.1

**Projects Affected**:
- `EC226.MemberSite.Repository`
- Any projects using DbContext

#### 4.4.1 DbContext Configuration

**Review and update if needed**:
```csharp
public class ApplicationDbContext : DbContext
{
   public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
   {
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      // ✅ .NET 10 - Take advantage of new features

      // Complex types (new in EF Core 8+)
      modelBuilder.Entity<Order>()
         .ComplexProperty(o => o.ShippingAddress);

      // Primitive collections (new in EF Core 8+)
      modelBuilder.Entity<Product>()
         .PrimitiveCollection(p => p.Tags);

      // JSON columns (improved in EF Core 8+)
      modelBuilder.Entity<Order>()
         .OwnsOne(o => o.Metadata, navigationBuilder =>
         {
            navigationBuilder.ToJson();
         });
   }
}
```

---

#### 4.4.2 Migration Generation

**After upgrade, generate new migration**:
```powershell
# In Package Manager Console
Add-Migration UpgradeToNet10 -Project EC226.MemberSite.Repository

# Review generated migration
# If no schema changes, migration will be empty (which is fine)

# Apply migration
Update-Database -Project EC226.MemberSite.Repository
```

---

## 5. Security Vulnerability Fixes

### 5.1 SixLabors.ImageSharp Vulnerability

**CVE**: [Check NuGet advisory]  
**Severity**: 🔴 High  
**Current**: 3.1.4  
**Fixed**: 3.1.12

**Projects**:
- EC226.MemberSite.Services
- EC226.Caching

**Update**:
```xml
<PackageReference Include="SixLabors.ImageSharp" Version="3.1.12" />
```

**Testing Required**:
1. Product image upload
2. Image thumbnail generation
3. Image resizing/cropping
4. Avatar upload (if applicable)

---

### 5.2 Review Other Security Advisories

**Check for new advisories**:
```powershell
dotnet list package --vulnerable
```

**Document findings and remediation**.

---

## 6. Docker & Deployment Updates

### 6.1 Update Dockerfiles

#### 6.1.1 EC226.MemberSite.Api\Dockerfile

**OLD**:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EC226.MemberSite.Api/EC226.MemberSite.Api.csproj", "EC226.MemberSite.Api/"]
# ... rest of COPY statements ...

RUN dotnet restore "./EC226.MemberSite.Api/EC226.MemberSite.Api.csproj"
COPY . .
WORKDIR "/src/EC226.MemberSite.Api"
RUN dotnet build "./EC226.MemberSite.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EC226.MemberSite.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EC226.MemberSite.Api.dll"]
```

**NEW** (Update base images):
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files (same as before)
COPY ["EC226.MemberSite.Api/EC226.MemberSite.Api.csproj", "EC226.MemberSite.Api/"]
COPY ["EC226.Auth/EC226.Auth.csproj", "EC226.Auth/"]
COPY ["EC226.Caching/EC226.Caching.csproj", "EC226.Caching/"]
COPY ["EC226.Configuration/EC226.Configuration.csproj", "EC226.Configuration/"]
COPY ["EC226.Core/EC226.Core.csproj", "EC226.Core/"]
COPY ["EC226.Filter/EC226.Filter.csproj", "EC226.Filter/"]
COPY ["EC226.Loging/EC226.Loging.csproj", "EC226.Loging/"]
COPY ["EC226.Mapping/EC226.Mapping.csproj", "EC226.Mapping/"]
COPY ["EC226.MemberSite.IRepository/EC226.MemberSite.IRepository.csproj", "EC226.MemberSite.IRepository/"]
COPY ["EC226.MemberSite.IServices/EC226.MemberSite.IServices.csproj", "EC226.MemberSite.IServices/"]
COPY ["EC226.MemberSite.Model/EC226.MemberSite.Model.csproj", "EC226.MemberSite.Model/"]
COPY ["EC226.MemberSite.Repository/EC226.MemberSite.Repository.csproj", "EC226.MemberSite.Repository/"]
COPY ["EC226.MemberSite.Services/EC226.MemberSite.Services.csproj", "EC226.MemberSite.Services/"]
COPY ["EC226.Middlewares/EC226.Middlewares.csproj", "EC226.Middlewares/"]
COPY ["EC226.Pagination/EC226.Pagination.csproj", "EC226.Pagination/"]
COPY ["EC226.Payment/EC226.Payment.csproj", "EC226.Payment/"]
COPY ["EC226.RedisMQ/EC226.RedisMQ.csproj", "EC226.RedisMQ/"]
COPY ["EC226.Swagger/EC226.Swagger.csproj", "EC226.Swagger/"]
COPY ["EC226.Task/EC226.Task.csproj", "EC226.Task/"]
COPY ["EC226.Utility/EC226.Utility.csproj", "EC226.Utility/"]

RUN dotnet restore "./EC226.MemberSite.Api/EC226.MemberSite.Api.csproj"
COPY . .
WORKDIR "/src/EC226.MemberSite.Api"
RUN dotnet build "./EC226.MemberSite.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EC226.MemberSite.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EC226.MemberSite.Api.dll"]
```

---

#### 6.1.2 EC226.CMS.Api\Dockerfile

**Same pattern - update base images**:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
# ...

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
# ...
```

---

### 6.2 Docker Compose Updates (if applicable)

**If using `docker-compose.yml`**:
```yaml
version: '3.8'

services:
  membersite-api:
    image: ec226-membersite-api:net10
    build:
      context: .
      dockerfile: EC226.MemberSite.Api/Dockerfile
    ports:
      - "5000:8080"
      - "5001:8081"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:8080;https://+:8081
    depends_on:
      - sqlserver
      - redis

  cms-api:
    image: ec226-cms-api:net10
    build:
      context: .
      dockerfile: EC226.CMS.Api/Dockerfile
    ports:
      - "5002:8080"
      - "5003:8081"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://+:8080;https://+:8081
    depends_on:
      - sqlserver
      - redis

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    # ...existing config...

  redis:
    image: redis:7-alpine
    # ...existing config...
```

---

### 6.3 CI/CD Pipeline Updates

**If using GitHub Actions / Azure DevOps**:

**GitHub Actions** (`.github/workflows/build.yml`):
```yaml
name: Build and Test

on:
  push:
    branches: [ main, upgrade-to-NET10 ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET 10
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'
    
    - name: Restore dependencies
      run: dotnet restore ec-226.sln
    
    - name: Build
      run: dotnet build ec-226.sln --no-restore --configuration Release
    
    - name: Test
      run: dotnet test ec-226.sln --no-build --verbosity normal --configuration Release
```

---

## 7. Testing Strategy

### 7.1 Unit Testing

**Project**: `EC226.Payment.Test`

**Actions**:
1. Run existing unit tests:
   ```powershell
   dotnet test EC226.Payment.Test\EC226.Payment.Test.csproj
   ```

2. Expected results:
   - All existing tests should pass
   - If tests fail, investigate breaking changes

3. Review test files:
   - `FluentEcpayTest_Payment.cs` - ECPay integration tests
   - Verify test data is still valid
   - Check mock configurations

**Success Criteria**:
- ✅ 100% of existing tests pass
- ✅ No new test failures introduced

---

### 7.2 Integration Testing

#### 7.2.1 Payment Flow Testing

**Critical Paths**:

1. **ECPay Integration**:
   - Generate payment URL
   - Process payment callback
   - Verify CheckMac validation
   - Test refund flow (if supported)

2. **Stripe Integration**:
   - Create payment intent
   - Process webhook events
   - Handle payment success/failure
   - Test subscription flows (if applicable)

**Test Environment**:
- Use ECPay test environment credentials
- Use Stripe test mode API keys
- Document test transactions

---

#### 7.2.2 API Endpoint Testing

**Member Site API** (`EC226.MemberSite.Api`):
- [ ] POST /api/Payment/CreateOrder - Create order
- [ ] POST /api/Payment/ECPayCallback - ECPay callback
- [ ] POST /api/Payment/StripeWebhook - Stripe webhook
- [ ] GET /api/Cart - Get cart items
- [ ] POST /api/Cart/AddItem - Add to cart
- [ ] POST /api/Cart/Checkout - Checkout
- [ ] GET /api/Category - Get categories
- [ ] GET /api/Article - Get articles
- [ ] POST /api/Auth/Login - User login
- [ ] POST /api/Auth/Register - User registration

**CMS API** (`EC226.CMS.Api`):
- [ ] GET /api/Content - Get content list
- [ ] POST /api/Content - Create content
- [ ] PUT /api/Content/{id} - Update content
- [ ] DELETE /api/Content/{id} - Delete content
- [ ] POST /api/Image/Upload - Upload image
- [ ] GET /api/Schedule - Get schedules
- [ ] GET /api/Collection - Get collections
- [ ] POST /api/CodeTable - Code table operations

---

#### 7.2.3 Authentication Testing

**Test Scenarios**:
1. JWT token generation
2. Token validation
3. Claims extraction
4. Role-based authorization
5. Token refresh (if implemented)

**Validation**:
```powershell
# Test JWT authentication
curl -X POST https://localhost:5001/api/Auth/Login \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"test123"}'

# Use token in subsequent requests
curl -X GET https://localhost:5001/api/Cart \
  -H "Authorization: Bearer {token}"
```

---

### 7.3 Performance Testing

**Baseline Comparison**:
1. Measure response times on .NET 8
2. Measure response times on .NET 10
3. Compare results

**Key Metrics**:
- API response time (p50, p95, p99)
- Database query performance
- Memory usage
- CPU usage

**Expected**: .NET 10 should show equal or better performance

---

### 7.4 Manual Testing Checklist

**E-commerce Flow**:
- [ ] Browse products
- [ ] Add products to cart
- [ ] Update cart quantities
- [ ] Remove from cart
- [ ] Proceed to checkout
- [ ] Select ECPay payment
- [ ] Complete payment on ECPay test site
- [ ] Verify order confirmation
- [ ] Check order status in CMS

**CMS Flow**:
- [ ] Login to CMS
- [ ] Create new content
- [ ] Upload images
- [ ] Edit existing content
- [ ] Publish content
- [ ] View content on member site

---

## 8. Rollback Plan

### 8.1 Git Rollback

**If major issues occur**:
```powershell
# Rollback to main branch
git checkout main

# Delete upgrade branch (if needed)
git branch -D upgrade-to-NET10

# Redeploy .NET 8 version
```

---

### 8.2 Docker Rollback

**Revert to .NET 8 images**:
```powershell
# Use previous Docker images
docker pull ec226-membersite-api:net8
docker pull ec226-cms-api:net8

# Restart services
docker-compose up -d
```

---

### 8.3 Database Rollback

**If EF migrations were applied**:
```powershell
# Rollback migration
Update-Database -Migration <PreviousMigrationName> -Project EC226.MemberSite.Repository
```

**Note**: If no schema changes, no rollback needed.

---

## 9. Post-Upgrade Validation

### 9.1 Smoke Tests

**After deployment to each environment**:
- [ ] Application starts without errors
- [ ] Health check endpoint responds
- [ ] Database connectivity works
- [ ] Redis connectivity works
- [ ] Logging works (check Seq/NLog)

---

### 9.2 Monitoring

**Watch for**:
- Error rates in logs
- Response time degradation
- Memory leaks
- Authentication failures
- Payment processing errors

**Tools**:
- Seq logs
- Application Insights (if configured)
- Server monitoring

---

### 9.3 User Acceptance Testing (UAT)

**Coordinate with stakeholders**:
- Schedule UAT session
- Provide test environment
- Document any issues
- Fix critical issues before production

---

## 10. Production Deployment

### 10.1 Deployment Checklist

**Pre-Deployment**:
- [ ] All tests pass
- [ ] Code review completed
- [ ] UAT sign-off received
- [ ] Rollback plan documented
- [ ] Database backup created
- [ ] Maintenance window scheduled

**Deployment Steps**:
1. Enable maintenance mode
2. Backup database
3. Deploy .NET 10 binaries
4. Run smoke tests
5. Disable maintenance mode
6. Monitor for 1 hour

**Post-Deployment**:
- [ ] Verify critical functionality
- [ ] Check error logs
- [ ] Monitor performance
- [ ] Notify stakeholders

---

## Appendix A: Project Dependency Graph

```
Level 0 (No dependencies):
  - EC226.Configuration
  - EC226.Pagination
  - EC226.Loging

Level 1 (Depends on Level 0):
  - EC226.Utility
  - EC226.Caching
  - EC226.Core
  - EC226.Auth

Level 2 (Depends on Level 0-1):
  - EC226.MemberSite.Model
  - EC226.MemberSite.IRepository
  - EC226.MemberSite.IServices
  - EC226.Payment
  - EC226.Swagger
  - EC226.RedisMQ
  - EC226.CodeGenerator

Level 3 (Depends on Level 0-2):
  - EC226.MemberSite.Repository
  - EC226.MemberSite.Services
  - EC226.Filter
  - EC226.Mapping
  - EC226.Task
  - EC226.Middlewares

Level 4 (Top level applications):
  - EC226.CMS.Api
  - EC226.MemberSite.Api
  - EC226.Payment.Test
```

---

## Appendix B: Package Version Matrix

| Package | .NET 8 Version | .NET 10 Version | Status |
|---------|----------------|-----------------|--------|
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.4 | 10.0.1 | ✅ Update |
| Microsoft.EntityFrameworkCore | 7.0.3 | 10.0.1 | ✅ Update |
| SixLabors.ImageSharp | 3.1.4 | 3.1.12 | 🔴 Security Fix |
| Autofac.Extensions.DependencyInjection | 9.0.0 | 9.0.0 | ✅ Compatible |
| Newtonsoft.Json | 13.0.3 | 13.0.4 | ✅ Update |
| DataAnnotationsValidator | 2.2.0 | N/A | ❌ Remove |
| Seq.Client.NLog | 2.3.27 | N/A | ❌ Remove |

---

## Appendix C: Breaking Changes Reference

**Official Documentation**:
- [.NET 9 Breaking Changes](https://learn.microsoft.com/en-us/dotnet/core/compatibility/9.0)
- [.NET 10 Breaking Changes](https://learn.microsoft.com/en-us/dotnet/core/compatibility/10.0)
- [ASP.NET Core 10 Migration](https://learn.microsoft.com/en-us/aspnet/core/migration/90-to-10)

---

## Appendix D: Contact & Support

**Internal Team**:
- Development Lead: _______________
- DevOps: _______________
- QA: _______________

**External Support**:
- ECPay Support: [ECPay contact]
- Stripe Support: [Stripe contact]

---

**End of Plan**

---

**Next Steps**: Proceed to task generation and execution.
