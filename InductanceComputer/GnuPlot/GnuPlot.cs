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
        /// ファイルを書き込むための関数
        /// </summary>
        /// <param name="path">pathに応じたStreamWriterを開く</param>
        /// <param name="func">匿名関数を投げる</param>
        private void WriteProcedure(string path, Action<StreamWriter> func)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    func(writer);
                }
            }
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

            WriteProcedure(DatPath, (stream) =>
            {
                for (int i = 0; i < X.Length; ++i)
                {
                    stream.WriteLine($"{X[i]}   {Y[i]}");
                }
            });
        }

        /// <summary>
        /// X軸の値が同一の複数のデータをプロットする
        /// </summary>
        /// <param name="X">種になるX軸のデータ</param>
        /// <param name="Ys">Y軸の複数データ</param>
        public void WriteData(double[] X, double[][] Ys)
        {
            if (X.Length != Ys.Length)
                throw new NoMatchArrayLengthException();

            WriteProcedure(DatPath, (stream) =>
            {
                int prevLength = 0;
                if (Ys.Length > 0)
                    prevLength = Ys[0].Length;

                for (int i = 0; i < X.Length; ++i)
                {
                    if (prevLength != Ys[i].Length)
                        throw new NoMatchArrayLengthException();
                    prevLength = Ys[i].Length;

                    var line = $"{X[i]}\t" + string.Join("\t", Ys[i]);
                    stream.WriteLine(line);
                }
            });
        }

        /// <summary>
        /// プロット文を差し込む
        /// </summary>
        /// <param name="arguments"></param>
        public void SetPlot(string arguments)
        {
            WriteProcedure(ScriptPath, (stream) =>
            {
                
            });
        }

        private void WriteScript(string arguments)
        {
            WriteProcedure(ScriptPath, (stream) =>
            {
                stream.WriteLine($"plot \"{DatPath}\" {arguments}");
                foreach (var fetch in fetchScripts)
                {
                    stream.WriteLine(fetch);
                }
                stream.WriteLine("pause -1");
            });
        }

        /// <summary>
        /// wgnuplotコマンドを実行する
        /// </summary>
        public void Execute(string plotArguments = "")
        {
            WriteScript(plotArguments);

            var command = $"wgnuplot \"{Directory.GetCurrentDirectory()}\\{ScriptPath}\"";
            Process.Start(command);
        }
    }
}
