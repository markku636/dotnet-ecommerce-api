using System;

namespace EC226.Utility.Helper
{
    public class Guard
    {
        public Guard()
        {
        }
        /// <summary>
        /// Throws an exception of type <typeparamref name="TException" /> with the specified message
        /// when the assertion statement is true.
        /// </summary>
        /// <typeparam name="TException">The type of exception to throw.</typeparam>
        /// <param name="assertion">The assertion to evaulate. If true then the <typeparamref name="TException" /> exception is thrown.</param>
        /// <param name="message">string. The exception message to throw.</param>
        public static void Against<TException>(bool assertion, string message, Action action = null)
            where TException : Exception
        {
            if (assertion)
            {
                action?.Invoke();
                System.Type type = typeof(TException);
                object[] objArray = new object[] { message };
                throw ((TException)Activator.CreateInstance(type, objArray));
            }
        }

        /// <summary>
        /// Throws an exception of type <typeparamref name="TException" /> with the specified message
        /// when the assertion
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="assertion"></param>
        /// <param name="message"></param>
        public static void Against<TException>(Func<bool> assertion, string message, Action action = null)
            where TException : Exception
        {
            if (assertion())
            {
                action?.Invoke();
                System.Type type = typeof(TException);
                object[] objArray = new object[] { message };
                throw ((TException)Activator.CreateInstance(type, objArray));
            }
        }

        /// <summary>
        /// Throws a <see cref="T:System.InvalidOperationException" /> when the specified object
        /// instance does not implement the <typeparamref name="TInterface" /> interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface type the object instance should implement.</typeparam>
        /// <param name="instance">The object insance to check if it implements the <typeparamref name="TInterface" /> interface</param>
        /// <param name="message">string. The exception message to throw.</param>
        public static void Implements<TInterface>(object instance, string message)
        {
            Guard.Implements<TInterface>(instance.GetType(), message);
        }

        /// <summary>
        /// Throws an <see cref="T:System.InvalidOperationException" /> when the specified type does not
        /// implement the <typeparamref name="TInterface" /> interface.
        /// </summary>
        /// <typeparam name="TInterface">The interface type that the <paramref name="type" /> should implement.</typeparam>
        /// <param name="type">The <see cref="T:System.Type" /> to check if it implements from <typeparamref name="TInterface" /> interface.</param>
        /// <param name="message">string. The exception message to throw.</param>
        public static void Implements<TInterface>(System.Type type, string message)
        {
            if (!typeof(TInterface).IsAssignableFrom(type))
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Throws a <see cref="T:System.InvalidOperationException" /> when the specified object
        /// instance does not inherit from <typeparamref name="TBase" /> type.
        /// </summary>
        /// <typeparam name="TBase">The base type to check for.</typeparam>
        /// <param name="instance">The object to check if it inherits from <typeparamref name="TBase" /> type.</param>
        /// <param name="message">string. The exception message to throw.</param>
        public static void InheritsFrom<TBase>(object instance, string message)
        where TBase : System.Type
        {
            Guard.InheritsFrom<TBase>(instance.GetType(), message);
        }

        /// <summary>
        /// Throws a <see cref="T:System.InvalidOperationException" /> when the specified type does not
        /// inherit from the <typeparamref name="TBase" /> type.
        /// </summary>
        /// <typeparam name="TBase">The base type to check for.</typeparam>
        /// <param name="type">The <see cref="T:System.Type" /> to check if it inherits from <typeparamref name="TBase" /> type.</param>
        /// <param name="message">string. The exception message to throw.</param>
        public static void InheritsFrom<TBase>(System.Type type, string message)
        {
            if (type.BaseType != typeof(TBase))
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Throws an exception if an instance of an object is not equal to another object instance.
        /// </summary>
        /// <typeparam name="TException">The type of exception to throw when the guard check evaluates false.</typeparam>
        /// <param name="compare">The comparison object.</param>
        /// <param name="instance">The object instance to compare with.</param>
        /// <param name="message">string. The message of the exception.</param>
        public static void IsEqual<TException>(object compare, object instance, string message)
        where TException : Exception
        {
            if (compare != instance)
            {
                System.Type type = typeof(TException);
                object[] objArray = new object[] { message };
                throw((TException)Activator.CreateInstance(type, objArray));
            }
        }
        public static void TypeOf<TType>(object instance, string message)
        {
            if (!(instance is TType))
            {
                throw new InvalidOperationException(message);
            }
        }
    
    }
}