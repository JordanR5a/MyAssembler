using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAssembler.Model
{
    public class STR : Command
    {
        public override string Type => "STR";

        public override byte[] Build(string context, bool debug)
        {
            var result = new byte[4];

            bool[] cond = Conditionals["AL"];
            var const1 = new bool[] { false, true, false, false, false, false, false, false };
            if (context.Contains(" "))
            {
                if (context.ToUpper().StartsWith("EA"))
                {
                    const1 = new bool[] { false, true, false, false, true, false, false, false };
                    context = context.Remove(0, 2);
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

            var rd = context.Split(",", 2)[0];
            if (rd.Contains("!"))
            {
                const1[6] = true;
                bitArray = new BitArray(new[] { int.Parse(rd.Substring(0, rd.Length - 1)) });
            }
            else bitArray = new BitArray(new[] { int.Parse(rd) });
            bitArray.Length = 4;
            var destinationRegister = new bool[4];
            bitArray.CopyTo(destinationRegister, 0);
            destinationRegister = destinationRegister.Reverse().ToArray();
            context = context.Remove(0, rd.Length + 1);

            var rn = context.Split(",", 2)[0];
            bitArray = new BitArray(new[] { int.Parse(rn) });
            bitArray.Length = 4;
            var baseRegister = new bool[4];
            bitArray.CopyTo(baseRegister, 0);
            baseRegister = baseRegister.Reverse().ToArray();
            context = context.Remove(0, rn.Length);

            var offset = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false };
            if (context.Contains("#"))
            {
                var hex = context.Substring(context.IndexOf("#") + 1);
                bitArray = new BitArray(new[] { int.Parse(hex, System.Globalization.NumberStyles.HexNumber) });
                offset = new bool[12];
                bitArray.Length = 12;
                bitArray.CopyTo(offset, 0);
                offset = offset.Reverse().ToArray();
            }

            result[0] = ToByte(cond.Concat(const1.Take(4)).ToArray());
            result[1] = ToByte(const1.Skip(4).Take(4).Concat(baseRegister).ToArray());
            result[2] = ToByte(destinationRegister.Concat(offset.Take(4)).ToArray());
            result[3] = ToByte(offset.Skip(4).Take(8).ToArray());
            result = result.Reverse().ToArray();

            if (debug) Console.WriteLine(ShowWork(Type, result));
            return result;
        }
    }
}
