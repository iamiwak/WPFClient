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
    
    public partial class Customers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customers()
        {
            this.Order = new HashSet<Order>();
        }
    
        public int Customer_ID { get; set; }
        public string FistName { get; set; }
        public string FamilyName { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string OrganisationName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string email { get; set; }
        public string Phone { get; set; }
        public string IDNumber { get; set; }
        public string IDDocumentName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
    }
}