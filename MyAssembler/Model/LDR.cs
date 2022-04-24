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

            var preParams = context.Split(" ", 2)[0];
            var cond = preParams.Substring(0, preParams.Length - 6);
            preParams = preParams.Remove(0, cond.Length);
            var i = int.Parse(preParams[0].ToString());
            var p = int.Parse(preParams[1].ToString());
            var u = int.Parse(preParams[2].ToString());
            var b = int.Parse(preParams[3].ToString());
            var w = int.Parse(preParams[4].ToString());
            var l = int.Parse(preParams[5].ToString());
            context = context.Remove(0, cond.Length + preParams.Length);

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

            var const1 = new bool[] { false, true };
            var offset = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };

            result[0] = ToByte(Conditionals[cond].Concat(const1).Concat(new[] { Convert.ToBoolean(i) }).Concat(new[] { Convert.ToBoolean(p) }).ToArray());
            result[1] = ToByte(new[] { Convert.ToBoolean(u) }.Concat(new[] { Convert.ToBoolean(b) }).Concat(new[] { Convert.ToBoolean(w) }).Concat(new[] { Convert.ToBoolean(l) }).Concat(baseRegister).ToArray());
            result[2] = ToByte(destinationReg.Concat(offset.Take(4)).ToArray());
            result[3] = ToByte(offset.Skip(4).Take(8).ToArray());
            result = result.Reverse().ToArray();

            if (debug) Console.WriteLine(ShowWork(Type, result));
            return result;
        }

    }
}
