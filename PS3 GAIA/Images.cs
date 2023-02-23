using System;
using System.Windows.Forms;

namespace QRC_Editor
{
    public partial class Images : Form
    {
        private int count = 20;
        private bool _preventMove = true;
        public Images()
        {
            Random rnd = new Random();
            //  int i = rnd.Next(1, 5);
            InitializeComponent();
            /* switch (i)
             {
                 case 1:
                     this.BackgroundImage = global::XForge.Properties.Resources.XIJ4GuV;
                     break;
                 case 2:
                     this.BackgroundImage = global::XForge.Properties.Resources.OhfFDWG;
                     break;
                 case 3:
                     this.BackgroundImage = global::XForge.Properties.Resources.M7RPlLM;
                     break;
                 case 4:
                     this.BackgroundImage = global::XForge.Properties.Resources.KR5u7Cg;
                     break;
                 case 5:
                     this.BackgroundImage = global::XForge.Properties.Resources.bLHtzBT;
                     break;
             }*/

        }
        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 274;
            const int SC_MOVE = 0xF010;

            if (_preventMove)
            {
                switch (message.Msg)
                {
                    case WM_SYSCOMMAND:
                        int command = message.WParam.ToInt32() & 0xfff0;
                        if (command == SC_MOVE)
                            return;
                        break;
                }
            }

            base.WndProc(ref message);
        }

        private void OUTFade_Tick(object sender, EventArgs e)
        {
            if (this.Opacity == 0)
            {
                OUTFade.Enabled = false;
                this.Close();
                return;
            }
            this.Opacity -= 0.01;
        }

        private void INFade_Tick(object sender, EventArgs e)
        {
            if (this.Opacity == 1)
            {
                INFade.Enabled = false;
                timer1.Enabled = true;
                return;
            }
            this.Opacity += 0.01;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count == 0)
            {
                timer1.Enabled = false;
                OUTFade.Enabled = true;
                return;
            }
            count -= 1;
        }
    }
}
