﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.models.reverse
{
    public class ReverseCommand
    {
        public int merchantConfigurationId { get; set; }
        public ulong payGateTranId { get; set; }
    }
}