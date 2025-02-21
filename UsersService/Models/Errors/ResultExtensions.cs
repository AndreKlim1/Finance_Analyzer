namespace UsersService.Models.Errors
{
    public static class ResultExtension
    {
        public static void Match(this Result result, Action onSuccess, Action<Error> onFailure)
        {
            if (result.IsSuccess)
            {
                onSuccess();
            }
            else
            {
                onFailure(result.Error);
            }
        }

        public static void Match<T>(this Result<T> result, Action<T> onSuccess, Action<Error> onFailure)
        {
            if (result.IsSuccess)
            {
                onSuccess(result.Value);
            }
            else
            {
                onFailure(result.Error);
            }
        }

        public static TResult Match<TResult>(this Result result, Func<TResult> onSuccess, Func<Error, TResult> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result.Error);
        }

        public static TResult Match<T, TResult>(this Result<T> result, Func<T, TResult> onSuccess,
            Func<Error, TResult> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
        }
    }
}
