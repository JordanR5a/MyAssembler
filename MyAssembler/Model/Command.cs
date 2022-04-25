using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MyAssembler.Model
{
    public abstract class Command
    {
        //TODO: binary replaced with symbol letter, cond AL can be assumed when not present, and all cond are flesh with command
        protected static Dictionary<string, bool[]> Conditionals = new Dictionary<string, bool[]>()
        {
            { "AL", new bool[] { true, true, true, false } },
            { "PL", new bool[] { false, true, false, true } }
        };

        //https://stackoverflow.com/questions/24322417/how-to-convert-bool-array-in-one-byte-and-later-convert-back-in-bool-array
        protected static byte ToByte(bool[] bits)
        {
            byte result = 0;
            // This assumes the array never contains more than 8 elements!
            int index = 8 - bits.Length;

            // Loop through the array
            foreach (bool b in bits)
            {
                // if the element is 'true' set the bit at that position
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }

        protected static string ShowWork(string type, byte[] array)
        {
            var str = new StringBuilder($"{type}: ");
            foreach (var b in array) str.Append(Convert.ToHexString(new[] { b })).Append(", ");
            str.Remove(str.ToString().LastIndexOf(","), 2);
            return str.ToString();
        }

        public abstract string Type { get; }
        public abstract byte[] Build(string context, bool debug);
    }
}
