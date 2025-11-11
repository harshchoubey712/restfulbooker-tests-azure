using System;
using System.Text;
using NUnit.Framework;
using Reqnroll;
using RestSharp;
using Allure.Commons;

namespace RestfulBooker.Acceptance.Steps
{
  [Binding]
  [Category("GetBooking")] // ✅ Feature-level tag for pipeline filterings
  public class GetBookingSteps
  {
    private readonly AllureLifecycle allure = AllureLifecycle.Instance;

    // 🟢 POSITIVE FLOW: Valid Booking IDs
    [Then(@"print booking details")]
    [Category("PositiveFlow")]
    [Category("GetBooking_ValidId")]
    public void ThenPrint()
    {
      if (CommonSteps.response == null)
      {
        Console.WriteLine("⚠️ No response found to print!");
        return;
      }

      var content = CommonSteps.response.Content ?? "No Content";
      Console.WriteLine("\n📄 Booking Details Response:\n" + content);

      try
      {
        allure.AddAttachment(
            "Booking Details Response",
            "application/json",
            Encoding.UTF8.GetBytes(content),
            "json"
        );
      }
      catch (Exception ex)
      {
        Console.WriteLine($"⚠️ Allure attachment failed: {ex.Message}");
      }
    }
  }    
}
