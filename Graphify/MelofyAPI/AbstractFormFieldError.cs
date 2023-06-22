using System;
using System.Collections.Generic;
using System.Text;

namespace MelofyAPI
{
    public class AbstractFormFieldError : Exception
    {
        public AbstractFormFieldError(string message) : base(message)
        {

        }
    }
}
