﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Moriarty.Msrc
{
    public class MS14_058 : IVulnerabilityCheck
    {
        private const string Id = "MS14-058";
        private static readonly string[] Exploits = new[]
        {
            "https://www.exploit-db.com/exploits/35101/"
        };

        public Vulnerability GetVulnerability()
        {
            return new Vulnerability(Id, Exploits);
        }

        public void Check(VulnerabilityCollection vulnerabilities, int buildNumber, List<int> installedKBs)
        {
            string systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            string filePath = Path.Combine(systemRoot, "system32", "win32k.sys");

            var versionInfo = FileVersionInfo.GetVersionInfo(filePath);

            int build = versionInfo.FileBuildPart;
            int revision = versionInfo.FilePrivatePart;

            // Implementing the vulnerability check logic
            if ((build == 7600 && revision >= 18000) ||
                (build == 7601 && revision <= 22823) ||
                (build == 9200 && revision <= 21247) ||
                (build == 9600 && revision <= 17353))
            {
                vulnerabilities.SetAsVulnerable(Id);
            }
        }
    }
}
