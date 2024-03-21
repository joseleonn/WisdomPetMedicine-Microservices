using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Pet.Domain.Exceptions
{
    public class InvalidPetStateException : Exception
    {
        public InvalidPetStateException(string message) : base(message)
        {
        }
    }
}
