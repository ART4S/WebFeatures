﻿using System.Collections.Generic;
using WebFeatures.Domian.Common;

namespace WebFeatures.Infrastructure.Tests.TestObjects
{
    public class TestValueObject : ValueObject
    {
        public string StringProperty { get; set; }

        protected override IEnumerable<object> GetComparisionValues()
        {
            yield return StringProperty;
        }
    }
}
