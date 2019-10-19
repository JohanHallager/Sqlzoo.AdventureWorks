﻿using Microsoft.AspNetCore.Mvc;
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
    }
}