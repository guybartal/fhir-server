﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Linq;
using System.Reflection;
using Microsoft.Health.Fhir.SqlServer.Features.Schema.Model;
using Microsoft.Health.Fhir.SqlServer.Features.Storage;
using Microsoft.Health.SqlServer.Features.Schema.Model;
using Xunit;

namespace Microsoft.Health.Fhir.SqlServer.UnitTests.Features.Import
{
    public class SqlServerBulkImportOperationTests
    {
        [Fact(Skip = "only disable indexes necessary")]
        public void GivenResourceRelatedTables_WhenNewIndexesAdded_BulkImportOperationShouldSupportNewIndexes()
        {
            Table[] resourceRelatedTables = new Table[]
            {
                VLatest.Resource,
                VLatest.ResourceWriteClaim,
                VLatest.CompartmentAssignment,
                VLatest.DateTimeSearchParam,
                VLatest.NumberSearchParam,
                VLatest.QuantitySearchParam,
                VLatest.ReferenceSearchParam,
                VLatest.ReferenceTokenCompositeSearchParam,
                VLatest.StringSearchParam,
                VLatest.TokenDateTimeCompositeSearchParam,
                VLatest.TokenNumberNumberCompositeSearchParam,
                VLatest.TokenQuantityCompositeSearchParam,
                VLatest.TokenSearchParam,
                VLatest.TokenStringCompositeSearchParam,
                VLatest.TokenText,
                VLatest.TokenTokenCompositeSearchParam,
                VLatest.UriSearchParam,
            };

            string[] supportedIndexesNames = SqlServerFhirDataBulkImportOperation.UnclusteredIndexes.Select(i => i.index.IndexName).ToArray();
            int expectedIndexesCount = 0;
            foreach (Table table in resourceRelatedTables)
            {
                string[] indexNames = table.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Where(f => f.Name.StartsWith("IX_")).Select(f => f.Name).ToArray();
                foreach (string indexName in indexNames)
                {
                    Assert.Contains(indexName, supportedIndexesNames);
                    expectedIndexesCount++;
                }
            }

            Assert.Equal(expectedIndexesCount, supportedIndexesNames.Length);
        }
    }
}