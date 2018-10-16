using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroCollectNew.Models.DataModel;

namespace DataAccess.EntityFramework
{
    public class RetroCollectNewContext : DbContext
    {
        public RetroCollectNewContext (DbContextOptions<RetroCollectNewContext> options)
            : base(options)
        {
        }

        public RetroCollectNewContext()            {
        }

        public DbSet<RetroCollectNew.Models.DataModel.GameListModel> GameListModel { get; set; }

        public DbSet<RetroCollectNew.Models.DataModel.ClientListModel> ClientListModel { get; set; }
    }
}
