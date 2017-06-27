using System;

namespace Daily_Routine_Game
{

    public class BoardSqu
    {
        public bool IsEvent;
        public int EventAction;
        public string PosActText;
        public string NegActText;

        public BoardSqu()
        {
        }

        public BoardSqu(bool isEvent, int eventAction, string posActText, string negActText)
        {
            IsEvent = isEvent;
            EventAction = eventAction;
            PosActText = posActText;
            NegActText = negActText;
        }
    }


}