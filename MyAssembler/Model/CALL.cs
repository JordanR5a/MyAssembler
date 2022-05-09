using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAssembler.Model
{
    public class CALL : B
    {
        public override string Type => "CALL";

        public override byte[] Build(string context, bool debug)
        {
            int toGo = Subroutines[context];
            int travel = toGo - (line + 2);

            return base.Build($"L #{travel.ToString("X")}", debug);
        }
    }
}
