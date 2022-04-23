using MyAssembler.Model;
using System;
using System.IO;

namespace MyAssembler
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] bytes = new byte[5];
            //File.WriteAllBytes("kernal7.img", bytes);


            new Assembler(true).Run("cmds.txt", "kernal7.img");
        }
    }
}
