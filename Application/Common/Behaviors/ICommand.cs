using MediatR;

namespace Application.Common.Behaviors
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
