# Payment Provider Skill

當使用者要求新增金流服務時，使用此技能。

## 觸發條件

- 新增金流提供商 (Payment Provider)
- 整合第三方金流
- 建立付款服務

## 架構模式

### 1. 建立 Config 類別

在 `EC226.Payment/Providers/{ProviderName}/` 目錄下建立設定類別：

```csharp
public class {ProviderName}Config : IPaymentConfig
{
    public string MerchantID { get; set; }
    public string HashKey { get; set; }
    public string HashIV { get; set; }
    public string ApiUrl { get; set; }
    public string NotifyUrl { get; set; }
    public string ReturnUrl { get; set; }
    public bool IsSandbox { get; set; }

    public static {ProviderName}Config CreateSandbox() => new()
    {
        MerchantID = "測試商店代號",
        HashKey = "測試 HashKey",
        HashIV = "測試 HashIV",
        ApiUrl = "測試 API 網址",
        IsSandbox = true
    };
}
```

### 2. 建立 Provider 類別

實作 `IPaymentProvider` 介面：

```csharp
public class {ProviderName}PaymentProvider : IPaymentProvider
{
    private readonly {ProviderName}Config _config;

    public PaymentProviderType ProviderType => PaymentProviderType.{ProviderName};

    public {ProviderName}PaymentProvider({ProviderName}Config config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public PaymentResponse CreatePayment(PaymentRequest request)
    {
        // 1. 驗證請求參數
        // 2. 建立金流商所需參數
        // 3. 產生檢查碼/簽章
        // 4. 產生 HTML Form
        // 5. 回傳 PaymentResponse
    }

    public PaymentCallbackResult ValidateCallback(IFormCollection form)
    {
        // 驗證並解析回調通知
    }

    public PaymentCallbackResult ValidateCallback(string encryptedData)
    {
        // 驗證加密資料回調 (如不支援可回傳 Invalid)
    }

    public string GenerateCallbackResponse(bool success)
    {
        // 回應給金流商的內容
    }
}
```

### 3. 新增 PaymentProviderType 列舉

在 `EC226.Payment/Abstractions/IPaymentProvider.cs` 新增列舉值：

```csharp
public enum PaymentProviderType
{
    ECPay,
    Newebpay,
    {ProviderName}  // 新增
}
```

### 4. 更新 PaymentProviderFactory

在 `EC226.Payment/Factory/PaymentProviderFactory.cs` 新增建立方法：

```csharp
public IPaymentProvider Create{ProviderName}Provider()
{
    var config = Get{ProviderName}Config();
    return new {ProviderName}PaymentProvider(config);
}

public {ProviderName}Config Get{ProviderName}Config()
{
    var section = _configuration.GetSection("{ProviderName}");
    return new {ProviderName}Config
    {
        MerchantID = section["MerchantID"],
        HashKey = section["HashKey"],
        HashIV = section["HashIV"],
        ApiUrl = section["ApiUrl"],
        NotifyUrl = section["NotifyUrl"],
        ReturnUrl = section["ReturnUrl"],
        IsSandbox = bool.Parse(section["IsSandbox"] ?? "false")
    };
}
```

### 5. 建立 Service 類別

在 `EC226.MemberSite.Services/` 目錄下建立服務類別：

```csharp
public class {ProviderName}Service : PaymentServiceBase
{
    protected override PaymentProviderType ProviderType => PaymentProviderType.{ProviderName};

    public {ProviderName}Service(
        IConfiguration configuration,
        PaymentProviderFactory providerFactory,
        IHttpContextAccessor httpContextAccessor)
        : base(configuration, providerFactory, httpContextAccessor)
    {
    }

    protected override IPaymentProvider GetProvider()
    {
        return _providerFactory.Create{ProviderName}Provider();
    }
}
```

### 6. 設定 appsettings.json

```json
"{ProviderName}": {
    "MerchantID": "商店代號",
    "HashKey": "HashKey",
    "HashIV": "HashIV",
    "ApiUrl": "API 網址",
    "NotifyUrl": "https://yoursite.com/payment/{provider}/notify",
    "ReturnUrl": "https://yoursite.com/payment/success",
    "IsSandbox": true
}
```

### 7. 撰寫單元測試

在 `EC226.Payment.Test/` 目錄下建立測試：

```csharp
public class {ProviderName}PaymentProviderTest
{
    private readonly {ProviderName}Config _config;
    private readonly {ProviderName}PaymentProvider _provider;

    public {ProviderName}PaymentProviderTest()
    {
        _config = {ProviderName}Config.CreateSandbox();
        _config.NotifyUrl = "https://test.com/notify";
        _config.ReturnUrl = "https://test.com/return";
        _provider = new {ProviderName}PaymentProvider(_config);
    }

    [Fact]
    public void CreatePayment_ValidRequest_ReturnsSuccessResponse()
    {
        var request = new PaymentRequest
        {
            OrderNo = $"TEST{DateTime.Now:yyyyMMddHHmmss}",
            Amount = 100,
            ItemDescription = "測試商品",
            PaymentMethod = PaymentMethodType.Credit,
            TradeDate = DateTime.Now
        };

        var response = _provider.CreatePayment(request);

        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.HtmlForm.Should().NotBeNullOrEmpty();
    }
}
```

## 必要檔案清單

| 檔案 | 用途 |
|------|------|
| `EC226.Payment/Providers/{Name}/{Name}Config.cs` | 金流設定 |
| `EC226.Payment/Providers/{Name}/{Name}PaymentProvider.cs` | 金流 Provider |
| `EC226.Payment/Providers/{Name}/{Name}Encryption.cs` | 加解密工具 (如需要) |
| `EC226.MemberSite.Services/{Name}Service.cs` | 服務層實作 |
| `EC226.Payment.Test/{Name}PaymentProviderTest.cs` | 單元測試 |

## 命名慣例

- Config 類別: `{ProviderName}Config`
- Provider 類別: `{ProviderName}PaymentProvider`
- Service 類別: `{ProviderName}Service`
- 測試類別: `{ProviderName}PaymentProviderTest`
