//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPFClient.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class Accessory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Accessory()
        {
            this.AccessoriesToBoat = new HashSet<AccessoriesToBoat>();
            this.OrderDetails = new HashSet<OrderDetails>();
        }
    
        public int Accessory_ID { get; set; }
        public string AccName { get; set; }
        public string DescriptionOfAccessory { get; set; }
        public string Price { get; set; }
        public string VAT { get; set; }
        public int Inventory { get; set; }
        public int OrderLevel { get; set; }
        public int OrderBatch { get; set; }
        public int Partner_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessoriesToBoat> AccessoriesToBoat { get; set; }
        public virtual Partner Partner { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
