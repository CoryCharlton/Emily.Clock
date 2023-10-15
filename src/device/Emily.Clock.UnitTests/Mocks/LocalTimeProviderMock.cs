using System;

namespace Emily.Clock.UnitTests.Mocks
{
    internal class LocalTimeProviderMock: ILocalTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
        public DateTime UtcNow => DateTime.UtcNow;

        public void Start()
        {
            StartCalled = true;
        }

        public bool StartCalled { get; set; }
    }
}
