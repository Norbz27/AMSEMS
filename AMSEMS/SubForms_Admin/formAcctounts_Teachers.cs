﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAccounts_Teachers : Form
    {
        public formAccounts_Teachers(String accountName, int role)
        {
            InitializeComponent();
            lblAccountName.Text = accountName;
        }
    }
}