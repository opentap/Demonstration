using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenTap.Metrics;

namespace OpenTap.Demonstration
{

    public class ProcessMetrics : IMetricSource, IOnPollMetricsCallback
    {
        public Stopwatch runnerTimer = Stopwatch.StartNew();
        public Dictionary<int, long> prevCpuByPid = new Dictionary<int, long>();
        public Dictionary<int, long> prevCpuTotalByPid = new Dictionary<int, long>();

        [Metric("Process Memory", null, MetricKind.Poll, DefaultPollRate = 5, DefaultEnabled = true)]
        public double Memory { get; set; }
        [Metric("Process CPU", null, MetricKind.Poll, DefaultPollRate = 5, DefaultEnabled = true)]
        public double CPU { get; set; }

        public ProcessMetrics()
        {
            // Initialize the process metrics
        }

        public void OnPollMetrics(IEnumerable<MetricInfo> metrics)
        {
            Memory = GetMemoryUsageForProcess(Process.GetCurrentProcess().Id);
            CPU = GetCPUUsageForProcess(Process.GetCurrentProcess().Id);
        }

        private float GetCPUUsageForProcess(int processId)
        {
            if (processId == 0)
                return 0;
            var proc = Process.GetProcessById(processId);
            long runnerCpu = proc.TotalProcessorTime.Ticks;
            long runnerCpuTotal = runnerTimer.Elapsed.Ticks;
            if (!prevCpuByPid.ContainsKey(processId))
            {
                prevCpuByPid[processId] = 0;
                prevCpuTotalByPid[processId] = 0;
            }

            var result = (float)(100.0 * (runnerCpu - prevCpuByPid[processId]) / (runnerCpuTotal - prevCpuTotalByPid[processId])) / Environment.ProcessorCount;
            prevCpuByPid[processId] = runnerCpu;
            prevCpuTotalByPid[processId] = runnerCpuTotal;
            return result;
        }

        private long GetMemoryUsageForProcess(int processId)
        {
            if (processId == 0)
                return 0;
            try
            {
                var proc = Process.GetProcessById(processId);
                return proc.WorkingSet64;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("'Memory usage' for a process metric failed", ex);
            }
        }

    }

}
