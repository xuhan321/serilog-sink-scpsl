using System;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace Serilog.Sinks.Scpsl
{
    /// <summary>
    /// 代表游戏SCP:SL（SCP:秘密实验室）接入Serilog的Sink。
    /// </summary>
    /// <remarks>
    /// 按照自定义的<see cref="ITextFormatter"/>模板新建此Sink。
    /// </remarks>
    /// <param name="formatter">要被传入的自定义模板。</param>
    public class ScpslSink(ITextFormatter formatter) : ILogEventSink
    {
        private readonly ITextFormatter _formatter = formatter ?? new MessageTemplateTextFormatter(
                "[{Level}] {Message:lj}{NewLine}{Exception}",
                formatProvider: null);
        private readonly object _locker = new();
        private readonly string _prefix = "[Serilog]";

        /// <summary>
        /// 按照默认<see cref="ITextFormatter"/>模板新建此Sink，新模板遵守以下格式。
        /// <para><c>[{Level}] {Message:lj}{NewLine}{Exception}</c></para>
        /// </summary>
        public ScpslSink() : this(null)
        {

        }
        /// <summary>
        /// 按照自定义的<see cref="ITextFormatter"/>模板与自定义的Prefix头新建此Sink。
        /// </summary>
        /// <param name="formatter">要被传入的自定义模板。</param>
        /// <param name="prefix">自定义的Prefix头。</param>
        public ScpslSink(ITextFormatter formatter, string prefix) : this(formatter)
        {
            _prefix = prefix;
        }
        /// <inheritdoc/>
        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
                throw new ArgumentNullException(nameof(logEvent));

            string renderedMessage;
            lock (_locker)
            {
                using var writer = new StringWriter();
                _formatter.Format(logEvent, writer);
                renderedMessage = writer.ToString();
            }

            string prefixPart = string.IsNullOrEmpty(_prefix) ? "" : _prefix + " ";
            string finalMessage = prefixPart + renderedMessage;

            ServerConsole.AddLog(finalMessage, GetColor(logEvent.Level));
        }

        private static ConsoleColor GetColor(LogEventLevel level) => level switch
        {
            LogEventLevel.Fatal => ConsoleColor.DarkRed,
            LogEventLevel.Verbose => ConsoleColor.DarkGreen,
            LogEventLevel.Debug => ConsoleColor.DarkGray,
            LogEventLevel.Information => ConsoleColor.Cyan,
            LogEventLevel.Warning => ConsoleColor.Yellow,
            LogEventLevel.Error => ConsoleColor.Red,
            _ => ConsoleColor.Gray
        };
    }
}
