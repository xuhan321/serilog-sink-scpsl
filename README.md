# Serilog.Sinks.Scpsl

> 为 SCP:SL 服务器定制的 SerilogSink，通过 `ServerConsole.AddLog` 输出到LocalAdmin控制台。

## 快速开始

```csharp
using Serilog;
using Serilog.Sinks.Scpsl;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Scpsl()
    .CreateLogger();

Log.Information("服务器启动");

```

## 配置
```csharp
.WriteTo.Scpsl(
ITextFormatter formatter = null,                     // 自定义格式化器，默认为 "[{Level}] {Message:lj}{NewLine}{Exception}"
    LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
)
```

#### 构造函数重载

```csharp
new ScpslSink()                                          // 默认前缀 "[Serilog]"
new ScpslSink(ITextFormatter formatter)                  // 自定义格式
new ScpslSink(ITextFormatter formatter, string prefix)   // 自定义格式和前缀（传入 null 或 "" 可移除前缀）
```

> ⚠️ **提示**：LocalAdmin 控制台会自动为每条日志添加时间戳，且无法修改。因此 Sink 的**默认格式化器**已去除 `{Timestamp}`，避免重复。如果你自定义格式化器，也请勿包含时间戳。

#### 输出示例
```csharp
 // LA配置假设时间戳为yyyy-MM-dd HH:mm:ss，前缀配置 "[Serilog]"

Log.Information("你好！");
// LA输出： [2026-07-12 20:00:00] [Serilog] [INFORMATION] 你好！

try
{
    throw new InvalidOperationException("测试异常");
}
catch (Exception ex)
{
    Log.Error(ex, "操作失败");
}
// 输出： [2026-07-12 20:00:00] [Serilog] [ERROR] 操作失败: System.InvalidOperationException: 测试异常
//        堆栈跟踪信息...
```

#### 自定义格式化器示例
```csharp
// 仅输出消息（无级别、无前缀）
var formatter = new MessageTemplateTextFormatter("{Message:lj}");
.WriteTo.Scpsl(formatter: formatter, prefix: "");
// 输出： [2026-07-12 20:00:00] 你好！

.WriteTo.Scpsl(prefix: "[MyPlugin]");
// 输出： [2026-07-12 20:00:00] [MyPlugin] [INFORMATION] 你好！
```
## 颜色映射
日志级别会自动映射为对应的控制台颜色（通过 `ServerConsole.AddLog` 的第二个参数应用）：
| 级别 | 颜色 |
| ----- | ----- |
| Fatal | DarkRed |
| Error | Red |
| Warning | Yellow |
| Information | Cyan |
| Debug | DarkGray |
| Verbose | DarkGreen |
| 其他 | Gray |
