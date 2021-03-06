using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MyAssembler.Model
{
    public class MOVW : Command
    {
        public override string Type => "MOVW";

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
            context = context.Remove(0, rd.Length);

            var imm16 = context.Substring(context.IndexOf("#") + 1);
            bitArray = new BitArray(new[] { int.Parse(imm16, System.Globalization.NumberStyles.HexNumber) });
            var bits = new bool[16];
            bitArray.Length = 16;
            bitArray.CopyTo(bits, 0);
            bits = bits.Reverse().ToArray();


            var const1 = new bool[] { false, false, true, true };
            var const2 = new bool[] { false, false, false, false };

            result[0] = ToByte(Conditionals[cond].Concat(const1).ToArray());
            result[1] = ToByte(const2.Concat(bits.Take(4)).ToArray());
            result[2] = ToByte(destinationReg.Concat(bits.Skip(4).Take(4)).ToArray());
            result[3] = ToByte(bits.Skip(8).Take(8).ToArray());
            result = result.Reverse().ToArray();

            if (debug) Console.WriteLine(ShowWork(Type, result));
            return result;
        }
    }
}
