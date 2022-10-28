using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCmd.Lib.ExceptionCollection
{
    [Serializable]
    public class CollectionEmptyException : Exception
    {
        public CollectionEmptyException() { }
        public CollectionEmptyException(string message) : base(message) { }
        public CollectionEmptyException(string message, Exception inner) : base(message, inner) { }
        protected CollectionEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
