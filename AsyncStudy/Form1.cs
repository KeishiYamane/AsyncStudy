using System;
using System.Threading;
using System.Windows.Forms;

namespace AsyncStudy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ThreadBtn(object sender, EventArgs e)
        {
            // スレッドを作成し、実行する
            Thread thread = new Thread(new ThreadStart(DoWork));
            thread.Start();
        }
        private void DoWork()
        {
            // スレッドで行う処理をここに書く
            for (int i = 0; i <= 10; i++)
            {
                // UIスレッドで実行する処理をInvokeメソッドを使って指定する
                this.Invoke((Action)(() =>
                {
                    // UIスレッドで実行したい処理をここに書く
                    label1.Text = i.ToString();
                }));

                Thread.Sleep(1000);
            }
        }
    }
}
