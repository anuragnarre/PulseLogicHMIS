using PulseLogicHMR.ServerModel;
using System.Collections.Generic;

namespace PulseLogicHMR.Services
{
    public interface IVerificationService
    {
        int CreateVerification(CreateVerificationDto dto);
        List<VerificationViewModel> GetVerificationViewModel(int VerificationId);
        int UpdateVerifcation(int VerificationId, int EmployeeId, string VerificationStatus);
    }
}
