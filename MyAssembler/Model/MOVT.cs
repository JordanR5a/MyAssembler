using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssembler.Model
{
    public class MOVT : Command
    {
        public override string Type => "MOVT";

        public override byte[] Build(string context)
        {
            var result = new byte[4];

            var cond = context.Split(" ", 2)[0];
            context = context.Replace(cond, "").Trim();

            BitArray bitArray;

            var rd = context.Split(",", 2)[0];
            bitArray = new BitArray(new[] { int.Parse(rd) });
            bitArray.Length = 4;
            var destinationReg = new bool[4];
            bitArray.CopyTo(destinationReg, 0);
            destinationReg = destinationReg.Reverse().ToArray();
            context = context.Replace(rd, "").Trim();

            var imm16 = context.Substring(context.IndexOf("#") + 1);
            var bytes = ConvertHexStringToByteArray(imm16);
            bitArray = new BitArray(bytes);
            var bits = new bool[16];
            bitArray.CopyTo(bits, 0);


            var const1 = new bool[] { false, false, true, true };
            var const2 = new bool[] { false, true, false, false };

            result[0] = ToByte(Conditionals[cond].Concat(const1).ToArray());
            result[1] = ToByte(const2.Concat(bits.Take(4)).ToArray());
            result[2] = ToByte(destinationReg.Concat(bits.Skip(4).Take(4)).ToArray());
            result[3] = ToByte(bits.Skip(8).Take(8).ToArray());

            return result;
        }
    }
}
