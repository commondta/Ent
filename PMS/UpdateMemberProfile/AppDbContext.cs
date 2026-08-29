using B_DB_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMemberProfile
{
    public class AppDbContext : DbContext
    {
        public DbSet<MemberProfile> MemberProfile { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server =DESKTOP-7OOOP01\\SQLEXPRESS; Database=UrbanQA;TrustServerCertificate=True; Integrated Security = true;");
        }
    }
}
