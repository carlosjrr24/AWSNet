using System.Data.Entity;
using Microsoft;
using AWSNet.Model.Entities;

namespace AWSNet.Model
{
    public class ModelContext : DbContext
    {
        public virtual DbSet<Product> Product { get; set; }

        public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        { }
    }
}
