using System;
using System.Collections.Generic;

namespace POSSystem.Services
{
    /// <summary>
    /// A factory class for managing service instances as singletons.
    /// </summary>
    public static class ServiceFactory
    {
        private static Dictionary<Type, Type> _choices = new Dictionary<Type, Type>();
        private static Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a child type for a given parent type.
        /// </summary>
        /// <typeparam name="TParent">The parent type to register.</typeparam>
        /// <typeparam name="TChild">The child type to associate with the parent type.</typeparam>
        /// <exception cref="InvalidOperationException">Thrown if the parent type has already been registered.</exception>
        public static void Register<TParent, TChild>() where TChild : TParent
        {
            Type parentType = typeof(TParent);

            if (_choices.ContainsKey(parentType))
            {
                throw new InvalidOperationException($"The type {parentType.Name} has already been registered.");
            }

            _choices.Add(parentType, typeof(TChild));
        }

        /// <summary>
        /// Gets the singleton instance of the child type associated with the given parent type.
        /// </summary>
        /// <typeparam name="TParent">The parent type whose child instance is required.</typeparam>
        /// <returns>The singleton instance of the child type.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the parent type has not been registered.</exception>
        public static TParent GetChildOf<TParent>()
        {
            Type parentType = typeof(TParent);

            if (!_choices.TryGetValue(parentType, out Type childType))
            {
                throw new InvalidOperationException($"The type {parentType.Name} has not been registered.");
            }


            if (!_instances.TryGetValue(parentType, out object instance))
            {
                instance = Activator.CreateInstance(childType);
                _instances.Add(parentType, instance);
            }

            return (TParent)instance;
        }
    }
}