using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data
{
    public class AuctionDbcontext : DbContext
    {
        public AuctionDbcontext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Entities.Auction> Auctions { get; set; }
    }
}