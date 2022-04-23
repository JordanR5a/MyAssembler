using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyAssembler.Model
{
    public class ORR : Command
    {
        public override string Type => "ORR";

        public override byte[] Build(string context, bool debug)
        {
            var result = new byte[4];

            var preParams = context.Split(" ", 2)[0];
            var cond = preParams.Substring(0, preParams.Length - 1);
            var s = int.Parse(preParams.Last().ToString());
            context = context.Remove(0, preParams.Length);

            BitArray bitArray;

            var rn = context.Split(",", 2)[0];
            bitArray = new BitArray(new[] { int.Parse(rn) });
            bitArray.Length = 4;
            var operandReg = new bool[4];
            bitArray.CopyTo(operandReg, 0);
            operandReg = operandReg.Reverse().ToArray();
            context = context.Remove(0, rn.Length + 1);

            var rd = context.Split(",", 2)[0];
            bitArray = new BitArray(new[] { int.Parse(rd) });
            bitArray.Length = 4;
            var destinationReg = new bool[4];
            bitArray.CopyTo(destinationReg, 0);
            destinationReg = destinationReg.Reverse().ToArray();
            context = context.Remove(0, rd.Length);

            var op = context.Substring(context.IndexOf("#") + 1);
            bitArray = new BitArray(new[] { int.Parse(op, System.Globalization.NumberStyles.HexNumber) });
            var operand = new bool[12];
            bitArray.Length = 12;
            bitArray.CopyTo(operand, 0);
            operand = operand.Reverse().ToArray();


            var const1 = new bool[] { false, false, true, true };
            var const2 = new bool[] { true, false, false };

            result[0] = ToByte(Conditionals[cond].Concat(const1).ToArray());
            result[1] = ToByte(const2.Concat(new[] { Convert.ToBoolean(s) }).Concat(destinationReg).ToArray());
            result[2] = ToByte(operandReg.Concat(operand.Take(4)).ToArray());
            result[3] = ToByte(operand.Skip(4).Take(8).ToArray());
            result = result.Reverse().ToArray();

            if (debug) Console.WriteLine(ShowWork(Type, result));
            return result;
        }
    }
}
