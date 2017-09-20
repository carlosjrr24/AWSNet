using AWSNet.Model.Base; //revisar x q no esta en publishing stattus
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AWSNet.Model.Entities
{
    public class Products : Entity
    {
        public int      ID { get; set; }
        public string   Name { get; set; }
        public int      CategoryID { get; set; }
        public decimal  UnitPrice { get; set; }
        public int      UnitsStock { get; set; }
        public bool     IsDeleted { get; set; }
        public DateTime CreationDate { get; set; }
        public Nullable<DateTime> LastModificationDate { get; set; }


        public Products()
        {
            ID = 0;
            Name = string.Empty;
            CategoryID = 0;
            UnitPrice = 0;
            UnitsStock = 0;

            IsDeleted = false;

            CreationDate = DateTime.UtcNow;
            LastModificationDate = DateTime.UtcNow;
        }

        public void Delete()
        {
            this.IsDeleted = true;
            this.LastModificationDate = DateTime.UtcNow;
        }

        public void Restore()
        {
            this.IsDeleted = false;
            this.LastModificationDate = DateTime.UtcNow;
        }
    }
}
