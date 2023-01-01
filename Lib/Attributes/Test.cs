using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class TestAttribute : Attribute
    {
        public string Description { get; set; }


        public TestAttribute()
        {

        }

        public TestAttribute(string description)
        {
            Description = description;
        }

    }
}
