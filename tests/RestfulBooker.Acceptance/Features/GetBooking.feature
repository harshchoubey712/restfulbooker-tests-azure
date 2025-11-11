@GetBooking
Feature: Get Booking by ID
  Verify booking details for given booking IDs.

  @PositiveFlow
  @GetBooking_ValidId
  Scenario Outline: Fetch booking details with valid ID
    Given I have the Restful Booker base URL
    When I send a GET request to "/booking/<id>"
    Then the response status code should be 200
    And print booking details

    Examples:
      | id |
      | 8  |
      | 5  |
      | 10 |

  @NegativeFlow
  @GetBooking_InvalidId
  Scenario Outline: Fetch booking details with invalid ID
    Given I have the Restful Booker base URL
    When I send a GET request to "/booking/<invalidId>"
    Then the response status code should be 404
    And print error message in response

    Examples:
      | invalidId |
      | 9999999   |
      | -1        |


  @NegativeFlow
  @GetBooking_NonNumericId
  Scenario Outline: Fetch booking details with non-numeric ID
    Given I have the Restful Booker base URL
    When I send a GET request to "/booking/<invalidId>"
    Then the response status code should be 404
    And print error message in response

    Examples:
      | invalidId |
      | abc       |
      | xyz@      |