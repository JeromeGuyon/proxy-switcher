using System;
using System.Threading;
using System.Windows.Forms;

namespace ProxySwitcher
{
	static class Program
	{
		/// <summary>
		/// Point d'entrée principal de l'application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			using (Mutex mutex = new Mutex(false, "Global\\79c283a9-49fd-4217-8595-7403cb2884fd"))
			{
				if (!mutex.WaitOne(0, false))
				{
					MessageBox.Show("Instance already running");
					return;
				}

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new Form1());
			}
		}
	}
}
