using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace neon.unittest
{
    [TestClass]
    public class UnitTest1
    {
        static testtool.neontesttool testtool = new testtool.neontesttool("net4smartcontract.test.dll");
        static void DumpAVM(Neo.Compiler.NeoMethod avmMethod)
        {
            System.Console.WriteLine("dump:" + avmMethod.displayName + " addr in avm:" + avmMethod.funcaddr);
            foreach(var c in avmMethod.body_Codes)
            {
                System.Console.WriteLine(c.Key.ToString("X04") + "=>" + c.Value.ToString());
            }
        }
        static void DumpBytes(byte[] data)
        {
            System.Console.WriteLine("AVM=");
            foreach (var b in data)
            {
                System.Console.Write(b.ToString("X02"));
            }
            System.Console.WriteLine("");
        }
        [TestMethod]
        public void GetALLILFunction()
        {
            var names = testtool.GetAllILFunction();
            foreach (var n in names)
            {
                System.Console.WriteLine("got name:" + n);
            }
        }
        [TestMethod]
        public void TestDumpAFunc()
        {
            var ilmethod = testtool.FindMethod("TestClass1", "UnitTest_001");
            var neomethod = testtool.GetNEOVMMethod(ilmethod);
            DumpAVM(neomethod);
            var bytes = testtool.NeoMethodToBytes(neomethod);
            DumpBytes(bytes);
        }
    }
}
