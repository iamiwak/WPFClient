//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPFClient.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class Readings
    {
        public int IdReading { get; set; }
        public Nullable<int> IdReader { get; set; }
        public Nullable<int> IdCopy { get; set; }
        public Nullable<System.DateTime> BeginDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    
        public virtual Copies Copies { get; set; }
        public virtual Readers Readers { get; set; }
    }
}