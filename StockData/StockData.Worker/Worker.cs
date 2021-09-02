using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, ICompanyService companyService, IStockPriceService stockPriceService)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            _logger = logger;
            _companyService = companyService;
            _stockPriceService = stockPriceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var url = _configuration.GetSection("Url").Get<List<string>>();

                var htmlDoc = UrlHandlerModel.GetUrlResponse(url[0]);

                if (htmlDoc != null)
                {
                    string marketStatus = "";

                    try
                    {
                        marketStatus = htmlDoc.DocumentNode.SelectNodes("//div/span")
                           .Where(a => a.InnerText.Contains("Market Status:")).FirstOrDefault()
                           .SelectNodes("span/b").FirstOrDefault()
                           .InnerText.Trim();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("{0} : Can't get Market Status {1}", DateTimeOffset.Now, ex);
                    }

                    if (marketStatus == "Open")
                    {
                        IList<HtmlNode> sharesTableRows = null;

                        try
                        {
                            sharesTableRows = htmlDoc.DocumentNode.SelectNodes("//table")
                               .Where(a => a.HasClass("shares-table")).FirstOrDefault()
                               .SelectNodes("tr | tbody/tr").ToList();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("{0} : Can't get Shares Table Rows {1}", DateTimeOffset.Now, ex);
                        }

                        if (sharesTableRows != null)
                        {
                            foreach (var htmlrows in sharesTableRows)
                            {
                                var rows = htmlrows.SelectNodes("td").ToList();

                                if (rows.Count != 11)
                                {
                                    _logger.LogError("{1} : Cells length mitch matched on Row No: {0}", rows[0], DateTimeOffset.Now);
                                    break;
                                }

                                if (!_companyService.ExistsCompany(rows[1].InnerText.Trim()))
                                {
                                    _companyService.CreateCompany(new Company()
                                    {
                                        TradeCode = rows[1].InnerText.Trim()
                                    });
                                }

                                _stockPriceService.CreateStockPrice(new StockPrice()
                                {
                                    CompanyId = rows[1].InnerText.Trim(),
                                    LastTradingPrice = rows[2].InnerText.Trim().ConvertToDouble(),
                                    High = rows[3].InnerText.Trim().ConvertToDouble(),
                                    Low = rows[4].InnerText.Trim().ConvertToDouble(),
                                    ClosePrice = rows[5].InnerText.Trim().ConvertToDouble(),
                                    YesterdayClosePrice = rows[6].InnerText.Trim().ConvertToDouble(),
                                    Change = rows[7].InnerText.Trim().ConvertToDouble(),
                                    Trade = rows[8].InnerText.Trim().ConvertToDouble(),
                                    Value = rows[9].InnerText.Trim().ConvertToDouble(),
                                    Volume = rows[10].InnerText.Trim().ConvertToDouble()
                                });

                            }
                        }
                        else
                        {
                            _logger.LogError("{time} : Found a empty Row List.", DateTimeOffset.Now);
                        }
                    }
                }
                else
                {
                    _logger.LogError("No Url response");
                }

                await Task.Delay(60000, stoppingToken);
            }
        }

    }

}
