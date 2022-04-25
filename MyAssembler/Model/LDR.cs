using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssembler.Model
{
    public class LDR : Command
    {
        public override string Type => "LDR";

        public override byte[] Build(string context, bool debug)
        {
            var result = new byte[4];

            string cond;
            if (context.Contains(" "))
            {
                cond = context.Split(" ", 2)[0];
                context = context.Remove(0, cond.Length);
            }
            else cond = "AL";

            BitArray bitArray;

            var rd = context.Split(",", 2)[0];
            bitArray = new BitArray(new[] { int.Parse(rd) });
            bitArray.Length = 4;
            var destinationReg = new bool[4];
            bitArray.CopyTo(destinationReg, 0);
            destinationReg = destinationReg.Reverse().ToArray();
            context = context.Remove(0, rd.Length + 1);

            var rn = context.Split(",", 2)[0];
            bitArray = new BitArray(new[] { int.Parse(rn) });
            bitArray.Length = 4;
            var baseRegister = new bool[4];
            bitArray.CopyTo(baseRegister, 0);
            baseRegister = baseRegister.Reverse().ToArray();

            var const1 = new bool[] { false, true, false, false, false, false, false, true };
            var offset = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };

            result[0] = ToByte(Conditionals[cond].Concat(const1.Take(4)).ToArray());
            result[1] = ToByte(const1.Skip(4).Take(4).Concat(baseRegister).ToArray());
            result[2] = ToByte(destinationReg.Concat(offset.Take(4)).ToArray());
            result[3] = ToByte(offset.Skip(4).Take(8).ToArray());
            result = result.Reverse().ToArray();

            if (debug) Console.WriteLine(ShowWork(Type, result));
            return result;
        }

    }
}
