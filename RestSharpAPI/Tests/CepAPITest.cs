using System.Net;
using Newtonsoft.Json;
using RestSharp;
using NUnit.Framework;
using RestSharpAPI.Data;

namespace RestSharpAPI.Tests;

public class Tests
{   
    string valid_cep = "13207340";
    private string base_url = "https://viacep.com.br/ws/" ;
    
    [SetUp]
    public void Setup()
    {
        
    }

    [TestCase("13207340", HttpStatusCode.OK, TestName = "Check success status code")]
    [TestCase("111111111", HttpStatusCode.BadRequest, TestName = "Check error status code")]
    public void StatusCodeTest(string cep, HttpStatusCode expectedHttpStatusCode)
    {   
        // arrange
        var client = new RestClient(($"{base_url}"));
        var request = new RestRequest($"{cep}/json", Method.Get);

        // act
        var response = client.Execute(request);        
        
        // assert
        Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
    }
    
    [Test]
    public void ContentTypeTest()
    {
        
        // arrange
        RestClient client = new RestClient($"{base_url}");
        RestRequest request = new RestRequest($"{valid_cep}/json", Method.Get);
    
        // act
        var response = client.Execute(request);
    
        // assert
        Assert.That(response.ContentType, Is.EqualTo("application/json"));
    }
    
    [Test]
    public void AddressTest()
    {
        // arrange
        RestClient client = new RestClient($"{base_url}");
        RestRequest request = new RestRequest($"{valid_cep}/json", Method.Get);
    
        // act
        var response = client.Execute(request);
    
        var getResponse = JsonConvert.DeserializeObject<GetCepResponse>(response.Content);
    
        // assert
        Assert.That(getResponse.Logradouro, Is.EqualTo("Rua Congo"));
        Assert.That(getResponse.Bairro, Is.EqualTo("Jardim Bonfiglioli"));
        Assert.That(getResponse.Localidade, Is.EqualTo("Jundiaí"));
        Assert.That(getResponse.Uf, Is.EqualTo("SP"));
        Assert.That(getResponse.Ddd, Is.EqualTo("11"));
    }
}
