using System;
using System.Collections.Generic;

namespace POSSystem.Services
{
    /// <summary>
    /// A factory class for managing service instances as singletons.
    /// </summary>
    public class ServiceFactory
    {
        private static Dictionary<string, Type> _choices = new Dictionary<string, Type>();
        private static Dictionary<string, object> _instances = new Dictionary<string, object>();

        /// <summary>
        /// Registers a child type for a given parent type.
        /// </summary>
        /// <param name="parent">The parent type to register.</param>
        /// <param name="child">The child type to associate with the parent type.</param>
        /// <exception cref="InvalidOperationException">Thrown if the parent type has already been registered.</exception>
        public static void Register(Type parent, Type child)
        {
            string parentName = parent.Name;

            if (_choices.ContainsKey(parentName))
            {
                throw new InvalidOperationException($"The type {parentName} has already been registered.");
            }

            _choices.Add(parentName, child);
        }

        /// <summary>
        /// Gets the singleton instance of the child type associated with the given parent type.
        /// </summary>
        /// <param name="parent">The parent type whose child instance is required.</param>
        /// <returns>The singleton instance of the child type.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the parent type has not been registered.</exception>
        public static object GetChildOf(Type parent)
        {
            string parentName = parent.Name;

            if (!_choices.ContainsKey(parentName))
            {
                throw new InvalidOperationException($"The type {parentName} has not been registered.");
            }

            if (!_instances.ContainsKey(parentName))
            {
                Type type = _choices[parentName];
                object instance = Activator.CreateInstance(type);
                _instances.Add(parentName, instance);
            }

            return _instances[parentName];
        }
    }
}