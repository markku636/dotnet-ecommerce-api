# EC226 .NET Core API 專案架構說明

> 本文件說明專案的資料夾命名、專案結構及檔案結構規範。

---

## 🤖 AI 快速理解區塊

### 專案一句話描述
.NET 10 電商平台後端 API，採用分層架構 (Controller → Service → Repository)，整合金流 (ECPay/Newebpay)、會員、訂單等功能。

### 核心技術堆疊
`ASP.NET Core` | `SqlSugar ORM` | `Autofac DI` | `Redis Cache` | `Hangfire` | `Swagger`

### 關鍵路徑速查
| 要做什麼 | 去哪裡找 |
|----------|----------|
| API 進入點 | `EC226.MemberSite.Api/Controllers/` |
| 業務邏輯 | `EC226.MemberSite.Services/` |
| 資料存取 | `EC226.MemberSite.Repository/` |
| 資料模型 | `EC226.MemberSite.Model/Entities/` |
| 金流整合 | `EC226.Payment/Providers/` |
| DI 註冊 | `EC226.MemberSite.Api/AutoFac/` |

### 架構流程圖
```
[Controller] → [IService] → [Service] → [IRepository] → [Repository] → [SqlSugar/DB]
      ↓              ↓            ↓
  ViewModel       Entity      Entity
```

### 常見開發任務
- **新增 API**: 在 `Controllers/` 建立繼承 `BaseController` 的控制器
- **新增服務**: 介面放 `IServices/`，實作放 `Services/`，會自動註冊
- **新增金流**: 參考 `.github/skills/payment-provider/SKILL.md`

---

## 📁 資料夾命名規範

### 命名原則
- **專案資料夾**：使用 `EC226.` 前綴 + 功能模組名稱 (PascalCase)
- **子資料夾**：使用 PascalCase 命名
- **特殊資料夾**：`.github`、`docs` 等使用小寫

### 專案資料夾命名規則

| 類型 | 命名格式 | 範例 |
|------|----------|------|
| 核心服務 | `EC226.{ModuleName}` | `EC226.Core`, `EC226.Utility` |
| 會員站專案 | `EC226.MemberSite.{Layer}` | `EC226.MemberSite.Api`, `EC226.MemberSite.Services` |
| CMS 專案 | `EC226.CMS.{Layer}` | `EC226.CMS.Api` |
| 測試專案 | `EC226.{ModuleName}.Test` | `EC226.Payment.Test` |

---

## 🏗️ 專案結構 (Solution Structure)

### 分層架構總覽

```
ec226.dotnet.core.api/
├── .github/                          # GitHub 相關配置
│   └── upgrades/                     # 升級計畫文件
├── docs/                             # 專案文件
│
├── ===== 應用程式層 (Application Layer) =====
├── EC226.MemberSite.Api/             # 會員站 Web API (主要 API 入口)
├── EC226.CMS.Api/                    # CMS 管理後台 (Razor Pages)
│
├── ===== 服務層 (Service Layer) =====
├── EC226.MemberSite.Services/        # 會員站業務邏輯服務實作
├── EC226.MemberSite.IServices/       # 會員站服務介面定義
│
├── ===== 資料存取層 (Data Access Layer) =====
├── EC226.MemberSite.Repository/      # 會員站資料存取實作
├── EC226.MemberSite.IRepository/     # 會員站資料存取介面定義
├── EC226.MemberSite.Model/           # 實體模型、ViewModel、DTO
│
├── ===== 基礎設施層 (Infrastructure Layer) =====
├── EC226.Configuration/              # 全域設定、常數定義
├── EC226.Core/                       # 核心配置 (DI、Swagger、Cache 設定)
├── EC226.Caching/                    # 快取服務 (Redis/Memory Cache)
├── EC226.RedisMQ/                    # Redis 訊息佇列
├── EC226.Loging/                     # 日誌記錄
│
├── ===== 共用功能層 (Cross-Cutting Layer) =====
├── EC226.Auth/                       # 認證授權 (JWT)
├── EC226.Filter/                     # 過濾器
├── EC226.Middlewares/                # 中介軟體
├── EC226.Swagger/                    # Swagger 文件配置
├── EC226.Mapping/                    # AutoMapper 映射
├── EC226.Utility/                    # 工具類別庫
├── EC226.Pagination/                 # 分頁功能
├── EC226.Task/                       # 背景任務
│
├── ===== 金流模組 (Payment Module) =====
├── EC226.Payment/                    # 金流整合服務
├── EC226.Payment.Test/               # 金流單元測試
│
├── ===== 程式碼產生器 =====
├── EC226.CodeGenerator/              # 程式碼產生工具
│
├── ===== 其他測試專案 =====
└── EC226.MemberSiteTest/             # 會員站測試專案
```

