using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAssembler.Model
{
    public class B : Command
    {
        public override string Type => "B";

        public override byte[] Build(string context, bool debug)
        {
            var result = new byte[4];

            bool[] cond = Conditionals["AL"];
            bool l = false;
            if (context.Contains(" "))
            {
                if (context.ToUpper().StartsWith("L"))
                {
                    l = true;
                    context = context.Remove(0, 1);
                }

                foreach (var con in Conditionals)
                    if (context.StartsWith(con.Key))
                    {
                        cond = con.Value;
                        context = context.Remove(0, con.Key.Length);
                    }
                context = context.Trim();
            }

            BitArray bitArray;
            var hex = context.Substring(context.IndexOf("#") + 1);
            bitArray = new BitArray(new[] { int.Parse(hex, System.Globalization.NumberStyles.HexNumber) });
            var offset = new bool[24];
            bitArray.Length = 24;
            bitArray.CopyTo(offset, 0);
            offset = offset.Reverse().ToArray();

            var const1 = new bool[] { true, false, true };

            result[0] = ToByte(cond.Concat(const1).Concat(new[] { l }).ToArray());
            result[1] = ToByte(offset.Take(8).ToArray());
            result[2] = ToByte(offset.Skip(8).Take(8).ToArray());
            result[3] = ToByte(offset.Skip(16).Take(8).ToArray());
            result = result.Reverse().ToArray();

            if (debug) Console.WriteLine(ShowWork(Type, result));
            return result;
        }
    }
}
