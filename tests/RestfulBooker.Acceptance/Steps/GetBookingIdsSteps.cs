using System;
using System.Text;
using Newtonsoft.Json.Linq;
using Allure.Commons;
using Reqnroll;

namespace RestfulBooker.Acceptance.Steps
{
  [Binding]
  public class GetBookingIdsSteps
  {
    [Then(@"print all booking IDs from the response")]
    public void ThenPrint()
    {
      if (CommonSteps.response == null)
      {
        Console.WriteLine("⚠️ No response found! Please ensure GET /booking was called.");
        return;
      }

      var content = CommonSteps.response.Content ?? "No Content";
      Console.WriteLine("📦 Raw Response:\n" + content);

      try
      {
        var jsonArray = JArray.Parse(content);
        Console.WriteLine($"\n📄 Total Bookings Found: {jsonArray.Count}");

        foreach (var booking in jsonArray)
        {
          Console.WriteLine($"🆔 Booking ID: {booking["bookingid"]}");
        }

        // ✅ Allure attachment
        AllureLifecycle.Instance.AddAttachment(
            "Booking IDs Response",
            "application/json",
            Encoding.UTF8.GetBytes(content),
            "json"
        );
      }
      catch (Exception ex)
      {
        Console.WriteLine($"❌ JSON Parse Error: {ex.Message}");
        AllureLifecycle.Instance.AddAttachment(
            "Invalid Booking IDs Response",
            "text/plain",
            Encoding.UTF8.GetBytes(content),
            "txt"
        );
      }
    }
  }
}
