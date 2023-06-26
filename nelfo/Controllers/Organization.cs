using System.Collections.Generic;
using Newtonsoft.Json;

public class Seller
{
  [JsonProperty("orgNo")]
  public string OrganizationNumber { get; set; }

  [JsonProperty("orgName")]
  public string OrganizationName { get; set; }
}

public class Product
{
  [JsonProperty("productNo")]
  public string ProductNumber { get; set; }

  [JsonProperty("description")]
  public string Description { get; set; }

  [JsonProperty("priceUnit")]
  public string PriceUnit { get; set; }

  [JsonProperty("price")]
  public string Price { get; set; }

  [JsonProperty("quantity")]
  public string Quantity { get; set; }

  [JsonProperty("weight")]
  public string Weight { get; set; }
}

public class Organization
{
  [JsonProperty("seller")]
  public Seller Seller { get; set; }

  [JsonProperty("products")]
  public List<Product> Products { get; set; }

  public Organization()
  {
    Seller = new Seller();
    Products = new List<Product>();
  }
}
