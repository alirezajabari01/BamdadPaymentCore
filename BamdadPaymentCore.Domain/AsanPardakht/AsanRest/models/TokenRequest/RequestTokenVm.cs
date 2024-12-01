using RestService.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



public class RequestTokenVm : ITokenVm
{
    public string RefId { get; set; }
    public int ResCode { get; set; }
    public string ResMessage { get; set; }
}