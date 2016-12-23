﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class OLEntities : DbContext
    {
        public OLEntities()
            : base("name=OLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<KHKArtikel> KHKArtikel { get; set; }
        public virtual DbSet<KHKPpsArbeitsgaenge> KHKPpsArbeitsgaenge { get; set; }
        public virtual DbSet<KHKPpsArbeitsplaetze> KHKPpsArbeitsplaetze { get; set; }
        public virtual DbSet<KHKPpsRessourcenAGPositionen> KHKPpsRessourcenAGPositionen { get; set; }
        public virtual DbSet<KHKPpsRessourcenkopf> KHKPpsRessourcenkopf { get; set; }
        public virtual DbSet<KHKPpsRessourcenPositionen> KHKPpsRessourcenPositionen { get; set; }
        public virtual DbSet<amxImportRL> amxImportRL { get; set; }
        public virtual DbSet<KHKTan> KHKTan { get; set; }
    
        public virtual int amxSpCopyArtikel(Nullable<short> mandant, string artikelnummerNew, string artikelnummerCopyFrom)
        {
            var mandantParameter = mandant.HasValue ?
                new ObjectParameter("Mandant", mandant) :
                new ObjectParameter("Mandant", typeof(short));
    
            var artikelnummerNewParameter = artikelnummerNew != null ?
                new ObjectParameter("ArtikelnummerNew", artikelnummerNew) :
                new ObjectParameter("ArtikelnummerNew", typeof(string));
    
            var artikelnummerCopyFromParameter = artikelnummerCopyFrom != null ?
                new ObjectParameter("ArtikelnummerCopyFrom", artikelnummerCopyFrom) :
                new ObjectParameter("ArtikelnummerCopyFrom", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("amxSpCopyArtikel", mandantParameter, artikelnummerNewParameter, artikelnummerCopyFromParameter);
        }
    }
}