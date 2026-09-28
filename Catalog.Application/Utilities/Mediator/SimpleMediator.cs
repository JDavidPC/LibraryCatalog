using Catalog.Application.Exceptions;
using System.Reflection;

namespace Catalog.Application.Utilities.Mediator
{
    public class SimpleMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public SimpleMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            Type useCaseType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));

            object? useCase = _serviceProvider.GetService(useCaseType);

            if (useCase is null)
            {
                throw new MediatorException($"No se encontró un handler para {request.GetType().Name}");
            }

            MethodInfo method = useCaseType.GetMethod("Handle")!;

            // DoNotWrapExceptions evita que las excepciones del caso de uso
            // (ej: NotFoundException) lleguen envueltas en un TargetInvocationException.
            Task<TResponse> task = (Task<TResponse>)method.Invoke(useCase,
                                                                  BindingFlags.DoNotWrapExceptions,
                                                                  null,
                                                                  new object[] { request },
                                                                  null)!;

            return await task;
        }
    }
}
