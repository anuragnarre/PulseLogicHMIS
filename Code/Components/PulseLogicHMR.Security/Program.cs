using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Data.SqlClient;


namespace PulseLogicHMR.Security
{
    class Program
    {
        static string connStr = ConfigurationManager.ConnectionStrings["RBAC_Connection"].ConnectionString;

        public static void Main(string[] args)
        {
            TestRoutes();
             
        }


        static void TestRoutes()
        {

            RbacDbContext dbContext = new RbacDbContext(connStr);

            List<PulseLogicRoute> rbacUserRoutes = RBAC.GetRoutesForUser(11, false);

            //below works fine..
            //List<RbacUser> allUsers = dbContext.Users.ToList();
            //List<RbacApplication> applications = dbContext.Applications.ToList();
            //List<RbacPermission> permissions = dbContext.Permissions.ToList();
            //List<RbacRole> roles = dbContext.Roles.ToList();
            //List<PulseLogicRoute> routes = dbContext.Routes.ToList();
            //List<UserRoleMap> userrolemaps = dbContext.UserRoleMaps.ToList();
            //List<RolePermissionMap> rolePermMaps = dbContext.RolePermissionMaps.ToList();
        }

        public static List<PulseLogicRoute> GetAllRoutes()
        {
            List<PulseLogicRoute> retList = new List<PulseLogicRoute>();
            retList.Add(new PulseLogicRoute() { DisplayName = "Dashboard", RouteId = 1, ParentRouteId = null });
            retList.Add(new PulseLogicRoute() { DisplayName = "Appointment", RouteId = 1, ParentRouteId = null });
            retList.Add(new PulseLogicRoute() { DisplayName = "Clinical", RouteId = 1, ParentRouteId = null });


            return retList;
        }

    }
}

