using System;
using System.Linq.Expressions;

namespace Infrastructure.Extensions
{
    public static class FuncExtensions
    {
        public static Func<TOutput> ConvertFunc<TInput, TOutput>(this Func<TInput> inputFunc)
        {
            Expression<Func<TInput>> inputFuncExpression = () => inputFunc();
            return inputFuncExpression.ConvertExpression<TInput, TOutput>().Compile();
        }

        public static Expression<Func<TOutput>> ConvertExpression<TInput, TOutput>(this Expression<Func<TInput>> inputFuncExpression)
        {
            // Add the boxing operation, but get a weakly typed expression
            Expression converted = Expression.Convert(inputFuncExpression.Body, typeof(TOutput));
            // Use Expression.Lambda to get back to strong typing
            return Expression.Lambda<Func<TOutput>>(converted, inputFuncExpression.Parameters);
        }
    }
}
