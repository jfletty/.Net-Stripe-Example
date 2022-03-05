using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe.Demo.Console;
using StripeExample.Demo.Services.Models;
using StripeExample.Demo.Services.Services;
using StripeExample.Demo.Services.Services.Interfaces;

var host = Host.CreateDefaultBuilder(args).Build();
var config = host.Services.GetRequiredService<IConfiguration>();

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .Configure<StripeConfig>(config)
    .AddSingleton<IManageCustomer, ManageCustomer>()
    .AddSingleton<IManageSubscription, ManageSubscription>()
    .BuildServiceProvider();

Console.WriteLine("Starting Application");

// Customer Management
var manageCustomer = serviceProvider.GetService<IManageCustomer>();

var exampleCustomer = ExampleCustomerDTO.Build();
var createCustomer = await manageCustomer!.CreateOrUpdateCustomerAsync(exampleCustomer);
var getCustomer = await manageCustomer.GetAsync(createCustomer.Id);

getCustomer.Name = "John Doe's Partner";
await manageCustomer.CreateOrUpdateCustomerAsync(getCustomer);
await manageCustomer.GetAllAsync();
await manageCustomer.DeleteCustomerAsync(getCustomer.Id);

// Subscription Management
var manageSubscription = serviceProvider.GetService<IManageSubscription>();

var exampleSubscription = ExampleSubscriptionDTO.Build(createCustomer.Id);
var createSubscription = await manageSubscription!.CreateOrUpdateAsync(exampleSubscription);
await manageSubscription.GetAsync(createSubscription.ExternalId);
await manageSubscription.GetAllAsync();