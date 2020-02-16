using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegisterationMVC.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.IO;
using WebGrease.Css.Ast;
using Newtonsoft.Json;

namespace RegisterationMVC.Controllers
{
    public class NewRegisterationController : Controller
    {
        // GET: NewRegisteration
        public ActionResult Index()
        {
            var model = new Registeration();
            return View(model);
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Index(Registeration registeration)
        {
            using (var client = new HttpClient())
            {
                //foreach (var file in registeration.File)
                //{
                //    byte[] fileData = new byte[file.ContentLength];
                //    await file.InputStream.ReadAsync(fileData, 0, file.ContentLength);
                //    registeration.FileData.Add(fileData);
                //}
                //byte[] bytes;
                //using (BinaryReader br = new BinaryReader(registeration.File.InputStream))
                //{
                //    bytes = br.ReadBytes(registeration.File.ContentLength);
                //}
                registeration.FirstName = registeration.FirstName;
                registeration.LastName = registeration.LastName;
                registeration.Email = registeration.Email;
                registeration.City = registeration.City;
                registeration.MobileNumber = registeration.MobileNumber;
                registeration.Bill = null;
                var baseurl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Accept.Clear();
                //HttpResponseMessage response = client.PostAsync("Registerations", registeration);
                //client.Timeout = TimeSpan.FromMinutes(5);
                //var postTask = client.PostAsJsonAsync<Registeration>("api/Registeration", registeration);
                //var postTask = client.PostAsync("Registerations", registeration);
                //HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("http://localhost:8082/");
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Registeration", registeration);
                if (response.IsSuccessStatusCode == true)
                {
                    return View();
                }
                //postTask.Wait();
                //var result = postTask.Result;
                //if (result.IsSuccessStatusCode)
                //{
                //    var readTask = result.Content.ReadAsAsync<Registeration>();
                //    readTask.Wait();

                //    var insertedStudent = readTask.Result;


                //}

            }
                return View();
        }

        [HttpPost]
        public ActionResult Register(Registeration register)
        {
            try
            {
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(register.File.InputStream))
                {
                    bytes = br.ReadBytes(register.File.ContentLength);
                }
                //Attempt to register:
                using (var client = new HttpClient())
                {
                    var response =
                        client.PostAsJsonAsync("http://localhost:64169/api/Registeration",

                        // Pass in an anonymous object that maps to the expected 
                        // RegisterUserBindingModel defined as the method parameter 
                        // for the Register method on the API:
                        new
                        {
                            FirstName = register.FirstName,
                            LastName = register.LastName,
                            Email = register.Email,
                            City = register.City,
                            MobileNumber = register.MobileNumber,
                            Bill = bytes
                        }).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        return PartialView("Error");
                        //return Response.Redirect(new Uri(Request.Url, Url.Action("Completed", "NewRegisteration")).ToString());
                        // Unwrap the response and throw as an Api Exception:
                        //var ex = CreateApiException(response);
                        //throw ex;
                    }
                    //else if (response.IsSuccessStatusCode) {
                    return RedirectToAction("Completed", "NewRegisteration");
                    //return Response.Redirect(Url.Action("Completed", "NewRegisteration"));
                    //}
                    //return response;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Completed()
        {
            return View();
        }

        //public static AstException CreateApiException(HttpResponseMessage response)
        //{
        //    var httpErrorObject = response.Content.ReadAsStringAsync().Result;

        //    // Create an anonymous object to use as the template for deserialization:
        //    var anonymousErrorObject =
        //        new { message = "", ModelState = new Dictionary<string, string[]>() };

        //    // Deserialize:
        //    var deserializedErrorObject =
        //        JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

        //    // Now wrap into an exception which best fullfills the needs of your application:
        //    var ex = new ApiException(response);

        //    // Sometimes, there may be Model Errors:
        //    if (deserializedErrorObject.ModelState != null)
        //    {
        //        var errors =
        //            deserializedErrorObject.ModelState
        //                                    .Select(kvp => string.Join(". ", kvp.Value));
        //        for (int i = 0; i < errors.Count(); i++)
        //        {
        //            // Wrap the errors up into the base Exception.Data Dictionary:
        //            ex.Data.Add(i, errors.ElementAt(i));
        //        }
        //    }
        //    // Othertimes, there may not be Model Errors:
        //    else
        //    {
        //        var error =
        //            JsonConvert.DeserializeObject<Dictionary<string, string>>(httpErrorObject);
        //        foreach (var kvp in error)
        //        {
        //            // Wrap the errors up into the base Exception.Data Dictionary:
        //            ex.Data.Add(kvp.Key, kvp.Value);
        //        }
        //    }
        //    return ex;
        //}
        public string Uploadfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if(file !=null && file.ContentLength>0)
            {
                string extention = Path.GetExtension(file.FileName);
                if(extention.ToLower().Equals(".jpg")|| extention.ToLower().Equals(".jpge")|| extention.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                    }
                    catch(Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('only image or pdf files only')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Plese select files')</script>");
                path = "-1";
            }

            return path;
        }

    }
}