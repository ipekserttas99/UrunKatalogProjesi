using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Jobs
{
    public interface ISendEmailJob
    {
        public void DoLogInJob(string recipientEmail, string recipientFirstName);
        public void DoRegisterJob(string recipientEmail, string recipientFirstName);
    }
}
