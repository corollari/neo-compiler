﻿using Neo.SmartContract.Framework;

namespace net4smartcontract.test
{
    public class TestClass1 : SmartContract
    {
        //default smartcontract entry point.
        //but the unittest can be init from anywhere
        //no need to add code in Main.
        public static object Main(string method, object[] args)
        {
            return null;
        }
        public static byte UnitTest_001()
        {
            var nb = new byte[] { 1, 2, 3, 4 };
            return nb[2];
        }
        public static byte UnitTest_002()
        {
            var nb = new byte[] { 1, 2, 3, 4 };
            return nb[2];
        }
        public static object UnitTest_BytesEqual()
        {
            byte[] script = new byte[] { 1, 2, 3 };
            return script == new byte[] { };
        }
        public static object UnitTest_ObjectEqual()
        {
            object script = new byte[] { 1, 2, 3 };
            object script2 = new byte[] { };
            return script == script2;
        }
    }
}
