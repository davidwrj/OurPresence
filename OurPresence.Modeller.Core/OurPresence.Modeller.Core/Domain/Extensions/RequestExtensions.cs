// Copyright (c)  Allan Nielsen.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace OurPresence.Modeller.Domain.Extensions
{
    public static class RequestExtensions
    {
        public static Request AddField(this Request request, string name, DataTypes dataTypes = DataTypes.String, bool nullable = false)
        {
            request.Fields.Add(new Field(name, dataTypes, nullable));
            return request;
        }

        public static Response AddResponse(this Request request)
        {
            request.Response = new Response();
            return request.Response;
        }

        public static Response AddField(this Response response, string name, DataTypes dataTypes = DataTypes.String, bool nullable = false)
        {
            response.Fields.Add(new Field(name, dataTypes, nullable));
            return response;
        }
    }
}
