﻿using BamdadPaymentCore.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers
{
    public class HealthCheckController : ApiBaseController
    {
        [HttpGet("{id:int}")]
        public string Check(int id)
         => "Bamdad Payment Code Available Your Id : " + id.ToString();
    }
}
