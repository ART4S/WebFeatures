﻿namespace WebFeatures.Requests
{
    public interface IRequest<TResponse> { }
    public interface IQuery<TResult> : IRequest<TResult> { }
    public interface ICommand<TResult> : IRequest<TResult> { }
}
