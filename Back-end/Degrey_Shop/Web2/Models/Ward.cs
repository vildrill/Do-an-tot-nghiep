//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ward
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Ward_Code { get; set; }
        public Nullable<long> District_Id { get; set; }
        public Nullable<long> City_Id { get; set; }
    }
}
