using Company.Customers.Infra.CrossCutting.Utils.Interfaces;
using System.Collections.Generic;

namespace Company.Customers.Infra.CrossCutting.Utils
{
    public static class Result
    {
        public static IOperation<T> CreateSuccess<T>()
        {
            return new OperationSuccess<T>(default);
        }

        public static IOperation<T> CreateSuccess<T>(T value)
        {
            return new OperationSuccess<T>(value);
        }

        public static IOperation<T> CreateFailure<T>(in string message, MessageDetail detail)
        {
            return new OperationFail<T>(message,detail);
        }

        public static IOperation<T> CreateFailure<T>(in string message)
        {
            return new OperationFail<T>(message, default(MessageDetail));
        }

        public static IOperation<T> CreateFailure<T>(in string message, List<MessageDetail> details)
        {
            return new OperationFail<T>(message, details);
        }

    }
}
