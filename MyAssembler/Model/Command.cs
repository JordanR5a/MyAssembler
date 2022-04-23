using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MyAssembler.Model
{
    public abstract class Command
    {
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

        //https://stackoverflow.com/questions/61210068/hex-string-to-bool-c-sharp
        protected byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0) hexString.Insert(0, "0");

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        public abstract string Type { get; }
        public abstract byte[] Build(string context);
    }
}
