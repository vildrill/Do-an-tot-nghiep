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
    
    public partial class Discount
    {
        public int Id_Discount { get; set; }
        public string Name { get; set; }
        public Nullable<double> Percent { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> End_Date { get; set; }
        public Nullable<int> Status { get; set; }
        public string Photo { get; set; }
        public Nullable<bool> Delete_Stt { get; set; }
    }
}