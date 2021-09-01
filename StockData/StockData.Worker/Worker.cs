using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockData.Scraping.BuisnessObjects;
using StockData.Scraping.Services;
using StockData.Worker.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockData.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ICompanyService _companyService;
        private readonly IStockPriceService _stockPriceService;

        public Worker(ILogger<Worker> logger, ICompanyService companyService, IStockPriceService stockPriceService)
        {
            _logger = logger;
            _companyService = companyService;
            _stockPriceService = stockPriceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string url = "https://www.dse.com.bd/latest_share_price_scroll_l.php";

                var htmlDoc = UrlHandlerModel.GetUrlResponse(url);

                var sharesTableRows = htmlDoc.DocumentNode.SelectNodes("//table")
                    .Where(a => a.HasClass("shares-table")).FirstOrDefault()
                    .SelectNodes("tr | tbody/tr").ToList();

                if(sharesTableRows != null)
                {
                    foreach (var htmlrow in sharesTableRows)
                    {
                        var row = htmlrow.SelectNodes("td").ToList();

                        if(row.Count != 11)
                        {
                            _logger.LogError("{1} : Cells length mitch matched on Row No: {0}", row[0], DateTimeOffset.Now);
                            break;
                        }

                        _companyService.CreateCompany(new Company() {
                            TradeCode = row[1].InnerText.Trim() 
                        });

                        _stockPriceService.CreateStockPrice(new StockPrice()
                        {
                            CompanyId = row[1].InnerText.Trim(),
                            LastTradingPrice = Double.Parse(row[2].InnerText.Trim()),
                            High = Double.Parse(row[3].InnerText.Trim()),
                            Low = Double.Parse(row[4].InnerText.Trim()),
                            ClosePrice = Double.Parse(row[5].InnerText.Trim()),
                            YesterdayClosePrice = Double.Parse(row[6].InnerText.Trim()),
                            Change = Double.Parse(row[7].InnerText.Trim()),
                            Trade = Double.Parse(row[8].InnerText.Trim()),
                            Value = Double.Parse(row[9].InnerText.Trim()),
                            Volume = Double.Parse(row[10].InnerText.Trim())
                        });

                    }
                }
                else
                {
                    _logger.LogError("{time} : Found a empty List.", DateTimeOffset.Now);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

    }

}
