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
using System.Linq;
using Microsoft.Practices.ObjectBuilder2.Tests.TestDoubles;
using Microsoft.Practices.ObjectBuilder2.Tests.TestObjects;
using Microsoft.Practices.Unity.TestSupport;
#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Microsoft.Practices.ObjectBuilder2.Tests
{
    /// <summary>
    /// Tests for the default ConstructorSelectorPolicy
    /// </summary>
    [TestClass]
    public class ConstructorSelectorFixture
    {
        [TestMethod]
        public void SelectorPicksDefaultConstructor()
        {
            IConstructorSelectorPolicy policy = CreateSelector();
            ConstructorInfo expectedCtor = GetConstructor<object>();
            MockBuilderContext context = GetContext<object>();
            SelectedConstructor result = policy.SelectConstructor(context, new PolicyList());

            Assert.AreEqual(expectedCtor, result.Constructor);
        }

        [TestMethod]
        public void SelectorPicksConstructorWithAttribute()
        {
            IConstructorSelectorPolicy policy = CreateSelector();
            ConstructorInfo expected = GetConstructor<ObjectWithMarkedConstructor>(typeof(string));

            SelectedConstructor result = policy.SelectConstructor(GetContext<ObjectWithMarkedConstructor>(), new PolicyList());

            Assert.AreEqual(expected, result.Constructor);
        }

        [TestMethod]
        public void SelectorPicksLongestConstructor()
        {
            IConstructorSelectorPolicy policy = CreateSelector();
            ConstructorInfo expected = GetConstructor<ObjectWithMultipleConstructors>(
                typeof(int), typeof(string));

            SelectedConstructor result =
                policy.SelectConstructor(GetContext<ObjectWithMultipleConstructors>(), new PolicyList());

            Assert.AreEqual(expected, result.Constructor);
        }

        [TestMethod]
        public void SelectorThrowsIfConstructorsAreAmbiguous()
        {
            IConstructorSelectorPolicy policy = CreateSelector();

            try
            {
                policy.SelectConstructor(GetContext<ObjectWithAmbiguousConstructors>(), new PolicyList());
            }
            catch(InvalidOperationException)
            {
                // If we got here we're ok
                return;
            }
            Assert.Fail("Expected exception did not occur");
        }

        [TestMethod]
        public void SelectorPicksMarkedConstructorEvenIfOtherwiseAmbiguous()
        {
            IConstructorSelectorPolicy policy = CreateSelector();
            ConstructorInfo expected = GetConstructor<ObjectWithAmbiguousMarkedConstructor>(
                typeof(string), typeof(string), typeof(int));

            SelectedConstructor result =
                policy.SelectConstructor(GetContext<ObjectWithAmbiguousMarkedConstructor>(), new PolicyList());

            Assert.AreEqual(expected, result.Constructor);
        }

        private static ConstructorSelectorPolicy<InjectionConstructorAttribute> CreateSelector()
        {
            return new ConstructorSelectorPolicy<InjectionConstructorAttribute>();
        }

        private static ConstructorInfo GetConstructor<T>(params Type[] paramTypes)
        {
            return typeof(T).GetTypeInfo().DeclaredConstructors.First(c => ParameterTypesMatch(c.GetParameters(), paramTypes));
        }

        private static bool  ParameterTypesMatch(ParameterInfo[] parameters, Type[] paramTypesToMatch)
        {
            if (parameters.Length != paramTypesToMatch.Length) return false;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != paramTypesToMatch[i]) return false;
            }

            return true;
        }

        private static MockBuilderContext GetContext<T>()
        {
            MockBuilderContext context = new MockBuilderContext();
            context.BuildKey = new NamedTypeBuildKey<T>();
            return context;
        }
    }
}
