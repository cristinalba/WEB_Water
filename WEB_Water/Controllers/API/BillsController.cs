﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Repositories;

namespace WEB_Water.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : Controller
    {
        private readonly IBillRepository _billRepository;

        public BillsController(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        [HttpGet]
        public IActionResult GetBills()
        {
            return Ok(_billRepository.GetAll());
        }

    }
}
