using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Security;
using PulseLogicHMR.Services.ProcessConfirmation.DTO;

namespace PulseLogicHMR.Services.ProcessConfirmation
{
    public interface IProcessConfirmationService
    {
        object ConfirmProcess(ProcessConfirmationUserCredentials_DTO processConfirmationUserCredentials);
    }
}
