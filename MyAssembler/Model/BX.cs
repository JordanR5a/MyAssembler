using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAssembler.Model
{
    public class BX : Command
    {
        public override string Type => "BX";

        public override byte[] Build(string context, bool debug)
        {
            var result = new byte[4];

            bool[] cond = Conditionals["AL"];
            if (context.Contains(" "))
            {

                foreach (var con in Conditionals)
                    if (context.StartsWith(con.Key))
                    {
                        cond = con.Value;
                        context = context.Remove(0, con.Key.Length);
                    }
                context = context.Trim();
            }

            BitArray bitArray;

            var rn = context;
            bitArray = new BitArray(new[] { int.Parse(rn) });
            bitArray.Length = 4;
            var operandReg = new bool[4];
            bitArray.CopyTo(operandReg, 0);
            operandReg = operandReg.Reverse().ToArray();

            var constant = new bool[] { 
                                        false, false, false, true, 
                                        false, false, true, false, 
                                        true, true, true, true,
                                        true, true, true, true,
                                        true, true, true, true,
                                        false, false, false, true
                                      };

            result[0] = ToByte(cond.Concat(constant.Take(4)).ToArray());
            result[1] = ToByte(constant.Skip(4).Take(8).ToArray());
            result[2] = ToByte(constant.Skip(12).Take(8).ToArray());
            result[3] = ToByte(constant.Skip(20).Take(4).Concat(operandReg).ToArray());
            result = result.Reverse().ToArray();

            if (debug) Console.WriteLine(ShowWork(Type, result));
            return result;
        }
    }
}
