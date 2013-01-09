using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Familiada
{
    class RoundData
    {
        public Question[] normal;
        public Question[] final;

        public RoundData()
        {
            normal = new Question[5];
            final = new Question[5];
        }
    }
}
