using System.Diagnostics.CodeAnalysis;
using Vintagestory.API.Common;

namespace VintageQuestingTest.util
{
    internal class TestLogger : LoggerBase
    {
        protected override void LogImpl(EnumLogType logType, [StringSyntax("CompositeFormat")] string format, params object[] args)
        {
            Console.WriteLine($"[{logType}] {string.Format(format, args)}");
        }
    }
}
