# Service Layer Skill

當使用者要求新增服務或業務功能時，使用此技能。

## 觸發條件

- 新增業務邏輯服務
- 建立 CRUD 功能
- 新增 Repository 資料存取

## 架構模式

### 分層流程

```
Controller → IService → Service → IRepository → Repository → SqlSugar
```

### 1. 建立服務介面

位置: `EC226.MemberSite.IServices/I{Name}Service.cs`

```csharp
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.MemberSite.IServices
{
    public interface I{Name}Service : IBaseServices<{EntityName}>
    {
        // 自訂方法
        Result<{ReturnType}> Get{Name}ById(int id);
        Result<List<{Name}VM>> GetAll();
    }
}
```

### 2. 建立服務實作

位置: `EC226.MemberSite.Services/{Name}Service.cs`

```csharp
using EC226.MemberSite.IRepository;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.MemberSite.Services
{
    public class {Name}Service : BaseServices<{EntityName}>, I{Name}Service
    {
        private readonly I{Name}Repository _{name}Repository;

        public {Name}Service(I{Name}Repository {name}Repository)
        {
            _{name}Repository = {name}Repository;
            BaseDal = {name}Repository;
        }

        public Result<{ReturnType}> Get{Name}ById(int id)
        {
            var result = new Result<{ReturnType}>();
            try
            {
                var data = _{name}Repository.QueryById(id);
                result.Data = data;
                result.Success();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
```

### 3. 建立 Repository 介面

位置: `EC226.MemberSite.IRepository/I{Name}Repository.cs`

```csharp
using EC226.MemberSite.Model.Entities;

namespace EC226.MemberSite.IRepository
{
    public interface I{Name}Repository : IBaseRepository<{EntityName}>
    {
        // 自訂資料存取方法
    }
}
```

### 4. 建立 Repository 實作

位置: `EC226.MemberSite.Repository/{Name}Repository.cs`

```csharp
using EC226.MemberSite.IRepository;
using EC226.MemberSite.Model.Entities;
using EC226.Repository.UnitOfWork;

namespace EC226.MemberSite.Repository
{
    public class {Name}Repository : BaseRepository<{EntityName}>, I{Name}Repository
    {
        public {Name}Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
```

### 5. 建立 Entity

位置: `EC226.MemberSite.Model/Entities/{EntityName}.cs`

```csharp
using SqlSugar;

namespace EC226.MemberSite.Model.Entities
{
    [SugarTable("{table_name}")]
    public class {EntityName}
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(Length = 100)]
        public string Name { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public DateTime? UpdateTime { get; set; }
    }
}
```

### 6. 建立 ViewModel

位置: `EC226.MemberSite.Model/ViewModels/{Name}VM.cs`

```csharp
namespace EC226.MemberSite.Model.ViewModels
{
    public class {Name}VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // 其他需要回傳的欄位
    }
}
```

### 7. 建立 Controller

位置: `EC226.MemberSite.Api/Controllers/{Name}Controller.cs`

```csharp
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace EC226.MemberSite.Api.Controllers
{
    [Route("{name}")]
    public class {Name}Controller : BaseController
    {
        private readonly I{Name}Service _{name}Service;

        public {Name}Controller(I{Name}Service {name}Service)
        {
            _{name}Service = {name}Service;
        }

        [HttpGet("{id}")]
        public Result<{ReturnType}> GetById(int id)
        {
            return _{name}Service.Get{Name}ById(id);
        }

        [HttpGet]
        public Result<List<{Name}VM>> GetAll()
        {
            return _{name}Service.GetAll();
        }
    }
}
```

## DI 自動註冊

Autofac 會自動掃描並註冊：
- `EC226.MemberSite.Services.dll` 中的所有服務
- `EC226.MemberSite.Repository.dll` 中的所有 Repository

**無需手動註冊**，只需遵循命名慣例即可。

## 必要檔案清單

| 檔案 | 專案 | 用途 |
|------|------|------|
| `I{Name}Service.cs` | IServices | 服務介面 |
| `{Name}Service.cs` | Services | 服務實作 |
| `I{Name}Repository.cs` | IRepository | Repository 介面 |
| `{Name}Repository.cs` | Repository | Repository 實作 |
| `{EntityName}.cs` | Model/Entities | 資料庫實體 |
| `{Name}VM.cs` | Model/ViewModels | 視圖模型 |
| `{Name}Controller.cs` | Api/Controllers | API 控制器 |

## 命名慣例

- 介面前綴 `I`: `IOrderService`, `IOrderRepository`
- 服務後綴 `Service`: `OrderService`
- Repository 後綴 `Repository`: `OrderRepository`
- ViewModel 後綴 `VM`: `OrderVM`
- Controller 後綴 `Controller`: `OrderController`

## 統一回傳格式

所有 API 回傳皆使用 `Result<T>` 包裝：

```csharp
var result = new Result<TData>();
result.Data = data;       // 設定資料
result.Success();         // 標記成功
result.Message = "錯誤訊息"; // 設定錯誤訊息
return result;
```
