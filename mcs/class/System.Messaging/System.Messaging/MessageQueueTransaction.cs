//
// System.Messaging
//
// Authors:
//      Peter Van Isacker (sclytrack@planetinternet.be)
//      Rafael Teixeira   (rafaelteixeirabr@hotmail.com)
//
// (C) 2003 Peter Van Isacker
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;

using Mono.Messaging;

namespace System.Messaging 
{

	// TODO: have to comply with 'This type is safe for multithreaded operations'
	public class MessageQueueTransaction : IDisposable 
	{
		private readonly IMessageQueueTransaction delegateTx;
		private readonly object syncObj = new object ();
		private bool isDisposed = false;

		public MessageQueueTransaction () : this (GetMessageQueueTransaction ())
		{
		}
		
		internal MessageQueueTransaction (IMessageQueueTransaction delegateTx)
		{
			this.delegateTx = delegateTx;
		}
		
		public MessageQueueTransactionStatus Status 
		{
			get { 
				return (MessageQueueTransactionStatus) delegateTx.Status;
			}
		}
		
		internal IMessageQueueTransaction DelegateTx {
			get { return delegateTx; }
		}
		
		private static IMessageQueueTransaction GetMessageQueueTransaction ()
		{
			return MessagingProviderLocator.GetProvider ().CreateMessageQueueTransaction ();
		}
			
		public void Abort ()
		{
			delegateTx.Abort ();
		}
		
		public void Begin ()
		{
			delegateTx.Begin ();
		}
		
		public void Commit ()
		{
			delegateTx.Commit ();
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		
		protected virtual void Dispose (bool disposing)
		{
			lock (syncObj) {
				if (!isDisposed && disposing) {
					delegateTx.Dispose ();
				}
			}
		}
		
		~MessageQueueTransaction()
		{
			Dispose ();
		}
	}
}
