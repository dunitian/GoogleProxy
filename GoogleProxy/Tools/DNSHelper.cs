using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace GoogleProxy.Tools
{
    public class DNSHelper
    {
        public static bool SetDNS(string[] dns)
        {
            if (dns == null)
            {
                return false;
            }

            ManagementClass wmi = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = wmi.GetInstances();
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;

            foreach (ManagementObject mo in moc)
            {

                if ((bool)mo["IPEnabled"])
                    continue;

                //设置dns
                inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                inPar["DNSServerSearchOrder"] = dns;
                outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
            }
            return true;
        }

        public static string SetDNS2()
        {
            //主dns
            string cmd = "netsh interface ip set dns \"本地连接\" static 180.76.76.76 primary";
            //开启一个进程
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            //启动的程序名
            p.StartInfo.FileName = "cmd.exe";
            // 关闭shell的使用
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            // 重定向标准输入
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            //不显示dos窗口
            p.StartInfo.CreateNoWindow = true;
            //启动程序
            p.Start();
            //写入命令
            p.StandardInput.WriteLine(cmd);
            //辅dns
            cmd = "netsh interface ip set dns \"本地连接\" static 114.114.114.114";
            //写入命令
            p.StandardInput.WriteLine(cmd);
            //退出
            p.StandardInput.WriteLine("exit");
            //执行结果返回
            string outputStr = p.StandardOutput.ReadToEnd();
            //关闭进程
            p.Close();
            return outputStr;
        }

        /// <summary>
        /// 恢复设置
        /// </summary>
        internal static string RecoveryDNS()
        {
            string cmd = "netsh interface ip set dns name=\"本地连接\" source=dhcp";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(cmd);
            p.StandardInput.WriteLine("exit");
            string outputStr = p.StandardOutput.ReadToEnd();
            p.Close();
            return outputStr;
        }
    }
}
