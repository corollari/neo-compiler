using System;

namespace Neo.Compiler.DotNet.Utils
{
    internal class DefLogger : ILogger
    {
        public void Log(string log)
        {
            Console.WriteLine(log);
        }
    }
}
