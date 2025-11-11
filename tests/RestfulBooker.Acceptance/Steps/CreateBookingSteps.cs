using System;
using System.Text;
using NUnit.Framework;
using Reqnroll;
using RestSharp;
using Allure.Commons;

namespace RestfulBooker.Acceptance.Steps
{
  [Binding]
  [Category("CreateBooking")]    // ✅ Feature-level tag
  public class CreateBookingSteps
  {
    private RestClient client => CommonSteps.client;
    private RestResponse response
    {
      get => CommonSteps.response!;
      set => CommonSteps.response = value;
    }

    private readonly AllureLifecycle allure = AllureLifecycle.Instance;

    // 🟢 Positive Flow Scenario: CreateBooking_Valid
    [When(@"I create a booking with firstname ""(.*)"", lastname ""(.*)"", price (.*), deposit ""(.*)"", checkin ""(.*)"", checkout ""(.*)"", needs ""(.*)""")]
    [Category("PositiveFlow")]            
    [Category("CreateBooking_Valid")]     
    public void WhenCreate(
        string first,
        string last,
        int price,
        string deposit,
        string checkin,
        string checkout,
        string needs)
    {
      var jsonBody =
$@"{{
  ""firstname"": ""{first}"",
  ""lastname"": ""{last}"",
  ""totalprice"": {price},
  ""depositpaid"": {deposit.ToLower()},
  ""bookingdates"": {{
    ""checkin"": ""{checkin}"",
    ""checkout"": ""{checkout}""
  }},
  ""additionalneeds"": ""{needs}""
}}";

      var req = new RestRequest("/booking", Method.Post)
          .AddHeader("Accept", "application/json")
          .AddHeader("Content-Type", "application/json")
          .AddStringBody(jsonBody, DataFormat.Json);

      response = client.Execute(req);
      var responseBody = response.Content ?? "No Content";

      try
      {
        allure.AddAttachment("Request Body", "application/json", Encoding.UTF8.GetBytes(jsonBody), "json");
        allure.AddAttachment("Response Body", "application/json", Encoding.UTF8.GetBytes(responseBody), "json");
      }
      catch (Exception e)
      {
        Console.WriteLine($"⚠️ Allure attachment failed: {e.Message}");
      }

      Console.WriteLine("📤 POST /booking");
      Console.WriteLine(jsonBody);
      Console.WriteLine($"\n📥 Response ({(int)response.StatusCode}): {responseBody}");
    }

    // 🔴 Negative Flow Scenario: Missing Field
    [When(@"I send an invalid JSON to create a booking")]
    [Category("NegativeFlow")]
    [Category("CreateBooking_MissingField")]
    public void WhenISendInvalidJsonToCreateBooking()
    {
      var invalidJson = @"{""firstname"":""Harsh"" ""lastname"":""Choubey"" ""totalprice"":120}"; // missing commas

      var req = new RestRequest("/booking", Method.Post)
          .AddHeader("Content-Type", "application/json")
          .AddStringBody(invalidJson, DataFormat.Json);

      response = client.Execute(req);

      var respBody = response.Content ?? "No Content";
      allure.AddAttachment("Invalid JSON Request", "application/json", Encoding.UTF8.GetBytes(invalidJson), "json");
      allure.AddAttachment("Response Body", "application/json", Encoding.UTF8.GetBytes(respBody), "json");

      Console.WriteLine("\n📤 Invalid JSON Request:\n" + invalidJson);
      Console.WriteLine($"\n📥 Response ({(int)response.StatusCode}): {respBody}");
    }

    // 🔴 Negative Flow Scenario: Invalid Date Format
    [When(@"I send a booking request with invalid date format")]
    [Category("NegativeFlow")]
    [Category("CreateBooking_InvalidDateFormat")]
    public void WhenISendABookingRequestWithInvalidDateFormat()
    {
      var invalidBooking = new
      {
        firstname = "Harsh",
        lastname = "Choubey",
        totalprice = 120,
        depositpaid = true,
        bookingdates = new
        {
          checkin = "invalidDate",
          checkout = "2025-11-15"
        },
        additionalneeds = "Breakfast"
      };

      var request = new RestRequest("/booking", Method.Post)
          .AddHeader("Content-Type", "application/json")
          .AddJsonBody(invalidBooking);

      response = client.Execute(request);
      var respBody = response.Content ?? "No Content";

      allure.AddAttachment("Invalid Date Request", "application/json", Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(invalidBooking)), "json");
      allure.AddAttachment("Response Body", "application/json", Encoding.UTF8.GetBytes(respBody), "json");

      Console.WriteLine("🔹 Invalid Date Request Sent");
      Console.WriteLine("🔹 Status: " + response.StatusCode);
      Console.WriteLine("🔹 Response Body: " + respBody);
    }

    // 🟢 Shared Step: Print booking details
    [Then(@"print created booking details")]
    [Category("PositiveFlow")]            
    [Category("CreateBooking_Valid")]
    public void ThenPrint()
    {
      Console.WriteLine("\n📄 Created Booking Details:\n" + response.Content);
    }
  }
}
