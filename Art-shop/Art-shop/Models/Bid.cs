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
    
    public partial class Bid
    {
        public int BidID { get; set; }
        public int AuctionID { get; set; }
        public string BidderName { get; set; }
        public decimal BidAmount { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual Auction Auction { get; set; }
        public virtual user_info user_info { get; set; }
    }
}
