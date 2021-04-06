﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Net;

namespace Microsoft.Health.Fhir.Core.Messages.BulkImport
{
    public class GetBulkImportResponse
    {
        public GetBulkImportResponse(HttpStatusCode statusCode)
            : this(statusCode, taskResult: null)
        {
        }

        public GetBulkImportResponse(HttpStatusCode statusCode, string taskResult)
        {
            StatusCode = statusCode;
            TaskResult = taskResult;
        }

        public HttpStatusCode StatusCode { get; }

        public string TaskResult { get; }
    }
}
