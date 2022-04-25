using MyAssembler.Model;
using System;
using System.IO;

namespace MyAssembler
{
    class Program
    {
        static void Main(string[] args)
        {
            new Assembler(true).Run("cmds.txt", "kernel7.img");
        }
    }
}
