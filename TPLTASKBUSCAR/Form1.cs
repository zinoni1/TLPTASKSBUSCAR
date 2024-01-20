using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPLTASKBUSCAR
{
    public partial class Form1 : Form
    {
        List<string> resultats = new List<string>();
        string hola = "";

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!chkCountry.Checked && !chkEmail.Checked && !chkFirst.Checked && !chkid.Checked && !chkIP.Checked && !chkLast.Checked)
            {
                txtResult.Text = "ERROR: Selecciona un campo mínimo";
                return;
            }

            resultats.Clear();
            hola = "";

            List<Task<List<string>>> tasks = new List<Task<List<string>>>();

            if (chkCountry.Checked)
                tasks.Add(Task.Run(() => trolo.buscar(txtbuscar.Text, 4)));
            if (chkEmail.Checked)
                tasks.Add(Task.Run(() => trolo.buscar(txtbuscar.Text, 3)));
            if (chkFirst.Checked)
                tasks.Add(Task.Run(() => trolo.buscar(txtbuscar.Text, 1)));
            if (chkid.Checked)
                tasks.Add(Task.Run(() => trolo.buscar(txtbuscar.Text, 0)));
            if (chkIP.Checked)
                tasks.Add(Task.Run(() => trolo.buscar(txtbuscar.Text, 5)));
            if (chkLast.Checked)
                tasks.Add(Task.Run(() => trolo.buscar(txtbuscar.Text, 2)));

            await Task.WhenAll(tasks);

            foreach (var completedTask in tasks)
            {
                List<string> results = completedTask.Result
                    .Where(result => !string.IsNullOrEmpty(result))
                    .ToList();

                if (results.Any())
                {
                  
                    resultats.AddRange(results);
                    hola = string.Join(Environment.NewLine, resultats);
                    txtResult.Text = hola;
                }
            }

            Task<List<string>> eduTask = Task.Run(() => AnalitzarAdreces(resultats, ".edu" ));
            Task<List<string>> infoTask = Task.Run(() => AnalitzarAdreces(resultats, ".info"));

            await Task.WhenAll(eduTask, infoTask);

            txtResult.Text += Environment.NewLine;
            txtResult.Text += Environment.NewLine + "Resultats .edu:";
            txtResult.Text += Environment.NewLine;
            txtResult.Text += Environment.NewLine + string.Join(Environment.NewLine, eduTask.Result);
            txtResult.Text += Environment.NewLine;
            txtResult.Text += Environment.NewLine + "Resultats .info:";
            txtResult.Text += Environment.NewLine;
            txtResult.Text += Environment.NewLine + string.Join(Environment.NewLine, infoTask.Result);

            if (string.IsNullOrEmpty(hola))
            {
                txtResult.Text = "No Encontrado!";
            }
        }

        private List<string> AnalitzarAdreces(List<string> lines, string domain)
        {
            return lines
                .Where(line => line.ToLower().Contains(domain.ToLower()))
                .ToList();
        }
    }
}
