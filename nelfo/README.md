# Nelfo Reader Project

Greetings!

This is a simple exercise that we expect should take between 1 to 2 hours to complete.

## The Goal

The goal of this exercise is to implement a Web API that accepts a Nelfo product file and responds with a JSON file containing data extracted from the Nelfo file.

Send your code to `cato@contracting.works`. Even if you ran out of time and the code is not in a runnable state, please send it in. The goal of the exercise is to fuel discussion around the code.

## Sample Code to Get You Started

You are not required to use C# or dotnet 7.0, but we provide you with some C# code to help you get started.

### Useful Links

Here are some links to the dotnet runtime and some code editors you can use:

1. Dotnet 7.0: [https://dotnet.microsoft.com/en-us/download/dotnet/7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
2. Visual Studio Code: [https://code.visualstudio.com/](https://code.visualstudio.com/)
3. Visual Studio Community Edition: [https://visualstudio.microsoft.com/vs/community/](https://visualstudio.microsoft.com/vs/community/)

### How to Build and Run the Sample Code

You can run the code from the command line using the following command:

```bash
dotnet run
```

This should display a response similar to the following:

```bash
╰─$ dotnet run
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5293
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
```

Please note that the local port may be different for you. In my case, I accessed the following page: [http://localhost:5293/swagger/](http://localhost:5293/swagger/)

This will open the Swagger documentation, which allows you to test the API directly from your browser.

An example API endpoint `/NelfoUpload/HealthCheck` is provided, which you can try invoking (it does nothing except log information).

You can then add and test the necessary methods to this API.

## Test Data

You can find an example Nelfo product file in `testdata/V4prisersmall.all`.

There is also a PDF document (`testdata/E-NVare40.pdf`) that describes the Nelfo product file format. Unfortunately, it is in Norwegian, but it may still be helpful.

## Nelfo Product File

Here is a brief description in english about the Nelfo product file.

A Nelfo product file is CSV file that contains product information and related data.

The file contains a lot of data, but you only need to extract a subset of it in order to create a JSON document like this:

```json
{
  "seller": {
    "orgNo": "<ORG-NO>",
    "orgName": "<ORG-NAME>"
  },
  "products": [
    {
      "productNo": "<PRODUCT-NO>",
      "description": "<PRODUCT-NO>",
      "priceUnit": "<PRICE-UNIT>",
      "price": "<PRICE>",
      "quantity": "<QUANTITY>",
      "weight": "<WEIGHT>"
    }
  ]
}
```

### VH Record

The first line is a header line containing constant information for all subsequent lines.

Example:
```
VH;EFONELFO;4.0;NO979692900MVA;;;20230321;;NOK;;Onninen AS;ONNINEN AS - H�GSLUNDVEIEN 55;;2020;SKEDSMOKORSET;NO
```

Cells to verify:
```
Cell #1 - Must be: VH
Cell #2 - Must be: EFONELFO
Cell #3 - Must be: 4.0
```

Cells to extract:
```
Cell #4  - Seller's organization number
Cell #11 - Seller's organization name
```

### VL Record

The VL record marks the beginning of each product line and contains information about the product.

Example:
```
VL;1;1000000;�LFLEX� 2YSLCYK-JB3X1,5+3G0,25; EMC/VFD FREKVENSOMFORMER KABE;2;MTR;METER;11570;10000;20230322;1;010206;F3Y;LAPP NORWA;;N;10000;;
```

Cells to verify:
```
Cell #1 - Must be: VL
Cell #2 - Must be: 1 (Skip products with different numbers)
```

Cells to extract:
```
Cell #3  - Product number
Cell #4  - Product description
Cell #7  - Product price unit
Cell #9  - Product price
Cell #10 - Product quantity
Cell #17 - Stock product (J/N or empty)
```

### VX Record

The VX record contains additional information. Extract the weight if it is available.

Example:
```
VX;VEKT;226
```

Cells to verify:
```
Cell #1 - Must be: VX
Cell #2 - Must be: VEKT (Skip products with different numbers)
```

Cells to extract:
```
Cell #3 - Product weight
```

### Other Records

Skip all other records.

