﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextEditor.Models;
using System.Net;
using System.Net.Http;
using Microsoft.AspNet.Identity;

namespace TextEditor.Controllers
{
    public class FileController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        // GET: File
        public ActionResult Index()
        {
            var filetable = db.FileTable.ToList();
            var model = new CrudFileTable();
            model.FiletableView = filetable;

            return View(model);
        }

        const string path = "~/Upload";

        [HttpPost]
        public ActionResult Index(CrudFileTable cft, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                var file = new FileTable();
                file.Time = DateTime.Now;
                file.Path = path;
                file.UserId = User.Identity.GetUserId();
                file.Name = file.Time.Year+file.Time.Month+file.Time.Day+file.Time.Hour+file.Time.Minute+file.Time.Second +"_"+ Path.GetFileName(fileUpload.FileName);

                if (fileUpload.ContentLength != 0)
                {
                    string pathForSaving = Server.MapPath(path);
                    if (CreateFolderIfNeeded(pathForSaving))
                    {
                        try
                        {
                            byte[] t = new byte[fileUpload.InputStream.Length];
                            fileUpload.InputStream.Read(t, 0, (int)fileUpload.InputStream.Length);
                            WriteFile(file.Path + "/" + file.Name, t);

                            db.FileTable.Add(file);
                            db.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                return RedirectToAction("index");
            }
            return View(cft);
        }

        public static void WriteFile(string fileName, byte[] bytes)
        {

            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(fileName)))
                System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(fileName));
            try
            {
                using (FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(fileName), FileMode.CreateNew, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            return View();
        }
    }
}