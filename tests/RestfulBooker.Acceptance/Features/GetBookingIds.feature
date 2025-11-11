@GetBookingIds
Feature: Get all booking IDs
  Verify list of all existing booking IDs from Restful Booker API.

  @PositiveFlow
  @GetBookingIds_Valid
  Scenario: Verify list of booking IDs
    Given I have the Restful Booker base URL
    When I send a GET request to "/booking"
    Then the response status code should be 200
    And print all booking IDs from the response

  @NegativeFlow
  @GetBookingIds_InvalidEndpoint
  Scenario: Verify invalid endpoint for booking IDs
    Given I have the Restful Booker base URL
    When I send a GET request to "/bookings"
    Then the response status code should be 404
    And print error message in response

  @NegativeFlow
  @GetBookingIds_ServerError
  Scenario: Verify server failure for booking IDs
    Given I have the Restful Booker base URL
    When I send a GET request to "/booking?checkin=invalid-date"
    Then the response status code should be 500
    And print error message in response
