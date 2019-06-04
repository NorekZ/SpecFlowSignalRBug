using System.Threading.Tasks;
using Xunit;

namespace SpecFlowSignalRBug
{
    public class WorkingXunitTest
    {
        [Fact]
        public async Task WorkingTest()
        {
            var steps = new Steps();

            steps.GivenSignalRServerIsRunning();
            await steps.GivenSignalRConnectionIsEstablished();
            await steps.WhenIInvokeMethodOnTheConnection();
            steps.ThenThisStepShouldBeCalled();
        }
    }
}