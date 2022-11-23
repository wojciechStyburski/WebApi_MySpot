using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Framework;

public class ServiceCollectionTests
{
    [Fact]
    public void test()
    {
        var serviceCollection = new ServiceCollection();
        //serviceCollection.AddTransient<IMessenger, Messenger>();
        //serviceCollection.AddSingleton<IMessenger, Messenger>();
        //serviceCollection.AddScoped<IMessenger, Messenger>();
        
        serviceCollection.AddScoped<IMessenger, Messenger>();
        serviceCollection.AddScoped<IMessenger, Messenger2>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        /*
        var messenger = serviceProvider.GetService<IMessenger>();
        messenger.Send();
        

        using (var scope = serviceProvider.CreateScope())
        {
            var messenger = scope.ServiceProvider.GetRequiredService<IMessenger>();
            messenger.Send();

            var messenger2 = scope.ServiceProvider.GetRequiredService<IMessenger>();
            messenger2.Send();

            messenger.ShouldNotBeNull();
            messenger2.ShouldNotBeNull();
        }
        

        using (var scope = serviceProvider.CreateScope())
        {
            var messenger = scope.ServiceProvider.GetRequiredService<IMessenger>();
            messenger.Send();

            var messenger2 = scope.ServiceProvider.GetRequiredService<IMessenger>();
            messenger2.Send();

            messenger.ShouldNotBeNull();
            messenger2.ShouldNotBeNull();
        }
        */

        var messenger = serviceProvider.GetService<IMessenger>();
        messenger.Send();

        var messengers = serviceProvider.GetService<IEnumerable<IMessenger>>();
        var messengers2 = serviceProvider.GetServices<IMessenger>();

    }

    private interface IMessenger
    {
        void Send();
    }

    private class Messenger : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();
        public void Send() => Console.WriteLine($"Sending a message... [{_id}]");
    }

    private class Messenger2 : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();
        public void Send() => Console.WriteLine($"Sending a message... [{_id}]");
    }

}