
using PulseLogicHMR.CommonTypes;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.Utilities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace PulseLogicHMR.Controllers
{
    public class StickersBL
    {
        public List<PatientStickerModel> GetPatientStickerDetails (PatientDbContext context, int PatientId)
        {
                List<PatientStickerModel> Data = context.Database.SqlQueryRaw<PatientStickerModel>("exec SP_GetPatientStickerDetails @PatientId",
                new SqlParameter("@PatientId", PatientId)).ToList();
                return Data;            
        }

    }
}


