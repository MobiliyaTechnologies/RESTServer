//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestService.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class RoomDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoomDetail()
        {
            this.Feedback = new HashSet<Feedback>();
        }
    
        public int Room_Id { get; set; }
        public string Room_Name { get; set; }
        public string Room_Desc { get; set; }
        public string Building { get; set; }
        public string Breaker_details { get; set; }
        public Nullable<double> X { get; set; }
        public Nullable<double> Y { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedback { get; set; }
    }
}