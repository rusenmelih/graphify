using System;
using System.Collections.Generic;
using System.Text;

namespace MelofyAPI
{
    public abstract class AbstractFormField : AbstractFormControl
    {
        protected string code;
        protected MessageResult message = null;

        public AbstractFormField(string key)
            : base(key)
        {

        }

        public MessageResult ErrorMsg
        {
            get { return this.message; }
        }
    }
}
