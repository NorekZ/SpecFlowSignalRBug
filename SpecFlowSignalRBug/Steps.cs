using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SignalRServer;
using TechTalk.SpecFlow;

namespace SpecFlowSignalRBug
{
    [Binding]
    public class Steps
    {
        private HubConnection _signalrConnection;
        private TestServer _server;
        private bool _wasHelloEventCalled;

        [Given(@"SignalR server is running")]
        public void GivenSignalRServerIsRunning()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>();

            _server = new TestServer(webHostBuilder);
        }

        [Given(@"SignalR connection is established")]
        public async Task GivenSignalRConnectionIsEstablished()
        {
            await EstablishConnectionAsync();
        }

        [When(@"I establish SignalR connection and invoke method")]
        public async Task WhenIEstablishSignalRConnectionAndInvokeMethod()
        {
            await EstablishConnectionAsync();
            await InvokeMethodAsync(); // when invoking method in the same method as connection establishing, its working
        }

        [When(@"I invoke method on the connection")]
        public async Task WhenIInvokeMethodOnTheConnection()
        {
            await InvokeMethodAsync(); // when invoking method in different method that connection establishing, its NOT working
        }

        private async Task EstablishConnectionAsync()
        {
            _signalrConnection = new HubConnectionBuilder()
                .WithUrl(
                    "http://localhost/chatHub",
                    o => o.HttpMessageHandlerFactory = _ => _server.CreateHandler())
                .Build();

            _signalrConnection.On("Hello", () => { _wasHelloEventCalled = true; });

            await _signalrConnection.StartAsync();
        }

        private async Task InvokeMethodAsync()
        {
            await _signalrConnection.InvokeAsync("SayHello");
        }

        [Then(@"an event should raise")]
        public async Task ThenAnEventShouldRaise()
        {
            await Task.Delay(1000); // must wait a bit

            _wasHelloEventCalled.Should().BeTrue();
        }

    }
}
