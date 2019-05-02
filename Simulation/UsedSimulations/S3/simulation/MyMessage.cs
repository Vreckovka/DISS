using OSPABA;
using Simulations.UsedSimulations.S3.entities;

namespace simulation
{
	public class MyMessage : MessageForm
	{
        public ZastavkaData ZastavkaData { get; set; }
        public Autobus Autobus { get; set; }
		public MyMessage(OSPABA.Simulation sim) :
			base(sim)
		{
		}

		public MyMessage(MyMessage original) :
			base(original)
		{
			// copy() is called in superclass
		}

		override public MessageForm CreateCopy()
		{
			return new MyMessage(this);
		}

		override protected void Copy(MessageForm message)
		{
			base.Copy(message);
			MyMessage original = (MyMessage)message;
			// Copy attributes
		}
	}
}
