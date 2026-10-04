# EC226 E-commerce Platform .NET 10 Upgrade Tasks

## Overview

This document tracks the execution of the EC226 e-commerce platform upgrade from .NET 8 to .NET 10 LTS. The upgrade follows a bottom-up strategy, upgrading 23 projects across 5 dependency tiers, addressing 260 compatibility issues including security vulnerabilities and payment integration dependencies.

**Progress**: 5/17 tasks complete (29%) ![0%](https://progress-bar.xyz/29)

---

## Tasks

### [✓] TASK-001: Verify prerequisites and environment setup *(Completed: 2026-01-10 16:10)*
**References**: Plan §1.1

- [✓] (1) Verify .NET 10.0 SDK installation (minimum version 10.0.100)
- [✓] (2) .NET 10 SDK detected and meets minimum requirements (**Verify**)
- [✓] (3) Check global.json compatibility if present in repository
- [✓] (4) Global.json compatible with .NET 10 or updated accordingly (**Verify**)
- [✓] (5) Validate Visual Studio or development tooling supports .NET 10
- [✓] (6) Development environment supports .NET 10 (**Verify**)

---

### [✓] TASK-002: Establish .NET 8 baseline *(Completed: 2026-01-10 16:11)*
**References**: Plan §1.2

- [✓] (1) Run full test suite on current .NET 8 version (EC226.Payment.Test project)
- [✓] (2) All baseline tests pass with 0 failures (**Verify**)
- [✓] (3) Document current test metrics (total tests, pass/fail/skip counts)
- [✓] (4) Test metrics documented for comparison (**Verify**)

---

### [✓] TASK-003: Upgrade Tier 1 foundation projects *(Completed: 2026-01-10 16:27)*
**References**: Plan §3.2 Tier 1, Plan §2.1, Plan §2.2

- [✓] (1) Update TargetFramework to net10.0 in projects: EC226.Configuration, EC226.Pagination, EC226.Loging
- [✓] (2) All Tier 1 project files updated to net10.0 (**Verify**)
- [✓] (3) Remove incompatible package Seq.Client.NLog from EC226.Loging per Plan §2.1.2
- [✓] (4) Seq.Client.NLog removed, Seq.Extensions.Logging retained (**Verify**)
- [✓] (5) Update Microsoft.Extensions.* packages to 10.0.x versions per Plan §2.2
- [✓] (6) Restore dependencies for Tier 1 projects
- [✓] (7) All Tier 1 dependencies restored successfully (**Verify**)
- [✓] (8) Build Tier 1 projects and resolve any compilation errors
- [✓] (9) All Tier 1 projects build with 0 errors (**Verify**)
- [✓] (10) Commit changes with message: "TASK-003: Upgrade Tier 1 foundation projects to .NET 10"

---

### [✓] TASK-004: Upgrade Tier 2 core libraries *(Completed: 2026-01-10 16:32)*
**References**: Plan §3.2 Tier 2, Plan §2.2, Plan §4.1

- [✓] (1) Update TargetFramework to net10.0 in projects: EC226.Utility, EC226.Caching, EC226.Core, EC226.Auth
- [✓] (2) All Tier 2 project files updated to net10.0 (**Verify**)
- [✓] (3) Update Microsoft.Extensions.* packages to 10.0.x versions per Plan §2.2
- [✓] (4) Update Microsoft.AspNetCore.* packages to 10.0.x versions per Plan §2.2
- [✓] (5) Restore dependencies for Tier 2 projects
- [✓] (6) All Tier 2 dependencies restored successfully (**Verify**)
- [✓] (7) Review and update legacy cryptography usage in EC226.Utility per Plan §4.1.2 (replace MD5/SHA1 with SHA256)
- [✓] (8) Build Tier 2 projects and resolve compilation errors
- [✓] (9) All Tier 2 projects build with 0 errors (**Verify**)
- [✓] (10) Commit changes with message: "TASK-004: Upgrade Tier 2 core libraries to .NET 10"

---

### [✓] TASK-005: Upgrade Tier 3 business logic layer *(Completed: 2026-01-10 16:45)*
**References**: Plan §3.2 Tier 3, Plan §2.1.1, Plan §2.2, Plan §4.1.1

- [✓] (1) Update TargetFramework to net10.0 in projects: EC226.MemberSite.Model, EC226.MemberSite.IRepository, EC226.MemberSite.IServices, EC226.Payment, EC226.Swagger, EC226.RedisMQ
- [✓] (2) All Tier 3 project files updated to net10.0 (**Verify**)
- [✓] (3) Remove DataAnnotationsValidator package per Plan §2.1.1 (functionality built into .NET 10)
- [✓] (4) DataAnnotationsValidator removed from all Tier 3 projects (**Verify**)
- [✓] (5) Update Microsoft.Extensions.* packages to 10.0.x versions per Plan §2.2
- [✓] (6) Restore dependencies for Tier 3 projects
- [✓] (7) All Tier 3 dependencies restored successfully (**Verify**)
- [✓] (8) Review and update CheckMac.cs in EC226.Payment for cryptography usage per Plan §4.1.1
- [✓] (9) Build Tier 3 projects and resolve compilation errors
- [✓] (10) All Tier 3 projects build with 0 errors (**Verify**)
- [✓] (11) Commit changes with message: "TASK-005: Upgrade Tier 3 business logic layer to .NET 10"

---

### [▶] TASK-006: Upgrade Tier 4 implementation layer
**References**: Plan §3.2 Tier 4, Plan §2.2, Plan §2.3.1, Plan §2.4

- [▶] (1) Update TargetFramework to net10.0 in projects: EC226.MemberSite.Repository, EC226.MemberSite.Services, EC226.CodeGenerator, EC226.Filter, EC226.Mapping, EC226.Task, EC226.Middlewares
- [ ] (2) All Tier 4 project files updated to net10.0 (**Verify**)
- [ ] (3) Update Microsoft.EntityFrameworkCore.* packages to 10.0.1 per Plan §2.2
- [ ] (4) Update SixLabors.ImageSharp to 3.1.12 in EC226.MemberSite.Services and EC226.Caching per Plan §2.3.1 (security fix)
- [ ] (5) Update AutoMapper and obsolete packages per Plan §2.4 (AutoMapper 13.0.1, Newtonsoft.Json 13.0.4)
- [ ] (6) Restore dependencies for Tier 4 projects
- [ ] (7) All Tier 4 dependencies restored successfully (**Verify**)
- [ ] (8) Build Tier 4 projects and resolve compilation errors
- [ ] (9) All Tier 4 projects build with 0 errors (**Verify**)
- [ ] (10) Commit changes with message: "TASK-006: Upgrade Tier 4 implementation layer to .NET 10"

---

### [ ] TASK-007: Test Tier 4 image processing after ImageSharp security update
**References**: Plan §2.3.1, Plan §7.2.2

- [ ] (1) Test image upload functionality in affected projects
- [ ] (2) Test thumbnail generation functionality
- [ ] (3) Test image resizing and cropping operations
- [ ] (4) All image processing operations work correctly (**Verify**)
- [ ] (5) Commit any test-related fixes with message: "TASK-007: Validate image processing after ImageSharp update"

---

### [ ] TASK-008: Upgrade Tier 5 API applications - MemberSite.Api
**References**: Plan §3.2 Tier 5, Plan §2.1.3, Plan §2.2, Plan §4.2, Plan §4.3

- [ ] (1) Update TargetFramework to net10.0 in EC226.MemberSite.Api project
- [ ] (2) EC226.MemberSite.Api project file updated to net10.0 (**Verify**)
- [ ] (3) Update or remove Microsoft.VisualStudio.Azure.Containers.Tools.Targets per Plan §2.1.3
- [ ] (4) Update all Microsoft.AspNetCore.* packages to 10.0.x versions per Plan §2.2
- [ ] (5) Update Autofac.Extensions.DependencyInjection if needed (verify 9.0.0 compatibility)
- [ ] (6) Restore dependencies for EC226.MemberSite.Api
- [ ] (7) All dependencies restored successfully (**Verify**)
- [ ] (8) Review and update Program.cs/Startup.cs per Plan §4.3.1 (ensure AddHttpContextAccessor registration)
- [ ] (9) Update JWT authentication configuration per Plan §4.2.2 if needed
- [ ] (10) Review BaseController for ClaimsPrincipal usage per Plan §4.2.3 (replace ClaimsPrincipal.Current with HttpContext.User)
- [ ] (11) Verify Autofac integration per Plan §4.3.2
- [ ] (12) Build EC226.MemberSite.Api and resolve compilation errors
- [ ] (13) EC226.MemberSite.Api builds with 0 errors (**Verify**)
- [ ] (14) Commit changes with message: "TASK-008: Upgrade MemberSite.Api to .NET 10"

---

### [ ] TASK-009: Upgrade Tier 5 API applications - CMS.Api
**References**: Plan §3.2 Tier 5, Plan §2.1.3, Plan §2.2, Plan §4.2, Plan §4.3

- [ ] (1) Update TargetFramework to net10.0 in EC226.CMS.Api project
- [ ] (2) EC226.CMS.Api project file updated to net10.0 (**Verify**)
- [ ] (3) Update or remove Microsoft.VisualStudio.Azure.Containers.Tools.Targets per Plan §2.1.3
- [ ] (4) Update all Microsoft.AspNetCore.* packages to 10.0.x versions per Plan §2.2
- [ ] (5) Update Autofac.Extensions.DependencyInjection if needed (verify 9.0.0 compatibility)
- [ ] (6) Restore dependencies for EC226.CMS.Api
- [ ] (7) All dependencies restored successfully (**Verify**)
- [ ] (8) Review and update Program.cs/Startup.cs per Plan §4.3.1 (ensure AddHttpContextAccessor registration)
- [ ] (9) Update JWT authentication configuration per Plan §4.2.2 if needed
- [ ] (10) Review BaseController for ClaimsPrincipal usage per Plan §4.2.3 (replace ClaimsPrincipal.Current with HttpContext.User)
- [ ] (11) Verify Autofac integration per Plan §4.3.2
- [ ] (12) Build EC226.CMS.Api and resolve compilation errors
- [ ] (13) EC226.CMS.Api builds with 0 errors (**Verify**)
- [ ] (14) Commit changes with message: "TASK-009: Upgrade CMS.Api to .NET 10"

---

### [ ] TASK-010: Upgrade test project and update Entity Framework
**References**: Plan §3.2 Tier 5, Plan §4.4

- [ ] (1) Update TargetFramework to net10.0 in EC226.Payment.Test project
- [ ] (2) EC226.Payment.Test project file updated to net10.0 (**Verify**)
- [ ] (3) Update test framework packages to .NET 10 compatible versions
- [ ] (4) Restore dependencies for EC226.Payment.Test
- [ ] (5) All test dependencies restored successfully (**Verify**)
- [ ] (6) Review DbContext configurations in EC226.MemberSite.Repository per Plan §4.4.1
- [ ] (7) Build EC226.Payment.Test and resolve compilation errors
- [ ] (8) Test project builds with 0 errors (**Verify**)
- [ ] (9) Generate EF Core migration if schema changes exist per Plan §4.4.2
- [ ] (10) Migration generated or confirmed no changes needed (**Verify**)
- [ ] (11) Commit changes with message: "TASK-010: Upgrade test project and update Entity Framework"

---

### [ ] TASK-011: Build entire solution and resolve remaining compilation errors
**References**: Plan §4

- [ ] (1) Build complete ec-226.sln solution
- [ ] (2) Resolve any remaining compilation errors referencing Plan §4 Breaking Changes sections
- [ ] (3) Address any DataAnnotations migration issues per Plan §2.1.1
- [ ] (4) Address any logging configuration issues per Plan §2.1.2
- [ ] (5) Rebuild solution after fixes
- [ ] (6) Complete solution builds with 0 errors (**Verify**)
- [ ] (7) Commit compilation fixes with message: "TASK-011: Resolve final compilation errors"

---

### [ ] TASK-012: Run payment integration unit tests
**References**: Plan §7.1

- [ ] (1) Run all unit tests in EC226.Payment.Test project
- [ ] (2) Fix any test failures related to .NET 10 behavioral changes
- [ ] (3) Re-run tests after fixes
- [ ] (4) All payment unit tests pass with 0 failures (**Verify**)
- [ ] (5) Commit test fixes with message: "TASK-012: Fix payment unit tests for .NET 10"

---

### [ ] TASK-013: Test payment integration flows
**References**: Plan §7.2.1

- [ ] (1) Test ECPay payment URL generation
- [ ] (2) Test ECPay CheckMac validation with updated cryptography
- [ ] (3) Test Stripe payment intent creation
- [ ] (4) Verify payment callbacks and webhooks function correctly
- [ ] (5) All payment integration flows work correctly (**Verify**)
- [ ] (6) Commit any integration fixes with message: "TASK-013: Validate payment integrations"

---

### [ ] TASK-014: Test API endpoints and authentication
**References**: Plan §7.2.2, Plan §7.2.3

- [ ] (1) Test MemberSite.Api critical endpoints per Plan §7.2.2 (payment, cart, auth, category, article)
- [ ] (2) Test CMS.Api critical endpoints per Plan §7.2.2 (content CRUD, image upload, schedules, collections)
- [ ] (3) Test JWT authentication flows per Plan §7.2.3 (token generation, validation, claims extraction)
- [ ] (4) Fix any endpoint or authentication issues found
- [ ] (5) Re-test after fixes
- [ ] (6) All critical API endpoints and authentication flows work correctly (**Verify**)
- [ ] (7) Commit API fixes with message: "TASK-014: Validate API endpoints and authentication"

---

### [ ] TASK-015: Update Docker configurations
**References**: Plan §6.1, Plan §6.2

- [ ] (1) Update base image to mcr.microsoft.com/dotnet/aspnet:10.0 in EC226.MemberSite.Api/Dockerfile per Plan §6.1.1
- [ ] (2) Update SDK image to mcr.microsoft.com/dotnet/sdk:10.0 in EC226.MemberSite.Api/Dockerfile
- [ ] (3) Update base image to mcr.microsoft.com/dotnet/aspnet:10.0 in EC226.CMS.Api/Dockerfile per Plan §6.1.2
- [ ] (4) Update SDK image to mcr.microsoft.com/dotnet/sdk:10.0 in EC226.CMS.Api/Dockerfile
- [ ] (5) Update docker-compose.yml image references if applicable per Plan §6.2
- [ ] (6) Build Docker images to verify Dockerfiles work correctly
- [ ] (7) Docker images build successfully (**Verify**)
- [ ] (8) Commit Docker updates with message: "TASK-015: Update Docker configurations for .NET 10"

---

### [ ] TASK-016: Update CI/CD pipeline configurations
**References**: Plan §6.3

- [ ] (1) Update GitHub Actions workflow to use .NET 10 SDK per Plan §6.3 (actions/setup-dotnet@v4 with dotnet-version: '10.0.x')
- [ ] (2) Update any Azure DevOps pipeline configurations if present
- [ ] (3) Verify pipeline configurations are valid
- [ ] (4) CI/CD pipelines configured correctly for .NET 10 (**Verify**)
- [ ] (5) Commit CI/CD updates with message: "TASK-016: Update CI/CD pipelines for .NET 10"

---

### [ ] TASK-017: Final validation and upgrade completion
**References**: Plan §7, Plan §9

- [ ] (1) Run complete test suite one final time
- [ ] (2) All tests pass with 0 failures (**Verify**)
- [ ] (3) Verify security vulnerability fixes applied (SixLabors.ImageSharp 3.1.12)
- [ ] (4) Security vulnerabilities resolved (**Verify**)
- [ ] (5) Check application health and connectivity (database, Redis, logging)
- [ ] (6) All application health checks pass (**Verify**)
- [ ] (7) Review logs for any warnings or errors
- [ ] (8) No critical warnings or errors in logs (**Verify**)
- [ ] (9) Commit final validation with message: "TASK-017: Complete .NET 10 upgrade validation"

---










