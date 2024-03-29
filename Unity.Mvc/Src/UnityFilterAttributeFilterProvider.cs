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

using System.Collections.Generic;
using System.Web.Mvc;

namespace Microsoft.Practices.Unity.Mvc
{
    /// <summary>
    /// Defines a filter provider for filter attributes that support injection of Unity dependencies.
    /// </summary>
    public class UnityFilterAttributeFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityFilterAttributeFilterProvider"/> class.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/> that will be used to inject the filters.</param>
        public UnityFilterAttributeFilterProvider(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets a collection of custom action attributes, and injects them using a Unity container.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>A collection of custom action attributes.</returns>
        protected override IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var list = base.GetActionAttributes(controllerContext, actionDescriptor);

            foreach (var item in list)
            {
                this.container.BuildUp(item.GetType(), item);
            }

            return list;
        }

        /// <summary>
        /// Gets a collection of controller attributes, and injects them using a Unity container.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>A collection of controller attributes.</returns>
        protected override IEnumerable<FilterAttribute> GetControllerAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var list = base.GetControllerAttributes(controllerContext, actionDescriptor);

            foreach (var item in list)
            {
                this.container.BuildUp(item.GetType(), item);
            }

            return list;
        }
    }
}
