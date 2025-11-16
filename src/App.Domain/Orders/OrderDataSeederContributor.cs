using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace App.Orders
{
    public class OrderDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly ILogger<OrderDataSeederContributor> _logger;

        public OrderDataSeederContributor(
            IRepository<Order, Guid> orderRepository,
            ILogger<OrderDataSeederContributor> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            var count = await _orderRepository.GetCountAsync();
            
            if (count > 0)
            {
                _logger.LogInformation("Skip seed.");
                return;
            }

            var random = new Random();
            var statuses = new[] { OrderStatus.Pending, OrderStatus.Processing, OrderStatus.Shipped, OrderStatus.Delivered, OrderStatus.Cancelled };
            var firstNames = new[] { "Juan", "María", "Carlos", "Ana", "Luis", "Carmen", "Pedro", "Laura", "Miguel", "Sofia" };
            var lastNames = new[] { "García", "Rodríguez", "Martínez", "López", "González", "Pérez", "Sánchez", "Ramírez", "Torres", "Flores" };
            var cities = new[] { "La Paz", "Santa Cruz", "Cochabamba", "Sucre", "Oruro", "Potosí", "Tarija", "Beni", "Pando" };
            var streets = new[] { "Av. 6 de Agosto", "Calle Comercio", "Av. Ballivián", "Calle Murillo", "Av. Arce", "Calle Potosí" };

            var orders = new List<Order>();

            for (int i = 1; i <= 50; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var customerName = $"{firstName} {lastName}";
                var customerEmail = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com";

                var orderDate = DateTime.UtcNow.AddDays(-random.Next(1, 180));
                var status = statuses[random.Next(statuses.Length)];

                DateTime? shipDate = null;
                if (status == OrderStatus.Shipped || status == OrderStatus.Delivered)
                {
                    shipDate = orderDate.AddDays(random.Next(1, 7));
                }

                var totalAmount = Math.Round((decimal)(random.NextDouble() * 1000 + 50), 2);

                var city = cities[random.Next(cities.Length)];
                var street = streets[random.Next(streets.Length)];
                var number = random.Next(100, 9999);
                var shippingAddress = $"{street} #{number}, {city}, Bolivia";

                var notes = i % 3 == 0 ? $"Nota de prueba para la orden {i}" : null;

                var order = new Order(
                    Guid.NewGuid(),
                    $"ORD-{DateTime.UtcNow:yyyyMMdd}-{i:D5}",
                    customerName,
                    customerEmail,
                    orderDate,
                    status,
                    totalAmount,
                    shippingAddress,
                    notes,
                    shipDate
                );

                orders.Add(order);
            }
            await _orderRepository.InsertManyAsync(orders, autoSave: true);
        }
    }
}