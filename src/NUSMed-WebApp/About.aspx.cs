using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Caching;
using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System.IO;

namespace NUSMed_WebApp
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAbout();

            List<string> time = new List<string>();
            List<string> ax = new List<string>();
            List<string> ay = new List<string>();
            List<string> az = new List<string>();
            List<string> gx = new List<string>();
            List<string> gy = new List<string>();
            List<string> gz = new List<string>();

            string line = string.Empty;
            using (StringReader reader = new StringReader(File.ReadAllText("C:\\Users\\trueh\\Downloads\\RawData_2015.12.16_09.55.28.txt")))
            {
                 line = string.Empty;

                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        string[] tokens = line.Split(',');
                        time.Add(tokens[0]);
                        ax.Add(tokens[1]);
                        ay.Add(tokens[2]);
                        az.Add(tokens[3]);
                        gx.Add(tokens[4]);
                        gy.Add(tokens[5]);
                        gz.Add(tokens[6]);
                    }

                } while (line != null);
            }

            string seconds = string.Join(", ", time.ToArray());

            ScriptManager.RegisterStartupScript(this, GetType(), "Update Timeseries", @"
                var layout = {
                    title: 'Gait Timeseries',
                yaxis: {
                    title: 'Magnitude',
                    autotick: true,
                    ticks: 'outside',
                  },
                  xaxis: {
                    title: 'Time (seconds)',
                    autotick: true,
                    ticks: 'outside',
                  },
                };

                var trace1 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'ax',
                        y: [" + string.Join(", ", ax.ToArray()) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#ed0212' }
                }

                var trace2 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'ay',
                        y: [" + string.Join(", ", ay.ToArray()) + @"],
                        x: [" +  seconds + @"],
                        line: { color: '#17BECF' }
                }

                var trace3 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'az',
                        y: [" + string.Join(", ", az.ToArray()) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#fcba03' }
                }

                var trace4 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'gx',
                        y: [" + string.Join(", ", gx.ToArray()) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#f014f7' }
                }

                var trace5 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'gy',
                        y: [" + string.Join(", ", gy.ToArray()) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#14f732' }
                }

                var trace6 = {
                        type: 'scatter',
                        mode: 'lines',
                        name: 'gz',
                        y: [" + string.Join(", ", gz.ToArray()) + @"],
                        x: [" + seconds + @"],
                        line: { color: '#f7ab14' }
                }

                Plotly.newPlot('modalFileViewPanelText', [trace1, trace2, trace3, trace4, trace5, trace6], layout);
            ", true);

        }

    }
}