### 專案相依性圖 (Dependency Graph)

```
Level 0 (無相依性):
  └── EC226.Configuration
  └── EC226.Pagination
  └── EC226.Loging

Level 1 (相依 Level 0):
  └── EC226.Utility
  └── EC226.Caching
  └── EC226.Core
  └── EC226.Auth

Level 2 (相依 Level 0-1):
  └── EC226.MemberSite.Model
  └── EC226.MemberSite.IRepository
  └── EC226.MemberSite.IServices
  └── EC226.Payment
  └── EC226.Swagger
  └── EC226.RedisMQ
  └── EC226.CodeGenerator

Level 3 (相依 Level 0-2):
  └── EC226.MemberSite.Repository
  └── EC226.MemberSite.Services
  └── EC226.Filter
  └── EC226.Mapping
  └── EC226.Task
  └── EC226.Middlewares

Level 4 (頂層應用程式):
  └── EC226.CMS.Api
  └── EC226.MemberSite.Api
  └── EC226.Payment.Test
```

---

## 📂 檔案結構規範

### EC226.MemberSite.Api (Web API 專案)

```
EC226.MemberSite.Api/
├── Program.cs                        # 應用程式進入點
├── AutoFac/
│   └── AutofacModuleRegister.cs      # Autofac DI 註冊
├── Attributes/
│   ├── StateAttribute.cs             # 狀態屬性
│   └── SkipAttribute.cs              # 跳過屬性
└── Controllers/
    ├── BaseController.cs             # 控制器基底類別
    ├── HealthController.cs           # 健康檢查
    ├── MemberController.cs           # 會員管理
    ├── ProductController.cs          # 產品管理
    ├── CategoryController.cs         # 分類管理
    ├── ArticleController.cs          # 文章管理
    ├── OrderController.cs            # 訂單管理
    ├── CartController.cs             # 購物車
    ├── PaymentController.cs          # 金流
    ├── TestPaymentController.cs      # 金流測試
    ├── CodeGeneratorController.cs    # 程式碼產生
    └── TestController.cs             # 測試用
```

### EC226.CMS.Api (CMS 後台 - Razor Pages)

```
EC226.CMS.Api/
├── Program.cs                        # 應用程式進入點
├── Code/
│   ├── AppStart/
│   │   ├── Configures.cs             # 應用程式配置
│   │   ├── Services.cs               # 服務註冊
│   │   ├── BaseController.cs         # 控制器基底
│   │   └── InitTable.cs              # 資料表初始化
│   ├── Filters/
│   │   ├── ActionFilter.cs           # Action 過濾器
│   │   └── ExceptionFilter.cs        # 例外過濾器
│   └── Util/
│       ├── FileHelper.cs             # 檔案處理
│       ├── ZipHelper.cs              # 壓縮處理
│       ├── MemoryCache.cs            # 記憶體快取
│       ├── TemplateHelper.cs         # 範本處理
│       ├── AutoMapperExtension.cs    # AutoMapper 擴充
│       └── SyntaxTreeHelper.cs       # 語法樹處理
├── Controllers/
│   ├── ImageController.cs            # 圖片管理
│   ├── ContentController.cs          # 內容管理
│   ├── CollectionController.cs       # 集合管理
│   └── ScheduleController.cs         # 排程管理
├── CodeGenApis/                      # 程式碼產生 API
│   ├── CodeTableApp/
│   ├── DatabaseApp/
│   ├── ProjectApp/
│   ├── TemplateApp/
│   └── ...
├── Dal/
│   └── Repository.cs                 # 資料存取
├── Filter/
│   └── CustomJsonFilter.cs           # JSON 過濾器
├── Helpers/
│   ├── ImageHelper.cs                # 圖片工具
│   ├── JobService.cs                 # 背景工作服務
│   ├── ScheduleHelper.cs             # 排程工具
│   └── SchemaController.cs           # Schema 控制器
├── InitProjects/                     # 專案初始化
│   ├── 1_Main.cs
│   ├── 2_SlnManager.cs
│   ├── 3_ProjectManager.cs
│   ├── 4_TemplateManager.cs
│   ├── Helper.cs
│   └── Model.cs
├── Pages/                            # Razor Pages
│   ├── Index.cshtml(.cs)
│   ├── Database.cshtml(.cs)
│   ├── Project.cshtml(.cs)
│   ├── Template.cshtml(.cs)
│   ├── CodeTableInfo.cshtml(.cs)
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   ├── Menu.cshtml(.cs)
│   │   ├── Top.cshtml(.cs)
│   │   └── ...
│   └── ...
└── wwwroot/                          # 靜態資源
    ├── vendors/                      # 第三方函式庫
    └── template/                     # 程式碼範本
```

