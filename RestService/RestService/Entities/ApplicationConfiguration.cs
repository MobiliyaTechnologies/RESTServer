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
    
    public partial class ApplicationConfiguration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApplicationConfiguration()
        {
            this.ApplicationConfigurationEntry = new HashSet<ApplicationConfigurationEntry>();
        }
    
        public int Id { get; set; }
        public string ConfigurationType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationConfigurationEntry> ApplicationConfigurationEntry { get; set; }
    }
}