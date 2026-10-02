using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseLogicHMR.Jobs
{
    interface IRemoteSync
    {                
        Task SyncSalesToRemoteServer();
        Task SyncSalesReturnToRemoteServer();
    }
}
