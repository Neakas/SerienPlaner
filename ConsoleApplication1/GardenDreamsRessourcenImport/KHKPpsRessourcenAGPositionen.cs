//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GardenDreamsRessourcenImport
{
    using System;
    using System.Collections.Generic;
    
    public partial class KHKPpsRessourcenAGPositionen
    {
        public int BelAGPosID { get; set; }
        public short Mandant { get; set; }
        public Nullable<int> BelPosID { get; set; }
        public short Position { get; set; }
        public short RessourceTypAg { get; set; }
        public string RessourcenummerAg { get; set; }
        public string Matchcode { get; set; }
        public string Dimensionstext { get; set; }
        public short IstMainAGPos { get; set; }
        public Nullable<decimal> Leistungsgrad { get; set; }
        public string Wartezeittext { get; set; }
        public Nullable<decimal> Wartezeit { get; set; }
        public byte[] Timestamp { get; set; }
    
        public virtual KHKPpsRessourcenPositionen KHKPpsRessourcenPositionen { get; set; }
    }
}