### EC226.MemberSite.Model (領域模型專案)

```
EC226.MemberSite.Model/
├── Entities/                         # 資料庫實體
│   ├── Member.cs
│   ├── Order.cs
│   ├── OrderDetail.cs
│   ├── Article.cs
│   ├── Category.cs
│   ├── Product.cs
│   ├── StockKeepingUnit.cs
│   ├── Payment.cs
│   ├── Transaction.cs
│   ├── Role.cs
│   ├── Account.cs
│   └── Expression/                   # 表達式工具
│       ├── PredicateBuilder.cs
│       └── ParameterRebinder.cs
├── ViewModels/                       # 視圖模型
│   ├── MemberVM.cs
│   ├── OrderVM.cs
│   ├── ProductVM.cs
│   ├── ArticleVM.cs
│   ├── CategoryVM.cs
│   ├── LoginVM.cs
│   ├── AuthResult.cs
│   ├── Payment/
│   │   └── GeneralPaymentConfig.cs
│   ├── CMS/
│   │   ├── ContentViewModel.cs
│   │   ├── ContentSchedule.cs
│   │   └── ...
│   ├── Basics/
│   │   ├── PageList.cs
│   │   └── IPageList.cs
│   ├── UI/
│   │   ├── Result.cs
│   │   ├── AdminUiCallBack.cs
│   │   └── ApiReturnCode.cs
│   └── LogInfo/
│       └── LogInfo.cs
├── FromBody/                         # 請求模型
│   ├── FMId.cs
│   └── Member/
│       └── FMLogin.cs
├── CustomEntities/                   # 自訂實體
├── Enums/                            # 列舉
│   ├── OrderStatus.cs
│   ├── OrderType.cs
│   ├── Channel.cs
│   └── ...
├── Const/                            # 常數
│   ├── HardCodeKey.cs
│   ├── RedisPrefixKey.cs
│   └── StoredProcedure.cs
├── Attributes/                       # 自訂屬性
│   ├── StateAttribute.cs
│   └── SkipAttribute.cs
└── CodeGenModels/                    # 程式碼產生器模型
    ├── DbModel/
    ├── ViewModel/
    ├── Common/
    └── Gen/
```

### EC226.MemberSite.Services (服務實作專案)

```
EC226.MemberSite.Services/
├── BaseServices.cs                   # 服務基底類別
├── MemberService.cs                  # 會員服務
├── OrderService.cs                   # 訂單服務
├── ProductManagementService.cs       # 產品管理服務
├── CategoryService.cs                # 分類服務
├── ArticleService.cs                 # 文章服務
├── ContentService.cs                 # 內容服務
├── CollectionService.cs              # 集合服務
├── ScheduleService.cs                # 排程服務
├── SequenceService.cs                # 序號服務
├── CodeGeneratorServices.cs          # 程式碼產生服務
├── PaymentAbstractService.cs         # 金流抽象服務
└── AutoMapperProfile/
    └── MapProfile.cs                 # AutoMapper 設定檔
```

### EC226.Configuration (設定專案)

```
EC226.Configuration/
├── AppSettingsHelper.cs              # appsettings 讀取工具
├── AppSettingsConstVars.cs           # appsettings 常數
├── GlobalConstVars.cs                # 全域常數
├── GlobalEnumVars.cs                 # 全域列舉
├── SystemSettingConstVars.cs         # 系統設定常數
└── SystemSettingDictionary.cs        # 系統設定字典
```

### EC226.Core (核心配置專案)

