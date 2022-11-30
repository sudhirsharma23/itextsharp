using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using System.Drawing;

namespace WebApplication1
{
    public partial class PdfProfile : System.Web.UI.Page
    {
        public static string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null)
            {
                SqlConnection con = new SqlConnection(constr);
                Document doc = new Document(PageSize.A4, 7, 5, 5, 0);
                string str1 = "Select * from Employees where id=1";
                con.Open();
                SqlCommand cmd = new SqlCommand(str1, con);
                DataTable dt = new DataTable();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
                //DataView dv = dt.DefaultView;  
                string filePath = HttpContext.Current.Server.MapPath("~/Resume/" + Request.QueryString["Id"]);
                if (File.Exists(filePath + "/" + "UserProfile.pdf"))
                {
                    Response.Redirect("~/ProfileReport.aspx?ID=" + Request.QueryString["Id"]);
                }
                else
                {
                    try
                    {
                        //Create [Portfolio] directory If does not exist  
                        if (!Directory.Exists(filePath))

                        {
                            Directory.CreateDirectory(filePath);
                        }
                        string newFilePath = filePath + "\\" + "UserProfile.pdf"; //GetFileName(filePath, "UserProfile"); //  
                        PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(newFilePath, FileMode.Create));
                        doc.Open();
                        //Get the user photos to be show in pdf  
                        Int64 _userProfileID = Int64.Parse(Request.QueryString["Id"]);
                        DataTable dtPhotos = dt;
                        iTextSharp.text.Font mainFont = FontFactory.GetFont("Segoe UI", 22, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#999")));
                        iTextSharp.text.Font infoFont1 = FontFactory.GetFont("Kalinga", 10, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#666")));
                        iTextSharp.text.Font expHeadFond = FontFactory.GetFont("Calibri (Body)", 12, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#666")));
                        PdfContentByte contentByte = writer.DirectContent;
                        DataTable objDataTable = dt;
                        ColumnText ct = new ColumnText(contentByte);
                        //Create the font for show the name of user  
                        doc.Open();
                        PdfPTable modelInfoTable = new PdfPTable(1);
                        modelInfoTable.TotalWidth = 100;
                        modelInfoTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        PdfPCell modelInfoCell1 = new PdfPCell()
                        {
                            BorderWidthBottom = 0,
                            BorderWidthTop = 0,
                            BorderWidthLeft = 0,
                            BorderWidthRight = 0
                        };
                        //Set right hand the first heading  
                        Phrase mainPharse = new Phrase();
                        Chunk mChunk = new Chunk(dt.Rows[0]["FirstName"].ToString() + " " + dt.Rows[0]["LastName"].ToString(), mainFont);
                        mainPharse.Add(mChunk);
                        mainPharse.Add(new Chunk(Environment.NewLine));
                        //Set the user role  
                        Chunk infoChunk1 = new Chunk("Profile - " + "Admin", infoFont1);
                        mainPharse.Add(infoChunk1);
                        mainPharse.Add(new Chunk(Environment.NewLine));
                        //Set the user Gender  
                        Chunk infoChunk21 = new Chunk("Gender - " + "Male", infoFont1);
                        mainPharse.Add(infoChunk21);
                        mainPharse.Add(new Chunk(Environment.NewLine));
                        //Set the user age  
                        Chunk infoChunk22 = new Chunk("Age - " + "44", infoFont1);
                        mainPharse.Add(infoChunk22);
                        mainPharse.Add(new Chunk(Environment.NewLine));
                        iTextSharp.text.Font infoFont2 = FontFactory.GetFont("Kalinga", 10, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#848282")));
                        //string Location = dt.Rows[0]["Location"].ToString() == string.Empty ? string.Empty : "Fountain Valley";
                        Chunk infoChunk2 = new Chunk("Address -" + "Fountain Valley", infoFont2);
                        mainPharse.Add(infoChunk2);
                        modelInfoCell1.AddElement(mainPharse);
                        //Set the mobile image and number  
                        Phrase mobPhrase = new Phrase();
                        // iTextSharp.text.Image mobileImage = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/goodmorning.jpg"));  
                        // mobileImage.ScaleToFit(10, 10);  
                        //Chunk cmobImg = new Chunk(mobileImage, 0, -2);  
                        Chunk cmob = new Chunk("Contact " + "714-785-9631", infoFont2);
                        //mobPhrase.Add(cmobImg);  
                        mobPhrase.Add(cmob);
                        modelInfoCell1.AddElement(mobPhrase);
                        //Set the message image and email id  
                        Phrase msgPhrase = new Phrase();
                        // iTextSharp.text.Image msgImage = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/goodmorning.jpg"));  
                        //msgImage.ScaleToFit(10, 10);  
                        //Chunk msgImg = new Chunk(msgImage, 0, -2);  
                        iTextSharp.text.Font msgFont = FontFactory.GetFont("Kalinga", 10, new iTextSharp.text.BaseColor(System.Drawing.Color.Pink));
                        Chunk cmsg = new Chunk("EMail - " + "sudhirsharma23@gmail.com", msgFont);
                        //msgPhrase.Add(msgImg);  
                        msgPhrase.Add(cmsg);
                        //Set the line after the user small information  
                        iTextSharp.text.Font lineFont = FontFactory.GetFont("Kalinga", 10, new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8")));
                        Chunk lineChunk = new Chunk("____________________________________________________________________", lineFont);
                        msgPhrase.Add(new Chunk(Environment.NewLine));
                        msgPhrase.Add(lineChunk);
                        modelInfoCell1.AddElement(msgPhrase);
                        modelInfoTable.AddCell(modelInfoCell1);
                        //Set the biography  
                        PdfPCell cell1 = new PdfPCell()
                        {
                            BorderWidthBottom = 0,
                            BorderWidthTop = 0,
                            BorderWidthLeft = 0,
                            BorderWidthRight = 0
                        };
                        cell1.PaddingTop = 5;
                        Phrase bioPhrase = new Phrase();
                        Chunk bioChunk = new Chunk("Biography", mainFont);
                        bioPhrase.Add(bioChunk);
                        bioPhrase.Add(new Chunk(Environment.NewLine));
                        Chunk bioInfoChunk = new Chunk("hello", infoFont1);
                        bioPhrase.Add(bioInfoChunk);
                        bioPhrase.Add(new Chunk(Environment.NewLine));
                        bioPhrase.Add(lineChunk);
                        cell1.AddElement(bioPhrase);
                        modelInfoTable.AddCell(cell1);
                        PdfPCell cellExp = new PdfPCell()
                        {
                            BorderWidthBottom = 0,
                            BorderWidthTop = 0,
                            BorderWidthLeft = 0,
                            BorderWidthRight = 0
                        };
                        cellExp.PaddingTop = 5;
                        Phrase ExperiencePhrase = new Phrase();
                        Chunk ExperienceChunk = new Chunk("Experience", mainFont);
                        ExperiencePhrase.Add(ExperienceChunk);
                        cellExp.AddElement(ExperiencePhrase);
                        modelInfoTable.AddCell(cellExp);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < dt.Rows.Count; i++)
                        //    {
                        //        //Set the experience  
                        //        PdfPCell expcell = new PdfPCell()
                        //        {
                        //            BorderWidthBottom = 0,
                        //            BorderWidthTop = 0,
                        //            BorderWidthLeft = 0,
                        //            BorderWidthRight = 0
                        //        };
                        //        expcell.PaddingTop = 5;
                        //        Phrase expPhrase = new Phrase();
                        //        StringBuilder expStringBuilder = new StringBuilder();
                        //        StringBuilder expStringBuilder1 = new StringBuilder();
                        //        //Set the experience details  
                        //        expStringBuilder.Append(dt.Rows[i]["Title"].ToString() + Environment.NewLine);
                        //        expStringBuilder.Append(dt.Rows[i]["CompanyName"].ToString() + Environment.NewLine);
                        //        expStringBuilder.Append(dt.Rows[i]["ComanyAddress"].ToString() + Environment.NewLine);
                        //        expStringBuilder1.Append("From " + dt.Rows[i]["From"].ToString() + " To " + dt.Rows[i]["to"].ToString() + Environment.NewLine);
                        //        // expPhrase.Add(new Chunk(Environment.NewLine));  
                        //        Chunk expDetailChunk = new Chunk(expStringBuilder.ToString(), expHeadFond);
                        //        expPhrase.Add(expDetailChunk);
                        //        expPhrase.Add(new Chunk(expStringBuilder1.ToString(), infoFont2));
                        //        expcell.AddElement(expPhrase);
                        //        modelInfoTable.AddCell(expcell);
                        //        if (dt.Rows[i]["Description"].ToString().Length > 600)
                        //        {
                        //            PdfPCell pCell1 = new PdfPCell()
                        //            {
                        //                BorderWidth = 0
                        //            };
                        //            PdfPCell pCell2 = new PdfPCell()
                        //            {
                        //                BorderWidth = 0
                        //            };
                        //            Phrase ph1 = new Phrase();
                        //            Phrase ph2 = new Phrase();
                        //            string experience1 = dt.Rows[i]["Description"].ToString().Substring(0, 599);
                        //            string experience2 = dt.Rows[i]["Description"].ToString().Substring(599, dt.Rows[i]["Description"].ToString().Length - 600);
                        //            ph1.Add(new Chunk(experience1, infoFont1));
                        //            ph2.Add(new Chunk(experience2, infoFont1));
                        //            pCell1.AddElement(ph1);
                        //            pCell2.AddElement(ph2);
                        //            modelInfoTable.AddCell(pCell1);
                        //            modelInfoTable.AddCell(pCell2);
                        //        }
                        //        else
                        //        {
                        //            PdfPCell pCell1 = new PdfPCell()
                        //            {
                        //                BorderWidth = 0
                        //            };
                        //            Phrase ph1 = new Phrase();
                        //            string experience1 = dt.Rows[i]["Description"].ToString();
                        //            ph1.Add(new Chunk(experience1, infoFont1));
                        //            pCell1.AddElement(ph1);
                        //            modelInfoTable.AddCell(pCell1);
                        //        }
                        //    }
                        //}
                        doc.Add(modelInfoTable);
                        //Set the footer  
                        PdfPTable footerTable = new PdfPTable(1);
                        footerTable.TotalWidth = 644;
                        footerTable.LockedWidth = true;
                        PdfPCell footerCell = new PdfPCell(new Phrase("Resume"));
                        footerCell.BackgroundColor = new iTextSharp.text.BaseColor(Color.Black);
                        iTextSharp.text.Image footerImage = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath("~/Resume/1/goodmorning.png"));
                        footerImage.SpacingBefore = 5;
                        footerImage.SpacingAfter = 5;
                        footerImage.ScaleToFit(100, 22);
                        footerCell.AddElement(footerImage);
                        footerCell.MinimumHeight = 30;
                        iTextSharp.text.Font newFont = FontFactory.GetFont("Segoe UI, Lucida Grande, Lucida Grande", 8, new iTextSharp.text.BaseColor(Color.White));
                        Paragraph rightReservedLabel = new Paragraph("© " + DateTime.Now.Year + " Resume. All rights reserved.", newFont);
                        footerCell.AddElement(rightReservedLabel);
                        footerCell.PaddingLeft = 430;
                        footerTable.AddCell(footerCell);
                        footerTable.WriteSelectedRows(0, -1, 0, doc.PageSize.Height - 795, writer.DirectContent);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        doc.Close();
                    }
                    Response.Redirect("~/ProfileReport.aspx?ID=" + Request.QueryString["Id"]);
                }
            }
        }
    }
}