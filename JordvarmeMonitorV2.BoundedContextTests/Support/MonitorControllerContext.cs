using TechTalk.SpecFlow;

namespace JordvarmeMonitorV2.BoundedContextTests.Support
{
    public class MonitorControllerContext
    {
        private readonly ScenarioContext _scenarioContext;

        public MonitorControllerContext(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        private static readonly string KeyMode = "KeyMode";

        public Mode Mode
        {
            get => _scenarioContext.ContainsKey(KeyMode)
                ? (Mode)_scenarioContext[KeyMode]
                : Mode.Stopped;

            set
            {
                if (!_scenarioContext.ContainsKey(KeyMode))
                {
                    _scenarioContext.Add(KeyMode, value);
                    return;
                }

                _scenarioContext[KeyMode] = value;
            }
        }

        public bool IsRunning => Mode.Running == Mode;
    }
}
