using Xunit;

namespace sd.IntegrationTests;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<SharedTestContext>
{

}
