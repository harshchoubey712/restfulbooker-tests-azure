@UpdateBooking
Feature: Update Booking
  Verify updating a booking with valid and invalid tokens.

  @PositiveFlow
  @UpdateBooking_ValidToken
  Scenario Outline: Update existing booking with valid token
    Given I have the Restful Booker base URL
    And I have a valid token
    When I update booking <bookingId> with firstname "<firstname>"
    Then the update response status code should be 200
    And print updated booking details

    Examples:
      | bookingId | firstname |
      | 5         | John      |
      | 10        | Alice     |


  @NegativeFlow
  @UpdateBooking_InvalidToken
  Scenario Outline: Try updating booking with invalid token
    Given I have the Restful Booker base URL
    And I have an invalid token
    When I update booking <bookingId> with firstname "<firstname>"
    Then the update response status code should be 403
    And print error message in response

    Examples:
      | bookingId | firstname |
      | 5         | Harsh     |
      | 8         | Choubey   |
