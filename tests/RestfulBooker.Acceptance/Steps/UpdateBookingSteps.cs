using System;
using System.Text;
using NUnit.Framework;
using Reqnroll;
using RestSharp;
using Allure.Commons;
using Newtonsoft.Json.Linq;
using RestfulBooker.Acceptance.Support; // ✅ for ConfigReader

namespace RestfulBooker.Acceptance.Steps
{
  [Binding]
  public class UpdateBookingSteps
  {
    private string token = "";
    private RestResponse? response;

    [Given(@"I have a valid token")]
    public void GivenIHaveAValidToken()
    {
      var client = CommonSteps.client;

      // ✅ Read credentials from config file
      var username = ConfigReader.Get("username");
      var password = ConfigReader.Get("password");

      var request = new RestRequest("/auth", Method.Post)
          .AddHeader("Content-Type", "application/json")
          .AddJsonBody(new { username = username, password = password });

      var res = client.Execute(request);
      var data = JObject.Parse(res.Content!);
      token = data["token"]?.ToString() ?? "";

      Console.WriteLine($"🔐 Token generated successfully: {token}");
    }

    // 🆕 Negative flow token generator
    [Given(@"I have an invalid token")]
    public void GivenIHaveAnInvalidToken()
    {
      // Hardcoded invalid token
      token = "invalid12345";
      Console.WriteLine($"🚫 Using invalid token: {token}");
    }

    [When(@"I update booking (.*) with firstname ""(.*)""")]
    public void WhenIUpdateBooking(int id, string firstname)
    {
      var client = CommonSteps.client;

      // ✅ Create JSON body dynamically
      var body = new
      {
        firstname = firstname,
        lastname = "Updated",
        totalprice = 250,
        depositpaid = true,
        bookingdates = new
        {
          checkin = "2025-11-10",
          checkout = "2025-11-15"
        },
        additionalneeds = "Lunch"
      };

      // ✅ Build the PUT request
      var request = new RestRequest($"/booking/{id}", Method.Put)
          .AddHeader("Content-Type", "application/json")
          .AddHeader("Accept", "application/json")
          .AddHeader("Cookie", $"token={token}")
          .AddJsonBody(body);

      Console.WriteLine($"\n📤 PUT /booking/{id}");
      Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(body, Newtonsoft.Json.Formatting.Indented));

      response = client.Execute(request);

      Console.WriteLine($"\n📥 Response ({(int)response.StatusCode}): {response.Content}");

      // ✅ Add response to Allure report
      AllureLifecycle.Instance.AddAttachment(
          $"PUT /booking/{id} Response",
          "application/json",
          Encoding.UTF8.GetBytes(response.Content ?? "No Content"),
          "json"
      );
    }

    [Then(@"the update response status code should be (.*)")]
    public void ThenTheResponseStatusCodeShouldBe(int expected)
    {
      Assert.IsNotNull(response, "Response is null!");
      Assert.AreEqual(expected, (int)response.StatusCode);
      Console.WriteLine($"✅ Status code verified: {(int)response.StatusCode}");
    }

    [Then(@"print updated booking details")]
    public void ThenPrintUpdatedBookingDetails()
    {
      Console.WriteLine("\n📄 Updated Booking Details:");
      Console.WriteLine(response?.Content ?? "No Content");
    }
  }
}
