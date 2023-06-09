﻿using System;
using System.Threading;
using System.Threading.Tasks;
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
            Thread thread = new Thread(DoWork);
            thread.Start();
            MessageBox.Show("完了"); // 終了を待たずに表示される
        }

        private void ThreadPoolBtn(object sender, EventArgs e)
        {
            // スレッドプールにタスクを投げる DoWorkに引数を渡さなくても無視するのでエラーにならない
            ThreadPool.QueueUserWorkItem(DoWork2);
            MessageBox.Show("完了"); // 終了を待たずに表示される
        }

        /// <summary>
        /// Taskボタン
        /// ContinueWith() メソッドで登録したコールバックメソッドが実行されるときには、
        /// TaskScheduler.FromCurrentSynchronizationContext() メソッドで取得した同期コンテキスト上で実行されるため、
        /// UI スレッドで安全にメッセージボックスを表示することができます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskBtn(object sender, EventArgs e)
        {
            // 現在のスレッドの同期コンテキストを取得
            var context = TaskScheduler.FromCurrentSynchronizationContext();
            // ContinueWith() メソッドを使用して、タスクが完了したときに実行するコールバックメソッド(今回はif文)を登録
            Task.Run(() => DoWork3()).ContinueWith(x => 
            {
                if (x.Result)
                {
                    MessageBox.Show("完了");
                }
                // コールバックメソッドを 同期コンテキスト(context)上で実行するように指定
            }, context);
        }
        
        /// <summary>
        /// async/awaitボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void async_awaitBtn(object sender, EventArgs e)
        {
            await Task.Run(() => DoWork());
            MessageBox.Show("完了");
        }


        private CancellationTokenSource  _token;
        /// <summary>
        /// キャンセルトークンボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private async void asyncCalncelTokenBtn(object sender, EventArgs e)
        {
            try
            {
                // トークンインスタンスを作成
                _token = new CancellationTokenSource();
                // 2つ目の_token.Tokenはすでにキャンセルトークンが投げられていた場合、Taskが動かずにすぐ止めることができる
                await Task.Run(() => DoWork4(_token.Token), _token.Token);
                MessageBox.Show("完了");
            }
            catch (OperationCanceledException cancel)
            {
                MessageBox.Show("キャンセルされました。");
            }
            finally
            {
                _token.Dispose();
                _token = null;
            }
        }





        /// <summary>
        /// キャンセルボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn(object sender, EventArgs e)
        {
            _token?.Cancel();
        }

        /// <summary>
        /// Thread用
        /// </summary>
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

        /// <summary>
        /// ThreadPool用
        /// </summary>
        /// <param name="o"></param>
        private void DoWork2(Object o)
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

        /// <summary>
        /// Task用
        /// </summary>
        private bool DoWork3()
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
            return true;
        }

        private bool DoWork4(CancellationToken token)
        {
            // スレッドで行う処理をここに書く
            for (int i = 0; i <= 10; i++)
            {
                token.ThrowIfCancellationRequested();
                // UIスレッドで実行する処理をInvokeメソッドを使って指定する
                this.Invoke((Action)(() =>
                {
                    // UIスレッドで実行したい処理をここに書く
                    label1.Text = i.ToString();
                }));

                Thread.Sleep(1000);
            }
            return true;
        }
    }
}
