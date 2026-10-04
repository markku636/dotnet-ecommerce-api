# EC226 Copilot Instructions

> 此文件幫助 GitHub Copilot 理解專案架構和開發慣例。

## 專案概述

這是一個 .NET 10 電商平台後端 API，採用分層架構設計。

## 架構模式

### 分層結構
```
Controller → Service → Repository → Database
     ↓           ↓           ↓
  ViewModel    Entity     SqlSugar
```

### 命名空間對應
- `EC226.MemberSite.Api` - Web API 控制器
- `EC226.MemberSite.Services` - 業務邏輯實作
- `EC226.MemberSite.IServices` - 服務介面定義
- `EC226.MemberSite.Repository` - 資料存取實作
- `EC226.MemberSite.IRepository` - Repository 介面
- `EC226.MemberSite.Model` - 實體和 ViewModel

## 開發慣例

### 新增功能時

1. **新增 Controller**
   - 位置: `EC226.MemberSite.Api/Controllers/`
   - 繼承: `BaseController`
   - 路由: `[Route("{feature}")]`

2. **新增 Service**
   - 介面: `EC226.MemberSite.IServices/I{Name}Service.cs`
   - 實作: `EC226.MemberSite.Services/{Name}Service.cs`
   - 繼承: `BaseServices<TEntity>` (如需 CRUD)

3. **新增 Repository**
   - 介面: `EC226.MemberSite.IRepository/I{Name}Repository.cs`
   - 實作: `EC226.MemberSite.Repository/{Name}Repository.cs`

4. **新增 Entity**
   - 位置: `EC226.MemberSite.Model/Entities/`
   - ORM: SqlSugar 標記

5. **新增 ViewModel**
   - 位置: `EC226.MemberSite.Model/ViewModels/`
   - 命名: `{Name}VM.cs`

### 金流開發

金流模組採用 Factory + Provider 模式：

```csharp
// 1. 建立 Config (EC226.Payment/Providers/{Name}/)
public class {Name}Config : IPaymentConfig { }

// 2. 建立 Provider (實作 IPaymentProvider)
public class {Name}PaymentProvider : IPaymentProvider { }

// 3. 更新 Factory
_providerFactory.Create{Name}Provider();

// 4. 建立 Service (繼承 PaymentServiceBase)
public class {Name}Service : PaymentServiceBase { }
```

### 依賴注入

專案使用 Autofac，服務自動註冊：
- Services DLL 自動掃描註冊
- Repository DLL 自動掃描註冊
- 金流服務使用 Named 註冊

## 程式碼風格

### C# 慣例
- 使用 PascalCase: 類別、方法、屬性
- 使用 camelCase: 參數、區域變數
- 私有欄位: `_camelCase`
- 介面前綴: `I`

### 回傳格式
```csharp
// 統一使用 Result<T> 包裝
public Result<TData> MethodName()
{
    var result = new Result<TData>();
    result.Data = data;
    result.Success();
    return result;
}
```

### 例外處理
```csharp
try
{
    // 業務邏輯
}
catch (Exception ex)
{
    result.Message = ex.Message;
    // 記錄日誌
}
```

## 常用路徑

| 用途 | 路徑 |
|------|------|
| API 控制器 | `EC226.MemberSite.Api/Controllers/` |
| 服務實作 | `EC226.MemberSite.Services/` |
| 服務介面 | `EC226.MemberSite.IServices/` |
| 實體模型 | `EC226.MemberSite.Model/Entities/` |
| ViewModel | `EC226.MemberSite.Model/ViewModels/` |
| 金流 Provider | `EC226.Payment/Providers/` |
| 設定檔 | `EC226.MemberSite.Api/appsettings.json` |
| DI 註冊 | `EC226.MemberSite.Api/AutoFac/` |

## 測試慣例

- 測試專案: `EC226.{Module}.Test`
- 測試框架: xUnit + FluentAssertions
- 命名: `{MethodName}_{Scenario}_{ExpectedResult}`

```csharp
[Fact]
public void CreatePayment_ValidRequest_ReturnsSuccessResponse()
{
    // Arrange
    // Act
    // Assert
}
```

## 注意事項

1. 新增服務會自動被 Autofac 掃描註冊
2. 修改介面需同步更新所有實作
3. 金流回調必須使用 HTTPS
4. 使用 `CreateSandbox()` 進行金流測試
