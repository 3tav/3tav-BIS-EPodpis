using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mSignAgent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            var svc = new mSignAgentLib();

            svc.Init();

            svc.RefreshMsignStatus();

        }
    }
}
