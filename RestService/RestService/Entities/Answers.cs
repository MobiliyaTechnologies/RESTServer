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
    
    public partial class Answers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Answers()
        {
            this.Feedback = new HashSet<Feedback>();
        }
    
        public int AnswerID { get; set; }
        public string AnswerDesc { get; set; }
        public Nullable<int> QuestionID { get; set; }
    
        public virtual Questions Questions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedback { get; set; }
    }
}
