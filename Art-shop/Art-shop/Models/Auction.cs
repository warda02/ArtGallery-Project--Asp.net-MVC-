//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Art_shop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Auction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Auction()
        {
            this.Bids = new HashSet<Bid>();
        }
    
        public int AuctionID { get; set; }
        public int ArtworkID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal StartingBid { get; set; }
        public Nullable<decimal> CurrentBid { get; set; }
        public Nullable<int> ArtistID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual Artist Artist { get; set; }
        public virtual Artwork Artwork { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual user_info user_info { get; set; }
    }
}
