# .NET E-Commerce API

> 以 .NET 10 打造的電商平台後端 API 架構範本 —— 分層架構、Autofac DI、SqlSugar ORM、Redis 快取 / 訊息佇列、Hangfire 排程，並內建可擴充的金流 Provider 模組（綠界 ECPay、藍新 Newebpay）。

[![.NET](https://img.shields.io/badge/.NET-10-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

> 🛠️ **關於這個專案**：這是一個在 AI 輔助開發工具問世之前，以「傳統工藝」打造的電商 side project —— 參考了開源社群與前輩們的架構設計，再依自己的需求整合、改寫與擴充，從分層架構、DI、ORM、快取、排程到金流串接逐步搭建起來。現在把它整理開源，作為 .NET 電商後端架構的參考與紀錄。

> ⚠️ 本專案為**架構開源**：所有金鑰、密碼、連線字串、內部主機位址皆已移除，`appsettings.json` 內僅保留**佔位值**。實際執行前請依下方「[設定](#️-設定)」章節填入你自己的值。

---

## 📑 目錄

- [功能特色](#-功能特色)
- [技術堆疊](#-技術堆疊)
- [專案結構](#-專案結構)
- [架構說明](#️-架構說明)
- [快速開始](#-快速開始)
- [設定](#️-設定)
- [金流模組](#-金流模組)
- [測試](#-測試)
- [Docker 部署](#-docker-部署)
- [相關文件](#-相關文件)
- [License](#-license)

---

## ✨ 功能特色

- **分層架構**：Controller → Service → Repository → Database，介面與實作分離（`IServices` / `IRepository`）
- **依賴注入**：Autofac 依命名慣例自動註冊 Service / Repository，支援 AOP（Castle DynamicProxy）
- **ORM**：SqlSugar，支援 MySQL / MariaDB 與 SQL Server 切換
- **認證授權**：JWT Bearer + 自訂 Policy
- **快取 & 佇列**：Redis 快取、Redis Message Queue（InitQ）
- **背景排程**：Hangfire（Redis / MySQL storage）
- **金流整合**：統一 `IPaymentProvider` 介面 + Factory，新增金流商只需實作一個 Provider
- **其他**：Swagger（OpenAPI）、NLog + Seq 結構化日誌、AutoMapper、MiniProfiler、Cloudflare Turnstile / reCAPTCHA 驗證、CRUD 程式碼產生器

## 🔧 技術堆疊

| 類別 | 技術 |
|------|------|
| Framework | .NET 10 / ASP.NET Core Web API |
| ORM | SqlSugar |
| DI Container | Autofac |
| Database | MySQL / MariaDB、SQL Server |
| Cache / MQ | Redis（StackExchange.Redis、InitQ） |
| Background Jobs | Hangfire |
| Auth | JWT（System.IdentityModel.Tokens.Jwt） |
| API Docs | Swagger / Swashbuckle |
| Logging | NLog + Seq |
| Mapping | AutoMapper |
| Testing | xUnit + FluentAssertions |

## 📁 專案結構

```
├── EC226.MemberSite.Api/          # Web API 進入點（Controllers、Program.cs、設定檔、郵件範本）
├── EC226.MemberSite.Services/     # 業務邏輯實作
├── EC226.MemberSite.IServices/    # 業務邏輯介面
├── EC226.MemberSite.Repository/   # 資料存取實作（SqlSugar）
├── EC226.MemberSite.IRepository/  # 資料存取介面
├── EC226.MemberSite.Model/        # Entities、ViewModels、DTOs、常數
├── EC226.Payment/                 # 金流模組（Abstractions / Factory / Providers）
├── EC226.Payment.Test/            # 金流單元測試
├── EC226.MemberSiteTest/          # 會員站測試
├── EC226.Auth/                    # JWT 認證授權
├── EC226.Core/                    # 核心設定（DI、SqlSugar、Redis、Swagger 註冊）
├── EC226.Caching/                 # 快取抽象（Redis / Memory）
├── EC226.Configuration/           # 設定讀取（AppSettingsHelper / AppSettingsConstVars）
├── EC226.Filter/                  # Action / Exception Filters
├── EC226.Middlewares/             # 中介軟體（請求日誌、IP 紀錄等）
├── EC226.Loging/                  # 日誌封裝
├── EC226.Mapping/                 # AutoMapper Profiles
├── EC226.RedisMQ/                 # Redis 訊息佇列消費者
├── EC226.Task/                    # Hangfire 背景工作
├── EC226.Swagger/                 # Swagger 擴充
├── EC226.Pagination/              # 分頁工具
├── EC226.Utility/                 # 共用工具類別
└── EC226.CodeGenerator/           # CRUD 程式碼產生器範本
```

## 🏗️ 架構說明

```
          HTTP Request
               │
   ┌───────────▼────────────┐
   │  Middlewares / Filters │  日誌、例外處理、驗證
   └───────────┬────────────┘
   ┌───────────▼────────────┐
   │      Controllers       │  EC226.MemberSite.Api
   └───────────┬────────────┘   ViewModel ⇄ DTO
   ┌───────────▼────────────┐
   │       Services         │  EC226.MemberSite.Services (IServices)
   └───────┬───────┬────────┘   業務邏輯、交易、快取
           │       │
           │   ┌───▼──────────────┐
           │   │ Payment Provider │  EC226.Payment
           │   └──────────────────┘
   ┌───────▼────────────────┐
   │     Repositories       │  EC226.MemberSite.Repository (IRepository)
   └───────────┬────────────┘   SqlSugar
               ▼
        MySQL / SQL Server        Redis（Cache / MQ / Hangfire）
```

| 分層 | 專案 | 職責 |
|------|------|------|
| **應用層** | `*.Api` | Controllers、路由、模型驗證 |
| **服務層** | `*.Services` / `*.IServices` | 業務邏輯、介面定義 |
| **資料層** | `*.Repository` / `*.IRepository` | 資料存取、ORM 操作 |
| **模型層** | `*.Model` | Entities、ViewModels、DTOs |
| **基礎設施** | `Core`、`Caching`、`Auth`、`Configuration` … | 跨模組共用功能 |

### 命名慣例

| 類型 | 格式 | 範例 |
|------|------|------|
| 控制器 | `{Name}Controller.cs` | `MemberController.cs` |
| 服務 | `{Name}Service.cs` | `OrderService.cs` |
| 介面 | `I{Name}.cs` | `IMemberService.cs` |
| ViewModel | `{Name}VM.cs` | `ProductVM.cs` |

## 🚀 快速開始

### 前置需求

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL 5.7+ / MariaDB 10.4+（或 SQL Server）
- Redis
- Docker（選用，用於快速啟動 DB / Redis）

### 1. 啟動 Redis 與資料庫

```powershell
# Redis（對應 appsettings.json 的 127.0.0.1:30006）
./build-local-redis-de.ps1

# MariaDB（對應 127.0.0.1:33070，會自動執行 init.sql 建立帳號）
cd EC226.MemberSite.Api/initMariadb
docker build -t ec226-mariadb --build-arg MYSQL_ROOT_PASSWORD=<your_root_password> .
docker run -d --name ec226-mariadb -p 33070:3306 ec226-mariadb
```

接著匯入範例 schema 與種子資料：`EC226.MemberSite.Api/initMariadb/ec226_202411252230.sql`。

> `init.sql` 內建立的 `ec226` 帳號密碼預設為 `change_me`，請自行修改並同步更新連線字串。

### 2. 設定機敏資訊

參考下方「[設定](#️-設定)」章節，建立 `appsettings.Development.json`（已被 `.gitignore` 排除）。

### 3. 建置與執行

```bash
dotnet restore
dotnet build
dotnet run --project EC226.MemberSite.Api
```

啟動後開啟 Swagger：`http://localhost:<port>/doc`

## ⚙️ 設定

`appsettings.json` 為**設定範本**，所有機敏欄位皆為空值或 `CHANGE_ME` 佔位值。

`AppSettingsHelper` 會依 `ASPNETCORE_ENVIRONMENT` 載入 `appsettings.{Environment}.json`（找不到時退回 `appsettings.json`），並可用**環境變數**覆寫任意欄位（以 `__` 表示階層）。

**本機開發** — 複製範本並填入真實值（此檔已在 `.gitignore` 中）：

```bash
cp EC226.MemberSite.Api/appsettings.json EC226.MemberSite.Api/appsettings.Development.json
```

**容器 / 正式環境** — 建議以環境變數注入：

```bash
ConnectionStrings__SqlConnection="Server=...;Database=...;Uid=...;Pwd=..."
JwtConfig__SecretKey="<random-32+-chars>"
Payment__Providers__ECPay__HashKey="..."
```

### 需要設定的機敏欄位

| 區段 | 欄位 | 說明 |
|------|------|------|
| `ConnectionStrings` | `SqlConnection` | 資料庫連線字串（`DbType`：`MySql` / `SqlServer`） |
| `RedisConfig` | `ConnectionString` | Redis 連線字串 |
| `HangFire` | `Login` / `PassWord` | Hangfire Dashboard 登入帳密 |
| `JwtConfig` | `SecretKey` / `TokenHelperSecret` | JWT 簽章金鑰（後者為 Base64） |
| `Encryption` | `DbKey` / `DbIV` / `ClientKey` / `ClientIV` | AES 金鑰，長度須為 16 / 24 / 32 bytes |
| `Mail` | `Host` / `Account` / `Password` … | SMTP 寄信設定 |
| `GoogleReCaptcha` | `KeyBackEndV2` / `KeyBackEndV3` | reCAPTCHA Server Key |
| `Cloudflare:Turnstile` | `SecretKey` / `BypassToken` | Turnstile 驗證 |
| `Sftp` | `Host` / `Username` / `Password` … | 圖片上傳 SFTP |
| `Payment:Providers` | `ECPay` / `Newebpay` 的 `MerchantID` / `HashKey` / `HashIV` | 台灣金流 |
| `Payment:PaymentOptions` | PayPal / Amazon Pay / Stripe 的 Client ID、Secret、Key | 國際金流 |

> 🔐 **請勿將任何真實金鑰提交至版本控制。** `.gitignore` 已排除 `appsettings.*.json`（保留 `appsettings.json` 範本）與 `.env`。
>
> Seq 日誌伺服器位址設定於 `EC226.MemberSite.Api/NLog.config`（預設 `http://localhost:5341/`）。

## 💳 金流模組

`EC226.Payment` 以 **Strategy + Factory** 模式設計，所有金流商實作統一介面：

```
EC226.Payment/
├── Abstractions/        # IPaymentProvider、IPaymentConfig、PaymentRequest / Response / CallbackResult
├── Factory/             # PaymentProviderFactory（依 PaymentType 建立 Provider）
└── Providers/
    ├── ECPay/           # 綠界：CheckMacValue (SHA256) 簽章
    └── Newebpay/        # 藍新：AES-256-CBC 加密 + SHA256 TradeSha
```

```csharp
public interface IPaymentProvider
{
    PaymentProviderType ProviderType { get; }
    PaymentResponse CreatePayment(PaymentRequest request);           // 產生付款 HTML Form
    PaymentCallbackResult ValidateCallback(IFormCollection form);    // 驗證回調（Form POST）
    PaymentCallbackResult ValidateCallback(string encryptedData);    // 驗證回調（加密資料）
    string GenerateCallbackResponse(bool success);                   // 回應金流商
}
```

使用方式：

```csharp
var provider = _providerFactory.Create(PaymentType.ECPAY);
var response = provider.CreatePayment(request);
```

**新增金流商**只需：

1. 在 `Providers/{Name}/` 實作 `IPaymentConfig` 與 `IPaymentProvider`
2. 在 `PaymentProviderFactory` 加入對應的建立邏輯
3. 在 `appsettings.json` 的 `Payment:Providers` 新增設定區段
4. 於 `EC226.MemberSite.Services` 繼承 `PaymentServiceBase` 實作服務

詳細步驟請參考 [金流 Provider 開發指引](.github/skills/payment-provider/SKILL.md)。

| 金流商 | 支援付款方式 |
|------|------|
| 綠界 ECPay | 信用卡、ATM、超商代碼、超商取貨付款 |
| 藍新 Newebpay | 信用卡、WebATM、ATM 轉帳 |
| Stripe / PayPal / Amazon Pay | 透過 `Payment:PaymentOptions` 設定 |

## 🧪 測試

```bash
# 全部測試
dotnet test

# 只跑金流測試
dotnet test EC226.Payment.Test
```

> 金流測試使用綠界官方公開的**測試商店**參數（MerchantID `2000132`），以及藍新的假資料，不會連到正式環境。

## 🐳 Docker 部署

```bash
# 在 repo 根目錄建置 API 映像檔
docker build -t dotnet-ecommerce-api -f EC226.MemberSite.Api/Dockerfile .

# 以環境變數注入機敏設定後執行
docker run -d -p 8080:80 \
  -e ConnectionStrings__SqlConnection="..." \
  -e RedisConfig__ConnectionString="..." \
  -e JwtConfig__SecretKey="..." \
  dotnet-ecommerce-api
```

遠端 Docker 主機部署可使用 `deploy-tw-dev.ps1`，執行前請先設定環境變數 `DOCKER_REMOTE_HOST`（例如 `10.0.0.10:2376`）。

## 📚 相關文件

- [專案架構說明](.github/instructions.md)
- [金流 Provider 開發指引](.github/skills/payment-provider/SKILL.md)
- [Service 層開發指引](.github/skills/service-layer/SKILL.md)
- [AI 開發指引（Copilot）](.github/copilot-instructions.md)
- [.NET 升級評估與計畫](.github/upgrades/)

## 📄 License

本專案採用 [Apache License 2.0](LICENSE) 授權。
