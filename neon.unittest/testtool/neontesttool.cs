using Neo.Compiler;
using Neo.Compiler.MSIL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace neon.unittest.testtool
{
    class DefLogger : ILogger
    {
        public void Log(string log)
        {
            Console.WriteLine(log);
        }
    }
    class neontesttool
    {
        public neontesttool(string dll)
        {
            this.Init(dll);
        }
        ILModule modIL;
        ModuleConverter converterIL;
        void Init(string filename)
        {
            string onlyname = System.IO.Path.GetFileNameWithoutExtension(filename);
            string filepdb = onlyname + ".pdb";
            var path = Path.GetDirectoryName(filename);
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    Directory.SetCurrentDirectory(path);
                }
                catch (Exception err)
                {
                    Console.WriteLine("Could not find path: " + path);
                    throw (err);
                }
            }
            var log = new DefLogger();
            this.modIL = new ILModule(log);
            System.IO.Stream fs = null;
            System.IO.Stream fspdb = null;

            //open file
            try
            {
                fs = System.IO.File.OpenRead(filename);

                if (System.IO.File.Exists(filepdb))
                {
                    fspdb = System.IO.File.OpenRead(filepdb);
                }

            }
            catch (Exception err)
            {
                log.Log("Open File Error:" + err.ToString());
                throw err;
            }
            //load module
            try
            {
                modIL.LoadModule(fs, fspdb);
            }
            catch (Exception err)
            {
                log.Log("LoadModule Error:" + err.ToString());
                throw err;
            }

            converterIL = new ModuleConverter(log);
            ConvOption option = new ConvOption()
            {
                useNep8 = true,
                useSysCallInteropHash = true
            };
            try
            {
                converterIL.Convert(modIL, option);
            }
            catch (Exception err)
            {
                log.Log("Convert IL->ASM Error:" + err.ToString());
                throw err;
            }
        }
        public string[] GetAllILFunction()
        {
            List<string> lists = new List<string>();
            foreach (var _class in modIL.mapType)
            {
                foreach (var method in _class.Value.methods)
                {
                    var name = method.Key;
                    lists.Add(name);
                }
            }
            return lists.ToArray();
        }
        public ILMethod FindMethod(string fromclass, string method)
        {
            foreach (var _class in modIL.mapType)
            {
                var indexbegin = _class.Key.LastIndexOf(".");
                var classname = _class.Key;
                if (indexbegin > 0)
                    classname = classname.Substring(indexbegin + 1);

                if (classname == fromclass)
                {
                    foreach (var _method in _class.Value.methods)
                    {
                        var indexmethodname = _method.Key.LastIndexOf("::");
                        var methodname = _method.Key.Substring(indexmethodname + 2);
                        var indexparams = methodname.IndexOf("(");
                        if (indexparams > 0)
                        {
                            methodname = methodname.Substring(0, indexparams);
                        }
                        if (methodname == method)
                            return _method.Value;
                    }
                }
            }
            return null;
        }
        public string GetFullMethodName(string fromclass, string method)
        {
            foreach (var _class in modIL.mapType)
            {
                var indexbegin = _class.Key.LastIndexOf("::");
                var classname = _class.Key.Substring(indexbegin + 2);
                if (classname == fromclass)
                {
                    foreach (var _method in _class.Value.methods)
                    {
                        var indexmethodname = _method.Key.LastIndexOf("::");
                        var methodname = _method.Key.Substring(indexmethodname + 2);
                        if (methodname == method)
                            return _method.Key;
                    }
                }
            }
            return null;
        }
        public NeoMethod GetNEOVMMethod(ILMethod method)
        {
            var neomethod = this.converterIL.methodLink[method];
            return neomethod;
        }
        public byte[] NeoMethodToBytes(NeoMethod method)
        {
            List<byte> bytes = new List<byte>();
            foreach (var c in method.body_Codes.Values)
            {
                bytes.Add((byte)c.code);
                if (c.bytes != null)
                    for (var i = 0; i < c.bytes.Length; i++)
                    {
                        bytes.Add(c.bytes[i]);
                    }
            }
            return bytes.ToArray();
        }
    }
}
