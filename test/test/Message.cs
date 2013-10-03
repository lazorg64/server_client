using System;

namespace test
{
	public class Message
	{

		private String from;
		private String to;
		private String message;
		private bool check;

		public Message (String From,String To,String Message){
			from=From;
			to=To;
			message=Message;
			check=false;
		}
        public String getto()
        {
            return to;
        }
        public String getmessage()
        {
            return message;
        }
        public String getfrom()
        {
            return from;
        }
	}
}

