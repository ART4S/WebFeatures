﻿using WebFeatures.Domian.Attibutes;
using WebFeatures.Domian.Common;

namespace WebFeatures.Domian.Entities
{
    [EntityMetadata("Countries")]
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public string Continent { get; set; }
    }
}