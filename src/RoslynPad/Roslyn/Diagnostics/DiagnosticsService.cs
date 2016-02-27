﻿using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Reflection;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace RoslynPad.Roslyn.Diagnostics
{
    public sealed class DiagnosticsService : IDiagnosticService
    {
        private static readonly Type InterfaceType = Type.GetType("Microsoft.CodeAnalysis.Diagnostics.IDiagnosticService, Microsoft.CodeAnalysis.Features", throwOnError: true);

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly object _inner;

        public static IDiagnosticService Load(CompositionHost host)
        {
            var service = host.GetExport(InterfaceType);
            return new DiagnosticsService(service);
        }

        private DiagnosticsService(object inner)
        {
            _inner = inner;
            var eventInfo = InterfaceType.GetEvent(nameof(DiagnosticsUpdated));
            eventInfo.AddEventHandler(_inner,
                Delegate.CreateDelegate(eventInfo.EventHandlerType, this,
                    typeof(DiagnosticsService).GetMethod(nameof(OnDiagnosticsUpdated),
                        BindingFlags.NonPublic | BindingFlags.Instance)));
        }

        // ReSharper disable once UnusedParameter.Local
        private void OnDiagnosticsUpdated(object sender, EventArgs e)
        {
            DiagnosticsUpdated?.Invoke(this, new DiagnosticsUpdatedArgs(e));
        }

        public event EventHandler<DiagnosticsUpdatedArgs> DiagnosticsUpdated;

        public IEnumerable<DiagnosticData> GetDiagnostics(Workspace workspace, ProjectId projectId, DocumentId documentId, object id,
            bool includeSuppressedDiagnostics, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UpdatedEventArgs> GetDiagnosticsUpdatedEventArgs(Workspace workspace, ProjectId projectId, DocumentId documentId,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}