```
EC226.Core/
├── AutoFac/
│   └── AutofacModuleRegister.cs      # Autofac 模組註冊
├── Config/
│   ├── SqlSugarSetup.cs              # SqlSugar ORM 設定
│   ├── RedisCacheSetup.cs            # Redis 快取設定
│   ├── MemoryCacheSetup.cs           # 記憶體快取設定
│   ├── SwaggerSetup.cs               # Swagger 設定
│   ├── CorsSetup.cs                  # CORS 設定
│   ├── HttpContextSetup.cs           # HttpContext 設定
│   ├── HangFireSetup.cs              # Hangfire 背景任務設定
│   └── RedisMessageQueueSetup.cs     # Redis MQ 設定
├── AOP/
│   ├── CacheAopBase.cs               # 快取 AOP 基底
│   ├── RedisCacheAop.cs              # Redis 快取 AOP
│   └── MemoryCacheAop.cs             # 記憶體快取 AOP
└── Attribute/
    ├── CachingAttribute.cs           # 快取屬性
    ├── RateLimitAttribute.cs         # 限流屬性
    └── UseTranAttribute.cs           # 交易屬性
```

---

## 🔧 技術堆疊

| 類別 | 技術 |
|------|------|
| Framework | .NET 10 |
| Web Framework | ASP.NET Core (Web API + Razor Pages) |
| ORM | SqlSugar |
| DI Container | Autofac |
| Cache | Redis + Memory Cache |
| Message Queue | Redis MQ |
| Background Jobs | Hangfire |
| Documentation | Swagger/OpenAPI |
| Logging | NLog + Seq |
| Mapper | AutoMapper |

---

## 📝 命名慣例

### 檔案命名

| 類型 | 命名規則 | 範例 |
|------|----------|------|
| 控制器 | `{Name}Controller.cs` | `MemberController.cs` |
| 服務 | `{Name}Service.cs` | `MemberService.cs` |
| 介面 | `I{Name}.cs` | `IMemberService.cs` |
| 實體 | `{EntityName}.cs` | `Member.cs` |
| ViewModel | `{Name}VM.cs` | `MemberVM.cs` |
| 設定 | `{Feature}Setup.cs` | `SqlSugarSetup.cs` |
| 過濾器 | `{Name}Filter.cs` | `ExceptionFilter.cs` |
| 屬性 | `{Name}Attribute.cs` | `CachingAttribute.cs` |

### 類別命名

- **PascalCase**：所有類別、方法、屬性
- **camelCase**：私有欄位、參數、區域變數
- **_camelCase**：私有欄位 (選用底線前綴)

---

## 📦 NuGet 套件管理

專案使用 `Directory.Build.props` 集中管理相依套件版本，確保：
- 統一的套件版本
- 安全性漏洞修復
- 跨專案一致性

```xml
<!-- Directory.Build.props -->
<Project>
  <ItemGroup>
    <PackageReference Include="BouncyCastle.Cryptography" Version="2.6.2" />
    <PackageReference Include="Npgsql" Version="10.0.1" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="8.15.0" />
    <PackageReference Include="Microsoft.IdentityModel.JsonWebTokens" Version="8.15.0" />
  </ItemGroup>
</Project>
```

---

## 🔗 相關文件

- [升級評估報告](upgrades/assessment.md)
- [升級計畫](upgrades/plan.md)
- [升級任務清單](upgrades/tasks.md)
- [執行日誌](upgrades/execution-log.md)

---

## 🚀 快速開始 (Quick Start)

### 安裝步驟

1. 確保已安裝 .NET 10 SDK
2. 複製專案後，還原相依套件：
   ```bash
   dotnet restore
   ```
3. 設定 `appsettings.json` 中的資料庫連線和金流參數
4. 編譯專案：
   ```bash
   dotnet build
   ```
5. 啟動 API：
   ```bash
   dotnet run --project EC226.MemberSite.Api
   ```

### 使用方式

```bash
# 執行 API 服務
dotnet run --project EC226.MemberSite.Api

# 執行 CMS 後台
dotnet run --project EC226.CMS.Api

# 執行單元測試
dotnet test EC226.Payment.Test
```

### 新增金流 Provider

1. 在 `EC226.Payment/Providers/{Name}/` 建立 Config 和 Provider 類別
2. 實作 `IPaymentProvider` 介面
3. 在 `PaymentProviderFactory` 新增建立方法
4. 建立 Service 類別繼承 `PaymentServiceBase`

### 注意事項

- 金流測試環境請使用 `CreateSandbox()` 建立測試設定
- 正式環境務必設定正確的 `MerchantID`、`HashKey`、`HashIV`
- 回調網址 (`NotifyUrl`) 必須為公開可連線的 HTTPS 網址
- 專案使用 Autofac 進行依賴注入，新增服務需確認註冊設定
- 修改 Abstractions 介面需同步更新所有 Provider 實作

---

*最後更新：2026-01-11*
