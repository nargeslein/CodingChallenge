using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge
{
    [Serializable]
    public class ConsolidationException : Exception
    {
        public ConsolidationException(string message):base(message)
        {

        }  
    }
}
