Restful Booker API Automation Framework

Tech Stack:
C# | Reqnroll (SpecFlow for .NET) | NUnit | RestSharp | Allure | Docker | Azure DevOps | GitLab CI

📘 Overview

This framework automates RESTful Booker API end-to-end using modern DevOps practices.
It validates CRUD operations for booking management through BDD-style API tests and generates detailed Allure HTML reports.

🎯 Key Highlights

✅ BDD framework using Reqnroll + NUnit + RestSharp

✅ Configurable execution via Azure Variable Groups

✅ Dockerized test runs in Azure or GitLab CI

✅ Allure reporting with request/response attachments

✅ Tag-based filtering (Feature / Flow / Scenario level)

🏗 Project Structure
RestfulBooker.Acceptance/
├── Features/
│ ├── CreateBooking.feature
│ ├── GetBooking.feature
│ ├── GetBookingIds.feature
│ ├── UpdateBooking.feature
│
├── Steps/
│ ├── CommonSteps.cs
│ ├── CreateBookingSteps.cs
│ ├── GetBookingSteps.cs
│ ├── GetBookingIdsSteps.cs
│ ├── UpdateBookingSteps.cs
│
├── Support/
│ ├── ConfigReader.cs
│ └── appsettings.json
│
├── TestHooks.cs
├── RestfulBooker.Acceptance.csproj
├── Dockerfile
└── azure-pipelines.yml

⚙️ Azure Variable Group: RestfulBooker-TestVars
Variable Name Purpose
CREATEBOOKING_FEATURE_FILTER Run all CreateBooking tests
GETBOOKING_FEATURE_FILTER Run all GetBooking tests
GETBOOKINGIDS_FEATURE_FILTER Fetch all booking IDs
UPDATEBOOKING_FEATURE_FILTER Update Booking tests
POSITIVE_FLOW_FILTER Positive scenarios only
NEGATIVE_FLOW_FILTER Negative scenarios only
FASTFOREX_API_KEY (optional) External API key placeholder (not used in current project)

🔐 Note:
The FASTFOREX_API_KEY variable exists for optional external API validation.
The Restful Booker API itself does not require any API key — it uses token-based authentication via the /auth endpoint.

🧩 CI/CD Pipeline (Azure DevOps)

File: azure-pipelines.yml

🔹 Parameters
parameters:

- name: FEATURE
  type: string
  default: "All"
  values: [All, CreateBooking, GetBooking, GetBookingIds, UpdateBooking]

- name: FLOW
  type: string
  default: "All"
  values: [All, PositiveFlow, NegativeFlow]

- name: SCENARIO
  type: string
  default: "All"
  values: [All, CreateBooking_Valid, GetBooking_ValidId, UpdateBooking_InvalidToken]

🔹 Execution Steps
steps:

- checkout: self

- script: |
  echo "🎯 Running filtered tests..."
  dotnet test --filter "TestCategory=${{ parameters.FEATURE }}"
  displayName: "🚀 Run Tests"

- script: |
  allure generate allure-results --clean -o allure-report
  displayName: "📊 Generate Allure Report"

- publish: allure-report
  artifact: allure-report
  displayName: "📤 Publish Allure Report"

🐳 Dockerfile (Containerized Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .

RUN dotnet restore RestfulBooker.Acceptance.csproj
RUN dotnet build RestfulBooker.Acceptance.csproj -c Release --no-restore

# Install Java + Allure CLI

RUN apt-get update && apt-get install -y openjdk-17-jre wget unzip
RUN wget https://github.com/allure-framework/allure2/releases/download/2.29.0/allure-2.29.0.zip && \
 unzip allure-2.29.0.zip -d /opt/ && \
 ln -s /opt/allure-2.29.0/bin/allure /usr/bin/allure

CMD ["dotnet", "test", "RestfulBooker.Acceptance.csproj", "-c", "Release", "--no-build"]

✅ Creates a self-contained image with .NET 8 + Java 17 + Allure CLI preinstalled.

🧱 Key Components
File Purpose
CommonSteps.cs Handles base URL, GET/POST methods, status code checks
CreateBookingSteps.cs Covers booking creation (valid/invalid)
GetBookingSteps.cs Validates booking retrieval (valid ID / negative cases)
UpdateBookingSteps.cs PUT booking tests (valid/invalid token)
TestHooks.cs Allure setup, teardown, and test lifecycle hooks
ConfigReader.cs Reads base URL and credentials from appsettings.json
🧪 Run Tests Locally
dotnet clean
dotnet build
dotnet test --filter "TestCategory=PositiveFlow"

📊 Generate Allure Report (Local)
allure generate allure-results --clean -o allure-report
allure serve allure-results

🐳 Docker Commands
Command Purpose
docker build -t restfulbooker-tests . Build Docker image
docker run --rm restfulbooker-tests Run all tests
docker run --rm restfulbooker-tests bash -c "dotnet test --filter 'TestCategory=PositiveFlow'" Run filtered tests
docker run --rm restfulbooker-tests bash -c "allure generate allure-results --clean -o allure-report" Generate HTML report
☁️ Azure / GitLab Execution

Uses RestfulBooker-TestVars variable group for secure config.

Select execution filters by Feature, Flow, or Scenario.

Generates artifacts:

TestResults/ → TRX test results

allure-results/ → Raw Allure data

allure-report/ → Final HTML report

🔐 Optional External API Integration (Example)

(Not used in this project — shown for illustration only)

string apiKey = Environment.GetEnvironmentVariable("FASTFOREX_API_KEY");
var client = new RestClient("https://api.fastforex.io");
var request = new RestRequest("/fetch-all", Method.Get);
request.AddParameter("api_key", apiKey);
var response = client.Execute(request);

✅ Summary

✔ End-to-end RESTful Booker API automation
✔ Modern BDD + DevOps stack (Reqnroll + NUnit + Allure + Docker)
✔ Filtered execution (Feature / Flow / Scenario level)
✔ Secure pipeline variables via Azure Library
✔ Allure HTML reporting for clear traceability
