﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Moriarty.Msrc
{
    public class MS10_015 : IVulnerabilityCheck
    {
        private const string Id = "MS10-015";
        private static readonly string[] Exploits = new[]
        {
            "https://www.exploit-db.com/exploits/11199/"
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
            string filePath = Path.Combine(systemRoot, "system32", "ntoskrnl.exe");
            var versionInfo = FileVersionInfo.GetVersionInfo(filePath);

            int build = versionInfo.FileBuildPart;
            int revision = versionInfo.FilePrivatePart;

            if (build == 7600 && revision <= 20591)
            {
                vulnerabilities.SetAsVulnerable(Id);
            }
        }
    }
}
