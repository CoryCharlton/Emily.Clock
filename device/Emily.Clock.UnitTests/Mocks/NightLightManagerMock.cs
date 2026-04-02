using Emily.Clock.UI.Lights;

namespace Emily.Clock.UnitTests.Mocks
{
    internal class NightLightManagerMock: INightLightManager
    {
        public float Brightness { get; set; }
        public bool Enabled { get; set; }

        public void CycleBrightness()
        {
            CycleBrightnessCalled = true;
        }

        public bool CycleBrightnessCalled { get; set; }

        public void CycleColor()
        {
            CycleColorCalled = true;
        }

        public bool CycleColorCalled { get; set; }

        public bool Initialize()
        {
            InitializeCalled = true;

            return InitializeResult;
        }

        public bool InitializeCalled { get; set; }
        public bool InitializeResult { get; set; } = true;

        public void Toggle()
        {
            Enabled = !Enabled;
            ToggleCalled = true;
        }

        public bool ToggleCalled { get; set; }
    }
}
