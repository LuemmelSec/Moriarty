﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Moriarty.Msrc
{
    public class MS13_053 : IVulnerabilityCheck
    {
        private const string Id = "MS13-053";
        private static readonly string[] Exploits = new[]
        {
            "https://www.exploit-db.com/exploits/33213/"
        };

        public Vulnerability GetVulnerability()
        {
            return new Vulnerability(Id, Exploits);
        }

        public void Check(VulnerabilityCollection vulnerabilities, int buildNumber, List<int> installedKBs)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                // 64-bit systems are not vulnerable
                return;
            }

            string systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            string filePath = Path.Combine(systemRoot, "system32", "win32k.sys");
            var versionInfo = FileVersionInfo.GetVersionInfo(filePath);

            int build = versionInfo.FileBuildPart;
            int revision = versionInfo.FilePrivatePart;

            // Implementing the vulnerability check logic
            if ((build == 7600 && revision >= 17000) ||
                (build == 7601 && revision <= 22348) ||
                (build == 9200 && revision <= 20732))
            {
                vulnerabilities.SetAsVulnerable(Id);
            }
        }
    }
}
