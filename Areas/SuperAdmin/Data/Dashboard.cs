using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssetsManagement_Admin.Areas.SuperAdmin.Data
{
    public class Dashboard
    {
    }
    public class DashboardChart
    {
        public List<PieChartData> objPieChartData { get; set; }
        public List<BarChartData> objYBarChartData { get; set; }
        public List<BarChartData> objNBarChartData { get; set; }

    }

    public class PieChartData
    {
        public string Ageing { get; set; }
        public string BlockValues { get; set; }
        public string NetValues { get; set; }

    }

    public class BarChartData
    {
        public string Ageing { get; set; }
        public string BlockValues { get; set; }
        public string NetValues { get; set; }
        public string BlockValuePercentage { get; set; }
        public string NetValuePercentage { get; set; }
    }
}