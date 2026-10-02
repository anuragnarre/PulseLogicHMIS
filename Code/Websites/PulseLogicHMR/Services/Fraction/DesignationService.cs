using System;
using System.Collections.Generic;
using System.Linq;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Core.Configuration;
using Microsoft.Extensions.Options;
using PulseLogicHMR.ServerModel;

namespace PulseLogicHMR.Services
{
    public class DesignationService : IDesignationService
    {
        public FractionDbContext db;
        private readonly string connString = null;

        public DesignationService(IOptions<MyConfiguration> _config)
        {
            connString = _config.Value.Connectionstring;
            db = new FractionDbContext(connString);
        }

        public List<DesignationModel> ListDesignation(int pageNumber = 1, int pageSize = 10)
        {
            pageNumber = Math.Max(1, pageNumber);
            int skip = (pageNumber - 1) * pageSize;
            var query = db.Designation.OrderBy(d => d.DesignationId).Skip(skip).Take(pageSize).ToList();
            return query;
        }

        public DesignationModel AddDesignation(DesignationModel model)
        {
            model.CreatedOn = DateTime.Now;
            db.Designation.Add(model);
            db.SaveChanges();
            return model;
        }

        public DesignationModel UpdateDesignation(DesignationModel model)
        {
            db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return model;
        }

        public DesignationModel GetDesignation(int id)
        {
            var result = db.Designation.Where(x => x.DesignationId == id).FirstOrDefault();      
            return result;
        }       
    }
}

