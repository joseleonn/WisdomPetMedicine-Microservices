using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Rescue.Domain.Exceptions
{
    public class InvalidAdopterStateException : Exception
    {
        public InvalidAdopterStateException(string message) : base(message)
        {
        }
    }
}
