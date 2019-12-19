﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rallyeLecture___ajout_eleves {
    public partial class form_add : Form {
        form_origin motherWindow;
        public form_add(form_origin F1) {
            InitializeComponent();
            motherWindow = F1;
            MySqlConnection cnx;
            string sCnx;
            sCnx = String.Format("server=172.16.0.148;uid=AdminRl;database=rallyeLecture;port=3306;pwd=siojjr");
            cnx = new MySqlConnection(sCnx);
            cnx.Open();
            string requete = "select id, niveauScolaire from niveau";
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cnx;
            cmd.CommandText = requete;
            cmd.CommandType = System.Data.CommandType.Text;
            MySqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            while (rdr.Read()) {
                cb_sclrlvl.Items.Add(rdr.GetString(1));
                cb_sclrlvl.ItemHeight = rdr.GetInt32(0);
            }
        }

        private void btn_intlaunch_Click(object sender, EventArgs e) {
            string aff = "";
            for (int i = 0; i < clb_csvf.CheckedItems.Count; i++) {
                string selectedCsvPath = tb_csvf.Text + "\\" +clb_csvf.CheckedItems[i];
                PassWordType pst;
                if ((rb_rand.Checked)||(rb_build.Checked)) {
                    if (rb_rand.Checked) {
                        pst = PassWordType.aleatoire;
                    }
                    else {
                        pst = PassWordType.construit;
                    }
                    List<Eleve> leselv = LesEleves.LoadCsv(pst, selectedCsvPath);
                    foreach (Eleve elv in leselv) {
                        aff += String.Format("{0} {1} {2} {3}\r\n",elv.Login,elv.Nom,elv.Prenom,elv.PassWord);
                    }
                }
            }
            lbl_eleveList.Text = aff;
        }

        private void btn_brwsecsv_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (fbd.ShowDialog() == DialogResult.OK) {
                tb_csvf.Text = fbd.SelectedPath;
                string[] allfiles = Directory.GetFiles(fbd.SelectedPath);
                for (int i = 0; i < allfiles.Length; i++) {
                    if (allfiles[i].Contains(".csv")) {
                        string[] decomposedPath = allfiles[i].Split('\\');
                        clb_csvf.Items.Add(decomposedPath[decomposedPath.Length - 1]);
                    }
                }
            }
        }

        private void form_add_FormClosed(object sender, FormClosedEventArgs e) {
            motherWindow.Close();
        }
    }
}