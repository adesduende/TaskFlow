using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Infrastructure.Mediator
{
    internal class TaskFlowMediator : IMediator
    {
        private readonly IServiceProvider _provider;

        public TaskFlowMediator(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }
        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

            var handler = _provider.GetService(handlerType);
            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for request type {request.GetType()} not found.");
            }

            var method = handlerType.GetMethod("HandleAsync");
            if (method == null)
            {
                throw new InvalidOperationException($"HandleAsync method not found on handler type {handlerType}.");
            }

            var result =  method.Invoke(handler, new object[] { request });

            return await (Task<TResponse>)result;

        }
    }
}
