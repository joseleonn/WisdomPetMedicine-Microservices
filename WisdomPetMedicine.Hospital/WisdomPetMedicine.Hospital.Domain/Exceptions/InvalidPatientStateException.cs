using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Hospital.Domain.Exceptions
{
    public class InvalidPatientStateException : Exception
    {
        public InvalidPatientStateException(string message) : base(message)
        {
        }
    }
}
