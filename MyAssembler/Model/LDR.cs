using System;
using System.Collections.Generic;
using System.Text;

namespace MyAssembler.Model
{
    public class LDR : Command
    {
        public override string Type => "LDR";

        public override byte[] Build(string context, bool debug)
        {
            return new byte[0];
        }

    }
}
