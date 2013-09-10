using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.IO;

namespace GuruETC.Web.Models
{
    public class ChartActionResult : ActionResult
    {
        private readonly Chart _chart;
        private readonly ChartImageFormat _imageFormat;

        public ChartActionResult(Chart chart, ChartImageFormat imageFormat = ChartImageFormat.Png)
        {
            if (chart == null) { throw new ArgumentNullException("chart"); }

            _chart = chart;
            _imageFormat = imageFormat;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.Clear();
            response.Charset = String.Empty;
            response.ContentType = "image/" + _imageFormat;

            if (_imageFormat == ChartImageFormat.Png)
            {
                // PNG can only write to a seek-able stream
                //  Thus we have to go through a memory stream, which permits seeking.
                using (var mStream = new MemoryStream())
                {
                    _chart.SaveImage(mStream, _imageFormat);
                    mStream.Seek(0, SeekOrigin.Begin);
                    mStream.CopyTo(response.OutputStream);
                }
            }
            else
            { // If we don't have to provide a seek-able stream, write directly to
                //  where the data needs to go.
                _chart.SaveImage(response.OutputStream, _imageFormat);
            }

            _chart.Dispose();
        }
    }
}