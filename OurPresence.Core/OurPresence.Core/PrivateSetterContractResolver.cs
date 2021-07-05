// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace OurPresence.Core
{
    public class PrivateSetterContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            if (jsonProperty.Writable)
                return jsonProperty;

            if (member is PropertyInfo propertyInfo)
            {
                var setter = propertyInfo.GetSetMethod(true);
                jsonProperty.Writable = setter != null;
            }

            return jsonProperty;
        }
    }
}
