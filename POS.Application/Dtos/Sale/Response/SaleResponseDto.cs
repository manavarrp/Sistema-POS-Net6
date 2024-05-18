﻿using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Dtos.Sale.Response
{
    public class SaleResponseDto
    {
        public int SaleId { get; set; }
        public string VoucherDescription { get; set; } = null!;
        public string VoucherNumber { get; set; } = null!;
        public string Client { get; set; } = null!;
        public string Warehouse {  get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public DateTime DateOfSale { get; set; }
        
    }
}
