using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Core
{
    public static class Monadas
    {
        /// <summary>
        /// Monada with
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="input"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TResult With<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator)
            where TInput : class
        {
            if (input == null)
            {
                return default(TResult);
            }

            return evaluator(input);
        }

        /// <summary>
        /// Monada Return
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="input"></param>
        /// <param name="evaluator"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TResult Return<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator,
            TResult defaultValue) where TInput : class
        {
            if (input == null)
            {
                return defaultValue;
            }

            return evaluator(input);
        }

        /// <summary>
        /// Monada IsNull
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNull<TInput>(this TInput input) where TInput : class
        {
            return input != null;
        }

        /// <summary>
        /// Monada IsNotNull
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotNull<TInput>(this TInput input) where TInput : class
        {
            return !input.IsNull();
        }

        /// <summary>
        /// Monada IsNotNullOrEmpty
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Monada if
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="input"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static TInput If<TInput>(this TInput input, Predicate<TInput> predicate) where TInput : class
        {
            if (input == null)
            {
                return null;
            }

            return predicate(input) ? input : null;
        }

        /// <summary>
        /// Mondata do
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="input"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static TInput Do<TInput>(this TInput input, Action<TInput> action) where TInput : class
        {
            if (input == null)
            {
                return null;
            }

            action(input);
            return input;
        }
    }
}