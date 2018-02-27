using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaldaGameWFA
{
    public partial class LevelForm : Form
    {
        public int PCLevel { get; set; }
        // Property for changing length of words choosing by PC.
        public LevelForm()
        {
            InitializeComponent();
        }

        private void easyLevelRadioButton_Click(object sender, EventArgs e)
        {
            PCLevel = 1;
        }

        private void middleLevelRadioButton_Click(object sender, EventArgs e)
        {
            PCLevel = 2;
        }

        private void hardLevelRadioButton_Click(object sender, EventArgs e)
        {
            PCLevel = 3;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
