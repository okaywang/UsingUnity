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

using Microsoft.Practices.Unity.Configuration;

namespace Microsoft.Practices.Unity.InterceptionExtension.Configuration
{
    /// <summary>
    /// Section extension class used to add the elements needed to configure
    /// Unity interception to the configuration schema.
    /// </summary>
    public class InterceptionConfigurationExtension : SectionExtension
    {
        /// <summary>
        /// Add the extensions to the section via the context.
        /// </summary>
        /// <param name="context">Context object that can be used to add elements and aliases.</param>
        [System.Security.SecuritySafeCritical]
        public override void AddExtensions(SectionExtensionContext context)
        {
            AddAliases(context);

            AddElements(context);
        }

        private static void AddElements(SectionExtensionContext context)
        {
            context.AddElement<InterceptionElement>("interception");
            context.AddElement<InterceptorElement>("interceptor");
            context.AddElement<AddInterfaceElement>("addInterface");
            context.AddElement<InterceptionBehaviorElement>("interceptionBehavior");
            context.AddElement<PolicyInjectionElement>("policyInjection");
            context.AddElement<InterceptorsElement>("interceptors");
        }

        [System.Security.SecurityCritical]
        private static void AddAliases(SectionExtensionContext context)
        {
            context.AddAlias<Interception>("Interception");
            context.AddAlias<IMatchingRule>("IMatchingRule");
            context.AddAlias<ICallHandler>("ICallHandler");

            context.AddAlias<AssemblyMatchingRule>("AssemblyMatchingRule");
            context.AddAlias<CustomAttributeMatchingRule>("CustomAttributeMatchingRule");
            context.AddAlias<MemberNameMatchingRule>("MemberNameMatchingRule");
            context.AddAlias<NamespaceMatchingRule>("NamespaceMatchingRule");
            context.AddAlias<ParameterTypeMatchingRule>("ParameterTypeMatchingRule");
            context.AddAlias<PropertyMatchingRule>("PropertyMatchingRule");
            context.AddAlias<TagAttributeMatchingRule>("TagAttributeMatchingRule");
            context.AddAlias<TypeMatchingRule>("TypeMatchingRule");

            context.AddAlias<VirtualMethodInterceptor>("VirtualMethodInterceptor");
            context.AddAlias<InterfaceInterceptor>("InterfaceInterceptor");
            context.AddAlias<TransparentProxyInterceptor>("TransparentProxyInterceptor");

            context.AddAlias<IInterceptionBehavior>("IInterceptionBehavior");
            context.AddAlias<PolicyInjectionBehavior>("PolicyInjectionBehavior");
        }
    }
}
