using ErrorOr;

namespace Application.Common.Extensions
{
    public static class ErrorOrExtensions
    {
        public static ErrorOr<TResult> Map<T, TResult>(this ErrorOr<T> result, Func<T, TResult> func)
        {
            return result.IsError ? result.Errors : func(result.Value);
        }

        public static async Task<ErrorOr<TResult>> MapAsync<T, TResult>(this Task<ErrorOr<T>> task, Func<T, TResult> func)
        {
            var result = await task;
            return result.IsError ? result.Errors : func(result.Value);
        }

        public static ErrorOr<TResult> Bind<T, TResult>(this ErrorOr<T> result, Func<T, ErrorOr<TResult>> func)
        {
            return result.IsError ? result.Errors : func(result.Value);
        }

        public static async Task<ErrorOr<TResult>> BindAsync<T, TResult>(this Task<ErrorOr<T>> task, Func<T, Task<ErrorOr<TResult>>> func)
        {
            var result = await task;
            return result.IsError ? result.Errors : await func(result.Value);
        }
    }
}
