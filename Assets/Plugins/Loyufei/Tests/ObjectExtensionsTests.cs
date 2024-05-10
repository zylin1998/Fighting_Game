using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Loyufei.Test
{
    public class ObjectExtensionsTests
    {
        [Test]
        public void ObjectExtensionsTestsSimplePasses()
        {
            object obj = default(object);

            Assert.AreEqual(true, obj.IsDefault());
            Assert.AreEqual(9999, obj.IsDefaultandReturn(9999));

            object obj1 = 125;
            object obj2 = 225;

            Assert.AreEqual(false, obj1.IsEqual(obj2));

            object obj3 = "Test Unit";

            Assert.IsNotNull(obj3.To<string>());
            Assert.AreEqual(true, obj3.TryTo(out string str));

            Assert.AreEqual('T', obj3.Convert(o => o.To<string>()[0]));
        }
    }
}