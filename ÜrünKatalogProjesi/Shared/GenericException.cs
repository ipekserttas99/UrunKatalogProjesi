using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Shared
{
    public class GenericException
    {
        public GenericException()
        {
        }

        public ApplicationResult GetError(Exception ex)
        {
           
            return new ApplicationResult { Succeeded = false, ErrorMessage = "No record found." };
        }
    }
}
