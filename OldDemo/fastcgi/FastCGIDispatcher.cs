using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using PHP;

namespace FastCGI
{
    class FastCGIDispatcher
    {
        private Dictionary<int, PHPFastCGIProcess> processes;

        public FastCGIDispatcher()
        {
            processes = new Dictionary<int, PHPFastCGIProcess>();
        }

        public PHPFastCGIProcess Dispatch()
        {
            lock (processes)
            {
                foreach (var item in processes)
                {
                    if (item.Value.WorkCount < 2)
                    {
                        return item.Value;
                    }
                }
                int port = FindUsablePort();
                var process = new PHPFastCGIProcess(port);
                processes.Add(port, process);
                return process;
            }
        }

        /// <summary>
        /// 停止所有进程
        /// </summary>
        public void Stop()
        {
            lock (processes)
            {
                foreach (var items in processes)
                {
                    items.Value.Stop();
                }
            }
        }

        /// <summary>
        /// 找到可用的端口
        /// </summary>
        /// <returns></returns>
        public int FindUsablePort()
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = ipProperties.GetActiveTcpListeners();
            List<int> ports = tcpEndPoints.Select(p => p.Port).Where(p => p > 9000).ToList<int>();
            lock (processes) ports.AddRange(processes.Keys);
            ports.Sort();
            int result = 9001;
            int i = 0;
            while (i < ports.Count && result < 60000)
            {
                if (result != ports[i++]) return result;
                ++result;
            }
            return 0;
        }
    }
}