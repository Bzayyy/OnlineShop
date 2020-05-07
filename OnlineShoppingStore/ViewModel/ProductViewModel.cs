
using Microsoft.AspNetCore.Http;
using OnlineShopEdmx.Model;
using OnlineShopping.Models;
using OnlineShopping.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShopping.ViewModel
{
    public class ProductViewModel
    {
        public ProductDetail dbModel { get; set; }
        public IEnumerable<PairModel> ddlIsActive { get; set; }
        public IFormFile image { get; set; }
    }

    public class ProductViewModelLst
    {
        public IEnumerable<ProductDetail> dbModelLst { get; set; }
    }
}
