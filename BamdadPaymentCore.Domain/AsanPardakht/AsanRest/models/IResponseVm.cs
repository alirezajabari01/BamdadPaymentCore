﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestService.models
{
    public interface IResponseVm
    {
        int ResCode { get; set; }
        string ResMessage { get; set; }
    }
}