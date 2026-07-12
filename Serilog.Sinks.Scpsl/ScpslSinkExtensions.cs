using System;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.Scpsl
{
    /// <summary>
    /// 接入Serilog的拓展类合集。
    /// </summary>
    public static class ScpslSinkExtensions
    {
        /// <summary>
        /// 添加 Scpsl-LA 输出 Sink。
        /// </summary>
        /// <param name="loggerConfiguration">日志配置。</param>
        /// <param name="formatter">可选的自定义格式化器。</param>
        /// <param name="restrictedToMinimumLevel">最小日志级别（默认 Verbose）。</param>
        /// <returns>配置对象。</returns>
        public static LoggerConfiguration Scpsl(
            this LoggerSinkConfiguration loggerConfiguration,
            ITextFormatter formatter = null,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose)
        {
            if (loggerConfiguration == null)
                throw new ArgumentNullException(nameof(loggerConfiguration));

            return loggerConfiguration.Sink(
                new ScpslSink(formatter),
                restrictedToMinimumLevel);
        }
    }
}
