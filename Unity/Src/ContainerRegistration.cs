﻿//===============================================================================
// Microsoft patterns & practices
// Unity Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.Unity
{
    /// <summary>
    /// Class that returns information about the types registered in a container.
    /// </summary>
    public class ContainerRegistration
    {
        private readonly NamedTypeBuildKey buildKey;

        internal ContainerRegistration(Type registeredType, string name, IPolicyList policies)
        {
            buildKey = new NamedTypeBuildKey(registeredType, name);
            MappedToType = GetMappedType(policies);
            LifetimeManagerType = GetLifetimeManagerType(policies);
            LifetimeManager = GetLifetimeManager(policies);
        }

        /// <summary>
        /// The type that was passed to the <see cref="IUnityContainer.RegisterType"/> method
        /// as the "from" type, or the only type if type mapping wasn't done.
        /// </summary>
        public Type RegisteredType { get { return buildKey.Type; } }

        /// <summary>
        /// The type that this registration is mapped to. If no type mapping was done, the
        /// <see cref="RegisteredType"/> property and this one will have the same value.
        /// </summary>
        public Type MappedToType { get; private set; }

        /// <summary>
        /// Name the type was registered under. Null for default registration.
        /// </summary>
        public string Name { get { return buildKey.Name; } }

        /// <summary>
        /// The registered lifetime manager instance.
        /// </summary>
        public Type LifetimeManagerType { get; private set; }

        /// <summary>
        /// The lifetime manager for this registration.
        /// </summary>
        /// <remarks>
        /// This property will be null if this registration is for an open generic.</remarks>
        public LifetimeManager LifetimeManager { get; private set; }

        private Type GetMappedType(IPolicyList policies)
        {
            var mappingPolicy = policies.Get<IBuildKeyMappingPolicy>(buildKey);
            if (mappingPolicy != null)
            {
                return mappingPolicy.Map(buildKey, null).Type;
            }
            return buildKey.Type;
        }

        private Type GetLifetimeManagerType(IPolicyList policies)
        {
            var key = new NamedTypeBuildKey(MappedToType, Name);
            var lifetime = policies.Get<ILifetimePolicy>(key);
            
            if(lifetime != null)
            {
                return lifetime.GetType();
            }

            if (MappedToType.GetTypeInfo().IsGenericType)
            {
                var genericKey = new NamedTypeBuildKey(MappedToType.GetGenericTypeDefinition(), Name);
                var lifetimeFactory = policies.Get<ILifetimeFactoryPolicy>(genericKey);
                if(lifetimeFactory != null)
                {
                    return lifetimeFactory.LifetimeType;
                }
            }

            return typeof (TransientLifetimeManager);
        }

        private LifetimeManager GetLifetimeManager(IPolicyList policies)
        {
            var key = new NamedTypeBuildKey(MappedToType, Name);
            return (LifetimeManager)policies.Get<ILifetimePolicy>(key);
        }
    }
}
