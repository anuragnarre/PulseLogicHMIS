using PulseLogicHMR.Core;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel.IMUDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Services.IMU
{
    public interface IIMUService
    {
        Task <DataTable> GetAllImuTestData(LabDbContext labDbcontext, DateTime fromDate , DateTime toDate);
        Task<IMUResponseModel> PostDataToIMU(LabDbContext labDbContex,CoreDbContext coreDbContext, List<Int64> reqIdList, RbacUser user);
    }
}
