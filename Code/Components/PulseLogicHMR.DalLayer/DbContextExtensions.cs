using Microsoft.EntityFrameworkCore;

namespace System.Data.Entity
{
    public static class DbContextExtensionsSystemEntity
    {
        public static void AddAuditCustomField(this DbContext context, string name, object value)
        {
        }
    }
}

namespace PulseLogicHMR.DalLayer
{
    using Microsoft.EntityFrameworkCore;

    public static class DbContextExtensionsDalLayer
    {
        public static void AddAuditCustomField(this DbContext context, string name, object value)
        {
        }
    }
}

namespace PulseLogicHMR.Controllers
{
    using Microsoft.EntityFrameworkCore;

    public static class DbContextExtensionsControllers
    {
        public static void AddAuditCustomField(this DbContext context, string name, object value)
        {
        }
    }
}
