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
        private static string SUB_RETURN = "BX 14";
        private bool Debug;

        public Assembler(bool debug)
        {
            Debug = debug;
        }

        //https://grabthiscode.com/csharp/c-get-all-child-classes-of-a-class
        Type[] GetInheritedClasses(Type MyType)
        {
            return Assembly.GetAssembly(MyType).GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(MyType)).OrderByDescending(x => x.Name.Length).ToArray();
        }

        Dictionary<string, int> GetSubroutines(string[] cmds)
        {
            var subs = new Dictionary<string, int>();
            for (int i = 0; i < cmds.Length; i++)
            {
                if (cmds[i].Contains(':'))
                {
                    subs.Add(cmds[i].Split(':')[0], i);

                    i++;
                    while (i < cmds.Length && cmds[i].Contains("\t"))
                    {
                        cmds[i - 1] =  cmds[i].Split('\t')[1];
                        i++;
                    }
                    cmds[--i] = SUB_RETURN;

                }
            }
            return subs;
        }

        public void Run(string cmdFileName, string outputFileName)
        {
            string[] cmds = File.ReadAllLines("../../../Resources/" + cmdFileName).Where(c => c.Length != 0).ToArray();

            Command.Subroutines = GetSubroutines(cmds);
            List<byte> bytes = new List<byte>();
            int line = 0;
            foreach (var cmd in cmds)
            {
                foreach (var type in GetInheritedClasses(typeof(Command)))
                {
                    var instance = (Command)Activator.CreateInstance(type);
                    instance.line = line;
                    var command = type.GetProperty("Type").GetGetMethod().Invoke(instance, null).ToString();
                    if (cmd.Split(" ", 2)[0].ToUpper().StartsWith(command))
                    {
                        bytes.AddRange((byte[])type.GetMethod("Build").Invoke(instance, new object[] { cmd.Replace(command, "").Trim(), Debug }));
                        break;
                    }
                }
                line++;
            }
            File.WriteAllBytes($"../../../Resources/{outputFileName}", bytes.ToArray());
        }
    }
}
