using System.Net;
using ApiInterationTests;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Net.Http.Json;
using DataAccess.Models;
using Service.TransferModels.Responses;

namespace ApiIntegrationTests;

public class PriceApiTest : ApiTestBase
{
    [Fact]
    public async Task Get_Prices_Gets_Prices()
    {
        await PgCtxSetup.DbContextInstance.Database.ExecuteSqlRawAsync(@"
INSERT INTO prices (id, price, numbers)
VALUES
    ('95f9a200-4538-4e43-8674-38b67579b8a7', 20.00, 5.00),
    ('1cd3f690-2eeb-405c-b8a7-922c80f0cb3d', 40.00, 6.00),
    ('af8c5461-d9ec-4ede-9bf0-abf2ffac9895', 80.00, 7.00),
    ('ac6f719f-e9e0-4fde-9b31-a12562f7ce01', 160.00, 8.00);
");
        
        var response = await TestHttpClient.GetAsync("/Price/GetPrices");
        var returnedPrices = await response.Content.ReadFromJsonAsync<List<PriceDto>>();
        var pricesInDb = PgCtxSetup.DbContextInstance.Prices.Select(p => p.Price1).OrderBy(x => x).ToList();
        var returnedPriceValues = returnedPrices.Select(p => p.Price1).OrderBy(x => x).ToList();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(pricesInDb, returnedPriceValues);
    }
}