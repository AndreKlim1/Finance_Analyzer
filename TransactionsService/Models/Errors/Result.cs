

namespace TransactionsService.Models.Errors
{
    public class Result
    {
        protected Result()
        {
            IsSuccess = true;
            Error = Error.None;
        }

        protected Result(Error error)
        {
            IsSuccess = false;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result Success() => new();
        public static Result Failure(Error error) => new(error);

    }

    public class Result<T> : Result
    {
        private readonly T? _value;

        public T? Value
        {
            get => IsSuccess ? _value : throw new InvalidOperationException();
            private init => _value = value;
        }

        private Result(T? value)
        {
            _value = value;
        }

        private Result(Error error) : base(error)
        {
            _value = default;
        }

        public static Result<T> Success(T value) => new(value);
        public new static Result<T> Failure(Error error) => new(error);
    }


}
