@CreateBooking
Feature: Create Booking
  Validate that booking creation works correctly.

  @PositiveFlow
  @CreateBooking_Valid
  Scenario Outline: Create booking with valid details
    Given I have the Restful Booker base URL
    When I create a booking with firstname "<firstname>", lastname "<lastname>", price <totalprice>, deposit "<deposit>", checkin "<in>", checkout "<out>", needs "<needs>"
    Then the response status code should be 200
    And print created booking details

    Examples:
      | firstname | lastname | totalprice | deposit | in         | out        | needs     |
      | Harsh     | Choubey  | 120        | true    | 2025-11-01 | 2025-11-05 | Breakfast |


  @NegativeFlow
  @CreateBooking_MissingField
  Scenario: Create booking with missing mandatory field
    Given I have the Restful Booker base URL
    When I send an invalid JSON to create a booking
    Then the response status code should be 400
    And print error message in response


  @NegativeFlow
  @CreateBooking_InvalidDateFormat
  Scenario: Create booking with invalid date format
    Given I have the Restful Booker base URL
    When I send a booking request with invalid date format
    Then the response status code should be 418
    And print error message in response

