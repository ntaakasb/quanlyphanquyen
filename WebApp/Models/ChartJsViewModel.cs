using System;
using WebApp.Models.Chart;
using Newtonsoft.Json;

namespace WebApp.Models
{
    public class ChartJsViewModel
    {
        public ChartJs Chart { get; set; }
        public string ChartJson { get; set; }
    }
}