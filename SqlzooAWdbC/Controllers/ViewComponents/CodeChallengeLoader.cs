using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlzooAWdbC.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlzooAWdbC.Controllers.ViewComponents
{
    public class CodeChallengeLoader : ViewComponent
    {
        private readonly AdventureWorksContext _context;

        public CodeChallengeLoader(AdventureWorksContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int challenge)
        {
            switch (challenge)
            {
                case 1:
                    return View(await CodeChallenge1());
                case 2:
                    return View(await CodeChallenge2());
                case 3:
                    return View(await CodeChallenge3());
                case 4:
                    return View(await CodeChallenge4());
                case 5:
                    return View(await CodeChallenge5());
                case 6:
                    return View(await CodeChallenge6());
                case 7:
                    return View(await CodeChallenge7());
                case 8:
                    return View(CodeChallenge8());
                case 9:
                    return View(await CodeChallenge9());
                case 10:
                    return View(await CodeChallenge10());
                case 11:
                    return View(await CodeChallenge11());
                case 12:
                    return View(await CodeChallenge12());
                case 13:
                    return View(await CodeChallenge13());
                case 14:
                    return View(await CodeChallenge14());
                default:
                    return View(null);
            }
        }


        private async Task<dynamic> CodeChallenge1()
        {
            var model = await _context.Customer
                .Where(c => c.CompanyName == "Bike World")
                .Select(c => new
                            {
                                c.FirstName,
                                c.EmailAddress
                            })
                .Distinct()
                .ToListAsync();

            return model;
        }

        private async Task<dynamic> CodeChallenge2()
        {
            var model = await _context.Address
                .Where(a => a.City == "Dallas")
                .Select(a => new
                                {
                                    a.CustomerAddress.First().Customer.CompanyName,
                                    a.City
                                })
                .ToListAsync();

            return model;
        }


        private async Task<dynamic> CodeChallenge3()
        {
            var model = await _context.SalesOrderDetail
                .Where(a => a.Product.ListPrice > 1000)
                .ToListAsync();

            return new List<dynamic> { new { Count = model.Count() } };
        }

        private async Task<dynamic> CodeChallenge4()
        {
            var model = await _context.SalesOrderHeader
                .Where(a => a.TaxAmt + a.SubTotal + a.Freight > 100000)
                .Select(a => new
                                {
                                    a.Customer.CompanyName,
                                    value = a.TaxAmt + a.SubTotal + a.Freight
                                })
                .ToListAsync();

            return model;
        }

        private async Task<dynamic> CodeChallenge5()
        {
            var model = await _context.SalesOrderDetail
                .Where(s => s.Product.Name == "Racing Socks, L" 
                            && s.SalesOrder.Customer.CompanyName == "Riding Cycles")
                .ToListAsync();

            return new List<dynamic> { new { Total = model.Sum(x => x.OrderQty) } };
        }

        private async Task<dynamic> CodeChallenge6()
        {
            var model = await _context.SalesOrderDetail
                .Where(s => s.OrderQty == 1)
                .Select(x=> new { 
                    x.SalesOrderId,
                    x.UnitPrice
                })
                .ToListAsync();

            return model;
        }
        private async Task<dynamic> CodeChallenge7()
        {
            var model = await _context.SalesOrderDetail
                .Where(s => s.Product.ProductModel.Name == "Racing Socks" )
                .Select(x => new {
                    x.Product.Name,
                    x.SalesOrder.Customer.CompanyName
                })
                .ToListAsync();

            return model;
        }

        private dynamic CodeChallenge8()
        {
            var product = _context.Product
                .Include(x =>x.ProductModel)
                .Include(x=>x.ProductModel.ProductModelProductDescription)
                .ThenInclude(x=>x.ProductDescription)
                .FirstOrDefault(x=>x.ProductId == 736);
            var descriptionCulture = product?.ProductModel?.ProductModelProductDescription.FirstOrDefault(x => x.Culture.ToLower().Contains("fr"));
            return new List<dynamic> { new { a = descriptionCulture?.ProductDescription?.Description } };
        }
        
        private async Task<dynamic> CodeChallenge9()
        {
            var model = await _context.SalesOrderHeader
                .OrderByDescending(x => x.SubTotal)
                .Select(x => new
                {
                    x.SubTotal,
                    x.Customer.CompanyName,
                    x.Freight,
                }).ToListAsync();

                return model;
        }
        private async Task<dynamic> CodeChallenge10()
        {
            var model = await _context.SalesOrderDetail
                .Where(x => x.Product.ProductCategory.Name == "Cranksets" 
                            && x.SalesOrder.ShipToAddress.City == "London")
                .SumAsync(x => x.OrderQty);

            return new List<dynamic> { new { a = model } };
        }

        private async Task<dynamic> CodeChallenge11()
        {
            var model = await _context.Customer
                .Where(x => x.CustomerAddress.Where(y => y.AddressType == "Main Office" && y.Address.City == "Dallas").Any())
                .Select(x => new {
                    x.CompanyName,
                    MainOfficeAddress =   x.CustomerAddress.First(y=>y.AddressType == "Main Office" ).Address.AddressLine1,
                    ShippingAddress  =  x.CustomerAddress.FirstOrDefault(y=>y.AddressType == "Shipping").Address.AddressLine1
                })
                .ToListAsync();

            return model;
        }

        private async Task<dynamic> CodeChallenge12()
        {
            var model = await _context.SalesOrderHeader
               .Select(x => new
               {
                   x.SalesOrderId,
                   x.SubTotal,
                   OrderQtyUnitPrice = x.SalesOrderDetail.Sum(y => y.OrderQty * y.UnitPrice),
                   OrderQtyListPrice = x.SalesOrderDetail.Sum(y => y.OrderQty * y.Product.ListPrice)
               })
                .ToListAsync();

            return model;
        }


        private async Task<dynamic> CodeChallenge13()
        {
            var model = await _context.SalesOrderDetail
                .GroupBy(x=>  x.Product.Name, (y) => new { 
                   Value = y.OrderQty * y.UnitPrice
                })
                .Select(x => new
                {
                    Product_Name = x.Key,
                    Total_Sale_Value =  x.Sum(y=> y.Value )  ,
                })
                .OrderByDescending(x => x.Total_Sale_Value)
                .ToListAsync();

            return model;
        }


        private async Task<dynamic> CodeChallenge14()
        {
            var ranges = new List<Range> {
                new Range{ Name ="    0-  99",  From = 0,     To = 99 } ,
                new Range{ Name ="  100- 999",  From = 100,   To = 999 },
                new Range{ Name =" 1000-9999",  From = 1000,  To = 9999 },
                new Range{ Name ="10000-",      From = 10000 },
            };

            var salesOrderDetail = await _context.SalesOrderDetail
                //.GroupBy(x=> ranges.FirstOrDefault(r => {
                //    bool v = r.To != null ? true : false;
                //    return v; 
                //} ).Name   )
                .ToListAsync();

            var model = ranges.Select(x => new
            {
                x.Name,
                Num_Orders = salesOrderDetail.Count(IsInRange(x)),
                Total_Value = salesOrderDetail.Where(IsInRange(x)).Sum(y => y.UnitPrice * y.OrderQty)
            }).ToList();

            return model;
        }

        private static Func<SalesOrderDetail, bool> IsInRange(Range x)
        {
            return y =>
            {
                bool v = x.To == null;
                var value = y.UnitPrice * y.OrderQty;
                if (v)
                {
                    return value >= x.From;
                }
                return x.From <= value && value <= x.To;
            };
        }
    }

    public class Range
    {
        public string Name { get; set; }
        public decimal From { get; set; }
        public decimal? To { get; set; }

    }



}
