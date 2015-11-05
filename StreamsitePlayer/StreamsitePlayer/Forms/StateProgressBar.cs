using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SeriesPlayer.Forms
{
    public class StateProgressBar : ProgressBar
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        public enum State
        {
            NORMAL = 1, ERROR = 2, WARNING = 3
        }

        private State currentState = State.NORMAL;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public State CurrentState
        {
            get
            {
                return currentState;
            }

            set
            {
                SetState(value);
            }
        }

        private void SetState(State state)
        {
            if (CurrentState != state)
            {
                StateProgressBar.SendMessage(base.Handle, 1040, (IntPtr)((int)state), IntPtr.Zero);
                currentState = state;
            }
        }
    }
}
