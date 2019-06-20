using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace ModelData
{
    public class RetroCollectNewContext : DbContext
    {
        public RetroCollectNewContext (DbContextOptions<RetroCollectNewContext> options)
            : base(options)
        {
        }

        public RetroCollectNewContext()            {
        }


        public DbSet<ClientListModel> ClientListModel { get; set; }
    }
}
