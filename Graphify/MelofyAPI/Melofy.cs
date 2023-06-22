using System;
using System.Collections.Generic;
using System.Text;

namespace MelofyAPI
{
    public class Melofy
    {
        private MelofyFn melofyFn;
        private string errMsg;

        public Melofy(MelofyFn melofyFn, string errMsg)
        {
            this.melofyFn = melofyFn;
            this.errMsg = errMsg;
        }

        public MelofyFn MelofyFn
        {
            get { return this.melofyFn; }
        }
        public string ErrorMsg
        {
            get { return this.errMsg; }
        }
    }
}
