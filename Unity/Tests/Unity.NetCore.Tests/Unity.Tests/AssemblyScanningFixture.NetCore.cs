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
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity.TestSupport;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Microsoft.Practices.Unity.Tests
{
    partial class AssemblyScanningFixture
    {
        [TestMethod]
        public void GetsTypesFromAssembliesLoadedFromApplicationFolderSkippingUnityAssembliesByDefault()
        {
            var typesByAssembly = AllClasses.FromApplication().GroupBy(t => t.GetTypeInfo().Assembly).ToDictionary(g => g.Key);

            Assert.IsTrue(typesByAssembly.ContainsKey(typeof(Microsoft.Practices.Unity.Tests.TestNetAssembly.PublicClass1).GetTypeInfo().Assembly));
            Assert.IsTrue(typesByAssembly.ContainsKey(typeof(AssemblyScanningFixture).GetTypeInfo().Assembly));
            Assert.IsFalse(typesByAssembly.ContainsKey(typeof(IUnityContainer).GetTypeInfo().Assembly));
        }

        [TestMethod]
        public void GetsTypesFromAssembliesLoadedFromApplicationFolderIncludingUnityAssembliesIfOverridden()
        {
            var typesByAssembly = AllClasses.FromApplication(includeUnityAssemblies: true).GroupBy(t => t.GetTypeInfo().Assembly).ToDictionary(g => g.Key);

            Assert.IsTrue(typesByAssembly.ContainsKey(typeof(Microsoft.Practices.Unity.Tests.TestNetAssembly.PublicClass1).GetTypeInfo().Assembly));
            Assert.IsTrue(typesByAssembly.ContainsKey(typeof(AssemblyScanningFixture).GetTypeInfo().Assembly));
            Assert.IsTrue(typesByAssembly.ContainsKey(typeof(IUnityContainer).GetTypeInfo().Assembly));
        }

        [TestMethod]
        public void GettigTypesFromAssembliesLoadedFromApplicationFolderWithoutSkippingErrorsThrows()
        {
            AssertExtensions.AssertException<AggregateException>(
                () => AllClasses.FromApplication(skipOnError: false),
                ae => Assert.IsTrue(ae.InnerException is BadImageFormatException));
        }
    }
}
