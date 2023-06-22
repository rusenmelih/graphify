using System;
using System.Collections.Generic;
using System.Text;

namespace MelofyAPI
{
    public class Component<T>
    {
        public string Code { get; set; }
        public T Content { get; set; }
    }
}
