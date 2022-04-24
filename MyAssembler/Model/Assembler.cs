using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyAssembler.Model
{
    public class Assembler
    {
        //If data from prev command needed, give this class static variable which command hildren can use.
        private bool Debug;

        public Assembler(bool debug)
        {
            Debug = debug;
        }

        //https://grabthiscode.com/csharp/c-get-all-child-classes-of-a-class
        Type[] GetInheritedClasses(Type MyType)
        {
            return Assembly.GetAssembly(MyType).GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(MyType)).ToArray();
        }

        public void Run(string cmdFileName, string outputFileName)
        {
            string[] cmds = File.ReadAllLines("../../../Resources/" + cmdFileName).Where(c => c.Length != 0).ToArray();

            List<byte> bytes = new List<byte>();
            foreach (var cmd in cmds)
            {
                foreach (var type in GetInheritedClasses(typeof(Command)))
                {
                    var command = type.GetProperty("Type").GetGetMethod().Invoke(Activator.CreateInstance(type), null).ToString();
                    if (cmd.Split(" ", 2)[0].ToUpper().Equals(command)) bytes.AddRange((byte[])type.GetMethod("Build").Invoke(Activator.CreateInstance(type), new object[] { cmd.Replace(command + " ", ""), Debug }));
                }
            }
            File.WriteAllBytes($"../../../Resources/{outputFileName}", bytes.ToArray());
        }
    }
}
