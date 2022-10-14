﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using GeekShopping.CouponApi.Model.Base;

namespace GeekShopping.CouponApi.Model
{
    [Table("coupon")]
    public class Coupon : BaseEntity
    {

        [Column("coupon_code")]
        [Required]
        [StringLength(30)]
        public string? CouponCode { get; set; }

        [Column("discount_amount")]
        [Required]
        public decimal DiscountAmount { get; set; }
    }
}
