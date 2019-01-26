using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace GnuPlot
{
    /// <summary>
    /// 配列の長さが一致しない場合に送出される例外
    /// </summary>
    public class NoMatchArrayLengthException : Exception { }

    public class GnuPlot
    {
        public string DatPath { get; private set; }
        public string ScriptPath { get; private set; }

        private List<string> fetchScripts = new List<string>();

        public GnuPlot(string datPath, string scriptPath)
        {
            DatPath = datPath;
            ScriptPath = scriptPath;
        }

        /// <summary>
        /// 1行のスクリプトを書き込む
        /// </summary>
        /// <param name="fetchingScript"></param>
        public void FetchScript(string fetchingScript)
        {
            fetchScripts.Add(fetchingScript);
        }

        /// <summary>
        /// DATファイルにデータを書き込む
        /// </summary>
        /// <param name="X">X軸のデータ</param>
        /// <param name="Y">Y軸のデータ</param>
        public void FetchData(double[] X, double[] Y)
        {
            if (X.Length != Y.Length)
                throw new NoMatchArrayLengthException();

            using (var dat = new FileStream(DatPath, FileMode.Create))
            {
                using (var stream = new StreamWriter(dat))
                {
                    for (int i = 0; i < X.Length; ++i)
                    {
                        stream.WriteLine($"{X[i]}   {Y[i]}");
                    }
                }
            }
        }

        private void WriteScript()
        {
            using (var script = new FileStream(ScriptPath, FileMode.Create))
            {
                using (var stream = new StreamWriter(script))
                {
                    foreach (var fetch in fetchScripts)
                    {
                        stream.WriteLine(fetch);
                    }
                    stream.WriteLine("pause -1");
                }
            }
        }

        public void Execute()
        {
            WriteScript();

            Process.Start($"wgnuplot \"{ScriptPath}\"");
        }
    }
}
