using System;
using System.Text;
using NUnit.Framework;
using Reqnroll;
using RestSharp;
using Allure.Commons;
using RestfulBooker.Acceptance.Support; // ✅ for ConfigReader

namespace RestfulBooker.Acceptance.Steps
{
  [Binding]
  public class CommonSteps
  {
    public static RestClient client;
    public static RestResponse? response;

    private readonly AllureLifecycle allure = AllureLifecycle.Instance;

    [Given(@"I have the Restful Booker base URL")]
    public void GivenBaseUrl()
    {
      var baseUrl = ConfigReader.Get("baseUrl");
      client = new RestClient(baseUrl);
      Console.WriteLine($"🌍 Base URL initialized from config: {baseUrl}");
    }

    [When(@"I send a GET request to ""(.*)""")]
    public void WhenGet(string endpoint)
    {
      var req = new RestRequest(endpoint, Method.Get)
          .AddHeader("Accept", "application/json");

      response = client.Execute(req);

      Console.WriteLine($"\n📤 GET Request: {client.Options.BaseUrl}{endpoint}");
      Console.WriteLine($"📥 Response ({(int)response.StatusCode}): {response.Content}");

      try
      {
        allure.AddAttachment("GET Request URL", "text/plain",
            Encoding.UTF8.GetBytes($"{client.Options.BaseUrl}{endpoint}"), "txt");

        allure.AddAttachment("GET Response Body", "application/json",
            Encoding.UTF8.GetBytes(response.Content ?? "No Content"), "json");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"⚠️ Allure attachment failed: {ex.Message}");
      }
    }

    [Category("PositiveFlow")]
    [Category("NegativeFlow")]
    [Then(@"the response status code should be (.*)")]
    public void ThenStatusCode(int expected)
    {
      Assert.IsNotNull(response, "Response is null!");
      Assert.AreEqual(expected, (int)response.StatusCode);
      Console.WriteLine($"✅ Status Code Verified: {(int)response.StatusCode}");
    }

    [Then(@"print error message in response")]
    [Category("NegativeFlow")]
    public void ThenPrintErrorMessageInResponse()
    {
      if (response == null)
      {
        Console.WriteLine("⚠️ No response found!");
        return;
      }

      var responseBody = response.Content ?? "No Content";
      Console.WriteLine("\n❌ Error message in response:");
      Console.WriteLine(responseBody);

      try
      {
        allure.AddAttachment("Error Response Body", "application/json",
            Encoding.UTF8.GetBytes(responseBody), "json");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"⚠️ Allure attachment failed: {ex.Message}");
      }
    }
  }
}
