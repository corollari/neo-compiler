using Neo.VM;

namespace Neo.Compiler.DotNet.Utils
{
    internal class TestTable : IScriptTable
    {
        public byte[] GetScript(byte[] script_hash)
        {
            if (script_hash.Length == 1 && script_hash[0] == 99)
            {
                return new byte[] { (byte)OpCode.DROP, (byte)OpCode.DROP, (byte)OpCode.RET };
            }
            return new byte[] { (byte)OpCode.PUSH1, (byte)OpCode.RET };
        }
    }
}
