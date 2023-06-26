using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace cwproj.Controllers;

public class HealthCheckRequest
{
    public bool? CheckMoreStuff { get; set; }
}

public class HealthCheckResponse
{
    public string? Message { get; set; }
}

[ApiController]
[Route("[controller]/[action]")]
public class NelfoUploadController : ControllerBase
{
    private readonly ILogger<NelfoUploadController> _logger;

    public NelfoUploadController(ILogger<NelfoUploadController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public HealthCheckResponse HealthCheck(HealthCheckRequest? req)
    {
        _logger.LogInformation($"HealthCheck: {req?.CheckMoreStuff ?? false}");

        return new HealthCheckResponse()
        {
            Message = "OK"
        };
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        try
        {
            Organization organization = new();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using var csvParser = new TextFieldParser(reader);
                csvParser.Delimiters = new string[] { ";" }; // Set the delimiter used in the CSV file

                while (!csvParser.EndOfData)
                {
                    // Read each line of the CSV file
                    string[] fields = csvParser.ReadFields();
                    string dataType = fields[0];

                    if (dataType.Equals("VH"))
                    {
                        organization.Seller.OrganizationNumber = fields[3];
                        organization.Seller.OrganizationName = fields[10];
                    }
                    else if (dataType.Equals("VL"))
                    {
                        Product product = new();
                        product.ProductNumber = fields[2];
                        product.Description = fields[3];
                        product.PriceUnit = fields[6];
                        product.Price = fields[8];
                        product.Quantity = fields[9];

                        organization.Products.Add(product);
                    }
                    else if (dataType.Equals("VX") && fields[1].Equals("VEKT"))
                    {
                        Product lastProduct = organization.Products[^1];
                        lastProduct.Weight = fields[2];
                    }
                }
            }

            string json = JsonConvert.SerializeObject(organization);

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(json);

            string contentType = "application/json";
            string fileName = "organization.json";

            return File(byteArray, contentType, fileName);
        }
        catch (Exception ex)
        {
            // Handle any errors that occurred during file processing
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }
}