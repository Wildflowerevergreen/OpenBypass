using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using ActivationServer;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenBypass
{
    public partial class Activation : Form
    {
        public Activation()
        {
            InitializeComponent();
            ActivationServer thread = new ActivationServer();
            thread.StartServer();
        }
    }
}
