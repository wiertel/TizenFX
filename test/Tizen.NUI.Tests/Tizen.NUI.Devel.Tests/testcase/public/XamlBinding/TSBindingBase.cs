﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;
using Tizen.NUI.Xaml;

namespace Tizen.NUI.Devel.Tests
{
    using tlog = Tizen.Log;

    [TestFixture]
    [Description("public/XamlBinding/BindingBase")]
    internal class PublicBindingBaseTest
    {
        private const string tag = "NUITEST";

        internal class MyBindingBase : BindingBase
        {
            internal override BindingBase Clone()
            {
                return null;
            }

            public void TestThrowIfApplied()
            {
                ThrowIfApplied();
            }
        }

        [SetUp]
        public void Init()
        {
            tlog.Info(tag, "Init() is called!");
        }

        [TearDown]
        public void Destroy()
        {
            tlog.Info(tag, "Destroy() is called!");
        }

        [Test]
        [Category("P1")]
        [Description("BindingBase Mode")]
        [Property("SPEC", "Tizen.NUI.Binding.BindingBase.Mode A")]
        [Property("SPEC_URL", "-")]
        [Property("CRITERIA", "PRW")]
        public void ModeTest()
        {
            tlog.Debug(tag, $"ModeTest START");
            Binding.Binding t2 = new Binding.Binding("Test");
            Assert.IsNotNull(t2, "null Binding");
            Assert.IsInstanceOf<Binding.Binding>(t2, "Should return BindingBase instance.");
            var ret = t2.Mode;
            Assert.AreEqual(BindingMode.Default, ret, "Should be equal");

            tlog.Debug(tag, $"ModeTest END");
        }

        [Test]
        [Category("P1")]
        [Description("BindingBase StringFormat")]
        [Property("SPEC", "Tizen.NUI.Binding.BindingBase.StringFormat A")]
        [Property("SPEC_URL", "-")]
        [Property("CRITERIA", "PRW")]
        public void StringFormatTest()
        {
            tlog.Debug(tag, $"StringFormatTest START");
            Binding.Binding t2 = new Binding.Binding("Test");
            Assert.IsNotNull(t2, "null Binding");
            Assert.IsInstanceOf<Binding.Binding>(t2, "Should return BindingBase instance.");

            var ret = t2.StringFormat;
            Assert.IsNull(ret, "null StringFormat");

            tlog.Debug(tag, $"StringFormatTest END");
        }

        [Test]
        [Category("P1")]
        [Description("BindingBase TargetNullValue")]
        [Property("SPEC", "Tizen.NUI.Binding.BindingBase.TargetNullValue A")]
        [Property("SPEC_URL", "-")]
        [Property("CRITERIA", "PRW")]
        public void TargetNullValueTest()
        {
            tlog.Debug(tag, $"TargetNullValueTest START");
            Binding.Binding t2 = new Binding.Binding("Test");
            Assert.IsNotNull(t2, "null Binding");
            Assert.IsInstanceOf<Binding.Binding>(t2, "Should return Binding instance.");

            var ret = t2.TargetNullValue;
            Assert.IsNull(ret, "null TargetNullValue");

            tlog.Debug(tag, $"TargetNullValueTest END");
        }

        [Test]
        [Category("P1")]
        [Description("BindingBase FallbackValue")]
        [Property("SPEC", "Tizen.NUI.Binding.BindingBase.FallbackValue A")]
        [Property("SPEC_URL", "-")]
        [Property("CRITERIA", "PRW")]
        public void FallbackValueTest()
        {
            tlog.Debug(tag, $"FallbackValueTest START");
            Binding.Binding t2 = new Binding.Binding("Test");
            Assert.IsNotNull(t2, "null Binding");
            Assert.IsInstanceOf<Binding.Binding>(t2, "Should return Binding instance.");

            var ret = t2.FallbackValue;
            Assert.IsNull(ret, "null FallbackValue");

            tlog.Debug(tag, $"FallbackValueTest END");
        }

        [Test]
        [Category("P1")]
        [Description("BindingBase AllowChaining")]
        [Property("SPEC", "Tizen.NUI.Binding.BindingBase.AllowChaining  A")]
        [Property("SPEC_URL", "-")]
        [Property("CRITERIA", "PRW")]
        public void AllowChainingTest()
        {
            tlog.Debug(tag, $"AllowChainingTest START");
            try
            {
                Binding.Binding t2 = new Binding.Binding("Test");
                Assert.IsNotNull(t2, "null Binding");
                t2.AllowChaining = true;
                Assert.True(t2.AllowChaining, "Should be true");
                t2.AllowChaining = false;
                Assert.False(t2.AllowChaining, "Should be false"); ;
            }
            catch (Exception e)
            {
                Assert.Fail("Caught Exception" + e.ToString());
            }
            tlog.Debug(tag, $"AllowChainingTest END");
        }

