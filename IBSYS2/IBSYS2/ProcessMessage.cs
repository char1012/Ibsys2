﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBSYS2
{
    public partial class ProcessMessage : Form
    {
        private String sprache = "de";

        public ProcessMessage(String sprache)
        {
            InitializeComponent();
            if (sprache != "en")
            {
                label1.Text = "Einen Moment bitte ...";
            }
            else
            {
                label1.Text = "One moment please ...";

            }
            this.sprache = sprache;
            // TODO Sprachen();
        }
    }
}
