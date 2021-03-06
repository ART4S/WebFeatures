﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WebFeatures.Application.Interfaces.Requests;

namespace WebFeatures.Infrastructure.Requests
{
	internal class RequestMediator : IRequestMediator
	{
		private static readonly ConcurrentDictionary<Type, object> PipelinesCashe
			= new ConcurrentDictionary<Type, object>();

		private readonly IServiceProvider _services;

		public RequestMediator(IServiceProvider services)
		{
			_services = services;
		}

		public Task<TResponse> SendAsync<TResponse>(
			IRequest<TResponse> request,
			CancellationToken cancellationToken = default)
		{
			var pipeline = (Pipeline<TResponse>) PipelinesCashe.GetOrAdd(
				request.GetType(),
				_ =>
				{
					Type pipelineType = typeof(Pipeline<,>).MakeGenericType(
						request.GetType(),
						typeof(TResponse));

					return Activator.CreateInstance(pipelineType);
				});

			return pipeline.HandleAsync(request, _services, cancellationToken);
		}
	}

	internal abstract class Pipeline<TResponse>
	{
		public abstract Task<TResponse> HandleAsync(
			IRequest<TResponse> request,
			IServiceProvider services,
			CancellationToken cancellationToken);
	}

	internal class Pipeline<TRequest, TResponse> : Pipeline<TResponse>
		where TRequest : IRequest<TResponse>
	{
		public override Task<TResponse> HandleAsync(
			IRequest<TResponse> request,
			IServiceProvider services,
			CancellationToken cancellationToken)
		{
			IRequestHandler<TRequest, TResponse> handler = 
				services.GetService<IRequestHandler<TRequest, TResponse>>() ?? 
				throw new InvalidOperationException("Handler hasn't been registered");

			RequestDelegate<Task<TResponse>> pipeline =
				() => handler.HandleAsync((TRequest) request, cancellationToken);

			IEnumerable<IRequestMiddleware<TRequest, TResponse>> middlewares =
				services.GetServices<IRequestMiddleware<TRequest, TResponse>>().Reverse();

			foreach (IRequestMiddleware<TRequest, TResponse> middleware in middlewares)
			{
				RequestDelegate<Task<TResponse>> next = pipeline; // for closure

				pipeline = () => middleware.HandleAsync((TRequest) request, next, cancellationToken);
			}

			return pipeline();
		}
	}
}