        [Test]
        [Category("P1")]
        [Description("BindingBase Context ")]
        [Property("SPEC", "Tizen.NUI.Binding.BindingBase.Context   A")]
        [Property("SPEC_URL", "-")]
        [Property("CRITERIA", "PRW")]
        public void ContextTest()
        {
            tlog.Debug(tag, $"ContextTest START");
            try
            {
                Binding.Binding t2 = new Binding.Binding("Test");
                Assert.IsNotNull(t2, "null Binding");
                t2.Context = null;
                Assert.IsNull(t2.Context, "Should go here");
                
            }
            catch (Exception e)
            {
                Assert.Fail("Caught Exception" + e.ToString());
            }
            tlog.Debug(tag, $"ContextTest END");
        }

        [Test]
        [Category("P1")]
        [Description("BindingBase IsApplied ")]
        [Property("SPEC", "Tizen.NUI.Binding.BindingBase.IsApplied   A")]
        [Property("SPEC_URL", "-")]
        [Property("CRITERIA", "PRW")]
        public void IsAppliedTest()
        {
            tlog.Debug(tag, $"ContextTest START");
            try
            {
                Binding.Binding t2 = new Binding.Binding("Test");
                Assert.IsNotNull(t2, "null Binding");
                Assert.IsFalse(t2.IsApplied, "Should be false by default");

            }
            catch (Exception e)
            {
                Assert.Fail("Caught Exception" + e.ToString());
            }
            tlog.Debug(tag, $"ContextTest END");
        }

        [Test]
        [Category("P2")]
        [Description("BindingBase  ThrowIfApplied")]
        [Property("SPEC", "Tizen.NUI.Binding.BindingBase.ThrowIfApplied M")]
        [Property("SPEC_URL", "-")]
        [Property("CRITERIA", "MR")]
        public void ThrowIfApplied()
        {
            tlog.Debug(tag, $"ThrowIfApplied START");

            var t2 = new  MyBindingBase();
            Assert.IsNotNull(t2, "null Binding");
            t2.Apply(false);
            Assert.Throws<InvalidOperationException>(() => t2.TestThrowIfApplied());
           
            tlog.Debug(tag, $"ThrowIfApplied END");
        }

        [Test]
        [Category("P1")]
        [Description("BindingBase  Apply")]
        [Property("SPEC", "Tizen.NUI.Binding.BindingBase.Clone M")]
        [Property("SPEC_URL", "-")]
        [Property("CRITERIA", "MR")]
        public void CloneTest()
        {
            tlog.Debug(tag, $"CloneTest START");
            try
            {
                Binding.Binding t2 = new Binding.Binding("Test");
                Assert.IsNotNull(t2, "null Binding");
                Binding.Binding c = t2.Clone() as Binding.Binding;
                Assert.IsNotNull(c, "null Binding");

                t2.TargetNullValue = new object();
                var ret = t2.GetSourceValue(null, null);
                Assert.IsNotNull(ret, "Should not be null");
            }
            catch (Exception e)
            {
                Assert.Fail("Caught Exception" + e.ToString());
            }
            tlog.Debug(tag, $"CloneTest END");
        }

        [Test]
        [Category("P2")]
        [Description("BindingBase  DisableCollectionSynchronization")]
        [Property("SPEC", "Tizen.NUI.Binding.BindingBase.DisableCollectionSynchronization M")]
        [Property("SPEC_URL", "-")]
        [Property("CRITERIA", "MCST")]
        public void DisableCollectionSynchronization()
        {
            tlog.Debug(tag, $"DisableCollectionSynchronization START");

            var v = new View();
            var l = new List<int>();
            Assert.Throws<ArgumentNullException>(() => Binding.Binding.EnableCollectionSynchronization( l, v, null));
            Assert.Throws<ArgumentNullException>(() => Binding.Binding.EnableCollectionSynchronization(null, v, null));
            Assert.Throws<ArgumentNullException>(() => Binding.Binding.DisableCollectionSynchronization(null));

            Assert.Throws<ArgumentNullException>(() => Binding.Binding.TryGetSynchronizedCollection(null, out var csc));
            tlog.Debug(tag, $"DisableCollectionSynchronization END");
        }
    }
}