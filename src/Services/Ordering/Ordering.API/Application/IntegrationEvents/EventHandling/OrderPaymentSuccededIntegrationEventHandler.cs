﻿namespace Ordering.API.Application.IntegrationEvents.EventHandling
{
    using MediatR;
    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.eShopOnContainers.Services.Ordering.API;
    using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
    using Microsoft.Extensions.Logging;
    using Ordering.API.Application.Commands;
    using Ordering.API.Application.IntegrationEvents.Events;
    using Serilog.Context;
    using System;
    using System.Threading.Tasks;

    public class OrderPaymentSuccededIntegrationEventHandler : 
        IIntegrationEventHandler<OrderPaymentSuccededIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderPaymentSuccededIntegrationEventHandler> _logger;

        public OrderPaymentSuccededIntegrationEventHandler(
            IMediator mediator,
            ILogger<OrderPaymentSuccededIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(OrderPaymentSuccededIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppShortName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppShortName} - ({@IntegrationEvent})", @event.Id, Program.AppShortName, @event);

                var command = new SetPaidOrderStatusCommand(@event.OrderId);
                await _mediator.Send(command);
            }
        }
    }
}