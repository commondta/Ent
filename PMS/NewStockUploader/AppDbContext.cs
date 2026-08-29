using B_DB_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStockUploader
{
    public class AppDbContext : DbContext
    {
        public DbSet<StockCreation> StockCreations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server =WIN-CM05CUDDJMV; Database=DHA_Live;TrustServerCertificate=True; User Id=sa; Password=s@dm24;MultipleActiveResultSets=True;");
        }
    }
}
