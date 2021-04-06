﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using EnsureThat;
using MediatR;

namespace Microsoft.Health.Fhir.Core.Messages.BulkImport
{
    public class CancelBulkImportRequest : IRequest<CancelBulkImportResponse>
    {
        public CancelBulkImportRequest(string taskId)
        {
            EnsureArg.IsNotNullOrWhiteSpace(taskId, nameof(taskId));

            TaskId = taskId;
        }

        public string TaskId { get; }
    }
}
