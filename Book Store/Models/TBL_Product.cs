//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Book_Store.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBL_Product()
        {
            this.TBL_Comments = new HashSet<TBL_Comments>();
            this.TBL_Orderdetail = new HashSet<TBL_Orderdetail>();
        }
    
        public int Product_ID { get; set; }
        public Nullable<int> Code { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<int> Weight { get; set; }
        public int Height { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Lenght { get; set; }
        public string Cover { get; set; }
        public Nullable<int> Pages { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public Nullable<int> Category_ID { get; set; }
        public string Meta_Title { get; set; }
        public string Summery { get; set; }
        public Nullable<int> Discount_ID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Writer_ID { get; set; }
        public Nullable<int> Shabak { get; set; }
        public Nullable<int> Entersharat { get; set; }
        public Nullable<int> Motarjem { get; set; }
        public Nullable<int> ChapYear { get; set; }
        public Nullable<int> ShomareChap { get; set; }
    
        public virtual TBL_Category TBL_Category { get; set; }
        public virtual TBL_Entesharat TBL_Entesharat { get; set; }
        public virtual TBL_Translator TBL_Translator { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_Comments> TBL_Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBL_Orderdetail> TBL_Orderdetail { get; set; }
        public virtual TBL_Writer TBL_Writer { get; set; }
    }
}
