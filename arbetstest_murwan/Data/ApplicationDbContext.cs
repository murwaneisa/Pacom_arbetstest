using arbetstest_murwan.Models;
using Microsoft.EntityFrameworkCore;

namespace arbetstest_murwan.Data
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Database context for the Modbus Simulator application
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        /// <summary>
        /// Gets or sets the Modbus values in the database
        /// </summary>
        public DbSet<ModbusValue> ModbusValues { get; set; }
    }
}
