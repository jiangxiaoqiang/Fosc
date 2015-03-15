using System;
using log4net;
using System.Threading;

namespace Fosc.Dolphin.UI
{
	
	public delegate int TakeAWhileDelegate(int data,int ms);
			
	public class MutiTaskSync
	{
		#region Attribute
		private static log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MutiTaskSync));		
		#endregion		
		
		public MutiTaskSync ()
		{
			
		}
		
		
		#region Function
		
		static int TakesAWhile(int data,int ms)
		{
			logger.Info("a");
			Thread.Sleep(ms);
			logger.Info ("b");
			return ++data;
		}
		
		#endregion 
	}
}

