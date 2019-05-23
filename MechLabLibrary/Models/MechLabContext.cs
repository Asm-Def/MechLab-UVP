using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MechLabLibrary.Models {
    public class MechLabContext : DbContext
    {
        public DbSet<MechLabData> Labs { get; set; }
        public DbSet<MechObjectData> Objects { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=labs.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MechLabData>().HasKey(t => t.LabID);
            modelBuilder.Entity<MechObjectData>().HasKey(t => new { t.LabID, t.ObjectID });
        }
    }
    public class MechLabData
    {
        public Guid LabID { get; set; }
        public double ViewX { get; set; }
        public double ViewY { get; set; }
        public double Eyeshot { get; set; }
        public DateTime ModifiedTime { get; set; }
        public Byte[] Image { get; set; }
    }

    public class MechObjectData
    {
        public Guid LabID { get; set; }
        public int ObjectID { get; set; }
        /// <summary>
        /// ="Object"表示质点；="Planet"表示星球
        /// </summary>
        public string Type { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double VX { get; set; }
        public double VY { get; set; }
        public double M { get; set; }
        public double? R { get; set; }
        public MechObjectData(Guid ID, int objID, string tp, double x, double y, double vx, double vy, double m)
        {
            LabID = ID;
            ObjectID = objID;
            Type = tp;
            X = x;
            Y = y;
            VX = vx;
            VY = vy;
            M = m;
        }
        public MechObjectData(Guid ID, int objID, string tp, double x, double y, double vx, double vy, double m, double r) : this(ID, objID, tp, x, y, vx, vy, m)
        {
            R = r;
        }
    }
}
