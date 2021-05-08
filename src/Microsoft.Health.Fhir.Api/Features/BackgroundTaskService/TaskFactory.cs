﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Health.Core.Features.Context;
using Microsoft.Health.Fhir.Core.Features.Context;
using Microsoft.Health.Fhir.Core.Features.Operations;
using Microsoft.Health.Fhir.Core.Features.Operations.Import;
using Microsoft.Health.TaskManagement;
using Newtonsoft.Json;

namespace Microsoft.Health.Fhir.Api.Features.BackgroundTaskService
{
    public class TaskFactory : ITaskFactory
    {
        private IFhirDataBulkImportOperation _fhirDataBulkImportOperation;
        private IImportResourceLoader _importResourceLoader;
        private IResourceBulkImporter _resourceBulkImporter;
        private IImportErrorStoreFactory _importErrorStoreFactory;
        private ISequenceIdGenerator<long> _sequenceIdGenerator;
        private IIntegrationDataStoreClient _integrationDataStoreClient;
        private ITaskManager _taskmanager;
        private IContextUpdaterFactory _contextUpdaterFactory;
        private RequestContextAccessor<IFhirRequestContext> _contextAccessor;
        private ILoggerFactory _loggerFactory;

        public TaskFactory(
            IFhirDataBulkImportOperation fhirDataBulkImportOperation,
            IImportResourceLoader importResourceLoader,
            IResourceBulkImporter resourceBulkImporter,
            IImportErrorStoreFactory importErrorStoreFactory,
            IContextUpdaterFactory contextUpdaterFactory,
            ITaskManager taskmanager,
            ISequenceIdGenerator<long> sequenceIdGenerator,
            IIntegrationDataStoreClient integrationDataStoreClient,
            RequestContextAccessor<IFhirRequestContext> contextAccessor,
            ILoggerFactory loggerFactory)
        {
            _fhirDataBulkImportOperation = fhirDataBulkImportOperation;
            _importResourceLoader = importResourceLoader;
            _resourceBulkImporter = resourceBulkImporter;
            _importErrorStoreFactory = importErrorStoreFactory;
            _sequenceIdGenerator = sequenceIdGenerator;
            _integrationDataStoreClient = integrationDataStoreClient;
            _taskmanager = taskmanager;
            _contextUpdaterFactory = contextUpdaterFactory;
            _contextAccessor = contextAccessor;
            _loggerFactory = loggerFactory;
        }

        public ITask Create(TaskInfo taskInfo)
        {
            if (taskInfo.TaskTypeId == ImportProcessingTask.ImportProcessingTaskId)
            {
                IContextUpdater contextUpdater = _contextUpdaterFactory.CreateContextUpdater(taskInfo.TaskId, taskInfo.RunId);
                ImportProcessingTaskInputData inputData = JsonConvert.DeserializeObject<ImportProcessingTaskInputData>(taskInfo.InputData);
                ImportProcessingProgress importProgress = string.IsNullOrEmpty(taskInfo.Context) ? new ImportProcessingProgress() : JsonConvert.DeserializeObject<ImportProcessingProgress>(taskInfo.Context);
                return new ImportProcessingTask(
                    inputData,
                    importProgress,
                    _fhirDataBulkImportOperation,
                    _importResourceLoader,
                    _resourceBulkImporter,
                    _importErrorStoreFactory,
                    contextUpdater,
                    _contextAccessor,
                    _loggerFactory);
            }

            if (taskInfo.TaskTypeId == ImportOrchestratorTask.ImportOrchestratorTaskId)
            {
                IContextUpdater contextUpdater = _contextUpdaterFactory.CreateContextUpdater(taskInfo.TaskId, taskInfo.RunId);
                ImportOrchestratorTaskInputData inputData = JsonConvert.DeserializeObject<ImportOrchestratorTaskInputData>(taskInfo.InputData);
                ImportOrchestratorTaskContext orchestratorTaskProgress = string.IsNullOrEmpty(taskInfo.Context) ? new ImportOrchestratorTaskContext() : JsonConvert.DeserializeObject<ImportOrchestratorTaskContext>(taskInfo.Context);

                return new ImportOrchestratorTask(
                    inputData,
                    orchestratorTaskProgress,
                    _taskmanager,
                    _sequenceIdGenerator,
                    contextUpdater,
                    _contextAccessor,
                    _fhirDataBulkImportOperation,
                    _integrationDataStoreClient,
                    _loggerFactory);
            }

            return null;
        }
    }
}