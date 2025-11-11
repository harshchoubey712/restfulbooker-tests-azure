using System;
using System.IO;
using System.Collections.Generic;
using Allure.Commons;
using Reqnroll;

namespace RestfulBooker.Acceptance.Steps
{
  [Binding]
  public sealed class TestHooks
  {
    private readonly AllureLifecycle _allure = AllureLifecycle.Instance;
    private readonly FeatureContext _featureContext;
    private readonly ScenarioContext _scenarioContext;
    private string _testUuid;
    private string _containerUuid;

    public TestHooks(FeatureContext featureContext, ScenarioContext scenarioContext)
    {
      _featureContext = featureContext;
      _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
      Directory.CreateDirectory("allure-results");
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
      var featureName = _featureContext.FeatureInfo.Title;
      var scenarioName = _scenarioContext.ScenarioInfo.Title;

      _testUuid = Guid.NewGuid().ToString();
      _containerUuid = Guid.NewGuid().ToString();

      // ✅ Starts Test Container (no parent UUID)
      _allure.StartTestContainer(new TestResultContainer
      {
        uuid = _containerUuid,
        name = featureName
      });

      // ✅ Start Test Case inside container
      var testResult = new TestResult
      {
        uuid = _testUuid,
        name = scenarioName,
        fullName = $"{featureName} - {scenarioName}",
        labels = new List<Label>
                {
                    Label.Feature(featureName),
                    Label.Story(scenarioName),
                    new Label { name = "thread", value = Environment.CurrentManagedThreadId.ToString() }
                }
      };

      // ✅ Link test to container
      _allure.StartTestCase(_containerUuid, testResult);
    }

    [AfterStep]
    public void AfterStep()
    {
      var stepName = _scenarioContext.StepContext.StepInfo.Text;

      var step = new StepResult
      {
        name = stepName,
        status = _scenarioContext.TestError == null ? Status.passed : Status.failed,
        statusDetails = _scenarioContext.TestError == null
              ? null
              : new StatusDetails
              {
                message = _scenarioContext.TestError.Message,
                trace = _scenarioContext.TestError.StackTrace
              }
      };

      _allure.UpdateTestCase(_testUuid, tr => tr.steps.Add(step));
    }

    [AfterScenario]
    public void AfterScenario()
    {
      // ✅ Stop and write test
      _allure.StopTestCase(_testUuid);
      _allure.WriteTestCase(_testUuid);

      // ✅ Stop and write container
      _allure.StopTestContainer(_containerUuid);
      _allure.WriteTestContainer(_containerUuid);
    }
  }
}
