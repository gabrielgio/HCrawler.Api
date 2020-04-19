using Moq;

namespace HCrawler.Test.Mock
{
    public static class ServiceMock
    {
        /// <summary>
        /// Return mocked instance of T
        /// </summary>
        public static T Spawn<T>(this Mock<T> source) where T : class => source.Object;

        /// <summary>
        /// Builder for `VerifyAll`. It returns the same instance so we can keep building.
        /// </summary>
        public static Mock<T> Ensure<T>(this Mock<T> source) where T : class
        {
            source.VerifyAll();
            return source;
        }
    }
}
