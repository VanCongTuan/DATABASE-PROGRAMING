using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVeMayBay.Models;

namespace WebDatVeMayBay.Controllers
{
    public class FlightController : Controller
    {
        string idnv = "";
        public NhanVien GetHotenByAccount(string username, string password)
        {
            var query = from nv in da.NhanViens
                        join tk in da.TaiKhoans on nv.id_TK equals tk.id
                        where tk.username == username && tk.password == password
                        select new { nv.hovaTen, nv.id };

            var result = query.FirstOrDefault();

            if (result != null)
            {
                // Tạo một đối tượng NhanVien từ kết quả truy vấn
                return new NhanVien { hovaTen = result.hovaTen, id = result.id };
            }

            return null;
        }

        [HttpGet]
        public ActionResult NhanVienLogin()
        {


            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult NhanVienLogin(string username, string password)
        {

            var user = da.TaiKhoans.SingleOrDefault(u => u.username == username && u.password == password);


            if (user != null)
            {
                var hoten = GetHotenByAccount(username, password);
                var userRole = user.userrole.ToString();

                Session["hotennv"] = new
                {
                    hovaTen = hoten.hovaTen,
                    role = userRole
                };


                Session["idnv"] = hoten.id;
                idnv = hoten.id;
                if (user.userrole == "NVLL")
                {
                    return RedirectToAction("LapLich");

                }
                if (user.userrole == "NVBV")
                {
                    return RedirectToAction("BanVe");

                }
                if (user.userrole == "AD")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                }
            }
            else
            {
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác, vui lòng thử lại.";
                return RedirectToAction("Index");

            }


            return View();

        }
        // GET: Flight
        public ActionResult Index()
        {
            return View();
        }
        QuanLyMayBayDataContext da = new QuanLyMayBayDataContext();
       
        public string GetIdHangVe(string hangve)
        {
            // Lấy danh sách tất cả các hàng về
            var hangVe = da.HangVes.FirstOrDefault(h => h.loaiHang == hangve);

            if (hangVe != null)
            {
                return hangVe.id;
            }
            else
            {
                return "-1";
            }

           
        }

       
        public string GetTuyenBayBySanBay(string start, string end)
        {
            var sbDi = da.SanBays.FirstOrDefault(sb => sb.diaChi == start);
            var sbDen = da.SanBays.FirstOrDefault(sb => sb.diaChi == end);

            if (sbDi != null && sbDen != null)
            {
                var tuyenBay = da.TuyenBays.FirstOrDefault(tb => tb.id_SbDi == sbDi.maSB && tb.id_SbDen == sbDen.maSB);

                if (tuyenBay != null)
                {
                    return tuyenBay.maTuyenBay;
                }
            }

            // Xử lý trường hợp không tìm thấy tuyến bay hoặc sân bay
            // Ví dụ: Trả về một giá trị mặc định hoặc xử lý lỗi
            return null;
        
        }
        public List<ChuyenBay> GetChuyenBayByTuyenBay(string start, string end)
        {
            string maTuyenBay = GetTuyenBayBySanBay(start, end);

            var chuyenBayList = da.ChuyenBays
                                .Where(cb => cb.id_TuyenBay == maTuyenBay)
                                .ToList();

            return chuyenBayList;
        }


        public List<LichBay> GetLichBay()
        {
            // Thực hiện truy vấn để lấy tất cả các hàng về từ cơ sở dữ liệu
            // Và trả về danh sách các đối tượng HangVe
            return da.LichBays.ToList();
        }
        public List<LichBay> FilterLichBayWithValidTime(DateTime startDate, DateTime dateNow, string id_nv)
        {
            dateNow = dateNow.ToLocalTime(); // Chuyển đổi ngày hiện tại sang múi giờ địa phương nếu cần

            var lichBays = da.LichBays.ToList();
            List<LichBay> filteredLichBays = new List<LichBay>();

            foreach (var lb in lichBays)
            {
                TimeSpan dateDelta = lb.NgayBay - dateNow;

                if (lb.NgayBay.Date == startDate.Date && dateDelta.TotalSeconds / 3600 > 0)
                {
                    filteredLichBays.Add(lb);
                }
            }

            return filteredLichBays;
        }


        public dynamic CountSoGheDaDat(string HangVe, int year, int month, int day)
        {
            var query = from ve in da.VeDats
                        join lichBay in da.LichBays on ve.lichbay_id equals lichBay.MaLB
                        join giaVe in da.LichBay_GiaVes on new { id_LichBay = lichBay.MaLB, id_HangVe = ve.maHangVe } equals new { giaVe.id_LichBay, giaVe.id_HangVe }
                        where ve.maHangVe == HangVe &&
                              lichBay.NgayBay.Year == year &&
                              lichBay.NgayBay.Month == month &&
                              lichBay.NgayBay.Day == day
                        group new { ve, lichBay, giaVe } by new { ve.lichbay_id, ve.maHangVe, lichBay.id_ChuyenBay, giaVe.SoLuongGhe } into g
                        select new
                        {
                            g.Key.lichbay_id,
                            g.Key.maHangVe,
                            g.Key.id_ChuyenBay,
                            g.Key.SoLuongGhe,
                            SoLuongVeDaDat = g.Count()
                        };

            return query.ToList<object>();
        }
        public List<LichBay> GetLichBayWithChuyenBays(List<ChuyenBay> listChuyenBay, List<LichBay> listLichBayFilterByTime)
        {
            List<LichBay> listLB = new List<LichBay>(); // Khởi tạo danh sách mới

            foreach (var chuyenBay in listChuyenBay)
            {
                foreach (var lb in listLichBayFilterByTime)
                {
                    if (chuyenBay.MaCB == lb.id_ChuyenBay)
                    {
                        listLB.Add(lb); // Thêm lichBay vào danh sách
                    }
                }
            }

            return listLB;
        }


        public string GetMayBay(string idMb)
        {


            var mayBay = da.MayBays
                                .Where(mb => mb.maMb == idMb)
                                .Select(mb => mb.tenMB)
                                .FirstOrDefault();
            return mayBay;

        }

        public dynamic GetGiaVe(string HangVe)
        {
            var query = from lichBay in da.LichBays
                        join lichBayGiaVe in da.LichBay_GiaVes on lichBay.MaLB equals lichBayGiaVe.id_LichBay
                        join giaVe in da.GiaVes on lichBayGiaVe.id_GiaVe equals giaVe.id
                        where lichBayGiaVe.id_HangVe == HangVe
                        select new
                        {
                            MaLB = lichBay.MaLB,
                            id_HangVe = lichBayGiaVe.id_HangVe,
                            Gia = giaVe.Gia
                        };

            return query.ToList(); ;
        }


        public dynamic GetSanBayTrungGian(string idChuyenBay)
        {

            var sanBayTrungGianList = (from sb in da.SanBays
                                       join sbtg in da.SanBayTrungGians on sb.maSB equals sbtg.maSb
                                       where sbtg.maCb == idChuyenBay
                                       select new
                                       {
                                           tenSB = sb.tenSB,
                                           diaChi = sb.diaChi,
                                           thoiGianDung = sbtg.thoiGianDung,
                                           GhiChu = sbtg.GhiChu,
                                           maCb = sbtg.maCb
                                       }).ToList();

            return sanBayTrungGianList.ToList<object>();


        }
        public dynamic GetValidLichBay(string start, string end, DateTime timeNow, int tongnguoi, string tiklevel, DateTime startDate, string idnv)
        {
            List<object> finalRes = new List<object>();

            DateTime parsedStartDate;
            if (startDate != null)
            {
                parsedStartDate = (DateTime)startDate;
            }
            else
            {

                parsedStartDate = DateTime.MinValue;
            }
            string parsedTiklevel = GetIdHangVe(tiklevel);

            var soHieuChuyenBay = GetChuyenBayByTuyenBay(start, end);
            var listLBValidTime = FilterLichBayWithValidTime(parsedStartDate, timeNow, idnv);
            var listSoLuongVeDaDat = CountSoGheDaDat(parsedTiklevel, parsedStartDate.Year, parsedStartDate.Month, parsedStartDate.Day);
            var listLB = GetLichBayWithChuyenBays(soHieuChuyenBay, listLBValidTime);

            var lichBayValidConGhe = (from dict2 in listSoLuongVeDaDat as IEnumerable<dynamic>
                                      from lb in listLB
                                      where lb.MaLB == dict2.lichbay_id && dict2.SoLuongGhe - dict2.SoLuongVeDaDat >= tongnguoi
                                      select lb).ToList();



            var listLichBayDaDat = new List<string>();
            foreach (var obj in listSoLuongVeDaDat)
            {
                listLichBayDaDat.Add(obj.lichbay_id);
            }


            var LichBay_Valid_GheTrong = listLB.Where(lb => !listLichBayDaDat.Contains(lb.MaLB)).ToList();

            var lichBayValidGheTrong = listLB.Where(lb => !listLichBayDaDat.Contains(lb.MaLB)).ToList();

            var result = lichBayValidConGhe.Concat(lichBayValidGheTrong).ToList();

            foreach (var lb in result)
            {
                foreach (var gv in GetGiaVe(parsedTiklevel))
                {
                    if (lb.MaLB == gv.MaLB)
                    {
                        finalRes.Add(new
                        {
                            MaLB = lb.MaLB,
                            MaCB = lb.id_ChuyenBay,
                            MayBay = GetMayBay(lb.id_MayBay),
                            NgayKhoiHanh = lb.NgayBay,
                            ThoiGianDi = lb.thoiGianBay,
                            HangVe = tiklevel,
                            SanBayTrungGian = GetSanBayTrungGian(lb.id_ChuyenBay),
                            GiaVe = gv.Gia
                        });
                    }
                }
            }

            return finalRes;
        }

        string URL = "";
        int LenSS = 0;
        public void StoreSearchInformation()
        {
            var form = Request.Form;

            var SearchInformation = new Dictionary<string, object>
        {
            { "start", form["StartDes"] },
            { "end", form["EndDes"] },
            { "adultNum", Convert.ToInt32(form["adultNumber"]) },
            { "childrenNum", Convert.ToInt32(form["childrenNumber"]) },
            { "ticketLevel", form["level"] },
            { "startDate", form["StartDate"] },
            { "timeNow", DateTime.Now }
        };

            if (!string.IsNullOrEmpty(form["one-way"]))
            {
                SearchInformation["cateFlight"] = form["one-way"];
            }
            else
            {
                SearchInformation["cateFlight"] = form["round-trip"];
                SearchInformation["endDate"] = form["EndDate"];
            }

            Session["thongtintimkiem"] = SearchInformation;
        }


        //[HttpGet]
        //[HttpPost]
        //public ActionResult chuyenbay(string time)
        //{
        //    var SSI = new Dictionary<string, object>();

        //    DateTime? startDate = null;

        //    if (Request.HttpMethod == "POST")
        //    {
        //        if (URL == "/chuyenbay")
        //        {
        //            SSI = Session["thongtintimkiem"] as Dictionary<string, object>;
        //        }
        //        else
        //        {
        //            StoreSearchInformation();
        //            SSI = Session["thongtintimkiem"] as Dictionary<string, object>;
        //        }

        //    }
        //    else if (Request.HttpMethod == "GET")
        //    {
        //        if (!string.IsNullOrEmpty(time))
        //        {
        //            startDate = DateTime.ParseExact(time, "yyyy-MM-dd", null);
        //        }

        //        SSI = Session["thongtintimkiem"] as Dictionary<string, object>;

        //        if (LenSS > 0)
        //        {
        //            string temp = SSI["end"].ToString();
        //            SSI["end"] = SSI["start"];
        //            SSI["start"] = temp;
        //        }

        //        SSI["startDate"] = startDate;

        //    }

        //    DateTime day = DateTime.Parse(SSI["timeNow"].ToString()).Date;
        //    List<DateTime> dateRange = new List<DateTime>();
        //    for (int i = 0; i < 30; i++)
        //    {
        //        dateRange.Add(day.AddDays(i));


        //    }

        //    int tongNguoi = (int)SSI["adultNum"] + (int)SSI["childrenNum"];

        //    if (startDate == null && SSI["cateFlight"].ToString() == "round-trip")
        //    {
        //        URL = "/chuyenbay";
        //    }
        //    if (SSI["cateFlight"].ToString() == "one-way")
        //    {
        //        URL = "/ThongTinHanhKhach";
        //    }


        //    if (URL == "/chuyenbay" && LenSS > 0)
        //    {
        //        URL = "/ThongTinHanhKhach";
        //        var LichBay = GetValidLichBay(SSI["start"].ToString(), SSI["end"].ToString(),
        //                                    DateTime.Parse(SSI["timeNow"].ToString())
        //                                    , tongNguoi, SSI["ticketLevel"].ToString(),
        //                                    DateTime.Parse(SSI["endDate"].ToString()),idnv);

        //        return RedirectToAction("chuyenbay", "FlightController", new
        //        {
        //            SSI = SSI,
        //            soluong = tongNguoi,
        //            soluongngl = (int)SSI["adultNum"],
        //            soluongtre = (int)SSI["childrenNum"],
        //            date_range = dateRange,
        //            LichBayArr = LichBay,
        //            current_day = SSI["timeNow"],
        //            dayStart = SSI["endDate"],
        //            cateFlight = SSI["cateFlight"],
        //            noidi = SSI["end"],
        //            noiden = SSI["start"],
        //            URL = URL
        //        });
        //    }

        //    var LichBay2 = GetValidLichBay(SSI["start"].ToString(), SSI["end"].ToString(),
        //                        DateTime.Parse(SSI["timeNow"].ToString())
        //                        , tongNguoi, SSI["ticketLevel"].ToString(),
        //                        DateTime.Parse(SSI["startDate"].ToString()), idnv);

        //    return RedirectToAction("chuyenbay", "FlightController", new
        //    {
        //        SSI = SSI,
        //        soluong = tongNguoi,
        //        soluongngl = (int)SSI["adultNum"],
        //        soluongtre = (int)SSI["childrenNum"],
        //        date_range = dateRange,
        //        LichBayArr = LichBay2,
        //        current_day = SSI["timeNow"],
        //        dayStart = SSI["endDate"],
        //        cateFlight = SSI["cateFlight"],
        //        noidi = SSI["end"],
        //        noiden = SSI["start"],
        //        URL = URL
        //    });
        //}


        //[HttpGet]
        //public ActionResult chuyenbay(string time)
        //{
        //    StoreSearchInformation();
        //    var SSI = new Dictionary<string, object>();
        //    DateTime startDate = DateTime.MinValue; // Initialize startDate to a default value

        //    if (!string.IsNullOrEmpty(time))
        //    {
        //        startDate = DateTime.ParseExact(time, "yyyy-MM-dd", null);
        //    }

        //    SSI = Session["thongtintimkiem"] as Dictionary<string, object>;

        //    if (SSI != null && SSI.Count > 0 && SSI.ContainsKey("start") && SSI.ContainsKey("end") && SSI["end"] != null)
        //    {
        //        string temp = SSI["end"].ToString();
        //        SSI["end"] = SSI["start"];
        //        SSI["start"] = temp;
        //    }

        //    SSI["startDate"] = startDate;

        //    DateTime day = DateTime.Parse(SSI["timeNow"].ToString()).Date;
        //    List<DateTime> dateRange = new List<DateTime>();
        //    for (int i = 0; i < 30; i++)
        //    {
        //        dateRange.Add(day.AddDays(i));
        //    }

        //    int tongNguoi = (int)SSI["adultNum"] + (int)SSI["childrenNum"];
        //    string URL = string.Empty;

        //    if (SSI != null && SSI.ContainsKey("cateFlight") && SSI["cateFlight"] != null && SSI["cateFlight"].ToString() == "round-trip")
        //    {
        //        URL = "/chuyenbay";
        //    }
        //    if (SSI != null && SSI.ContainsKey("cateFlight") && SSI["cateFlight"] != null && SSI["cateFlight"].ToString() == "one-way")
        //    {
        //        URL = "/ThongTinHanhKhach";
        //    }

        //    if (URL == "/chuyenbay" && SSI != null && SSI.Count > 0)
        //    {
        //        URL = "/ThongTinHanhKhach";
        //        var LichBay = GetValidLichBay(SSI["start"].ToString(), SSI["end"].ToString(),
        //                                      DateTime.Parse(SSI["timeNow"].ToString()),
        //                                      tongNguoi, SSI["ticketLevel"].ToString(),
        //                                      DateTime.Parse(SSI["endDate"].ToString()), idnv);

        //        return View("chuyenbay", new
        //        {
        //            SSI = SSI,
        //            soluong = tongNguoi,
        //            soluongngl = (int)SSI["adultNum"],
        //            soluongtre = (int)SSI["childrenNum"],
        //            date_range = dateRange,
        //            LichBayArr = LichBay,
        //            current_day = SSI["timeNow"],
        //            dayStart = SSI["endDate"],
        //            cateFlight = SSI["cateFlight"],
        //            noidi = SSI["end"],
        //            noiden = SSI["start"],
        //            URL = URL
        //        });
        //    }

        //    DateTime timeNow = DateTime.Now;
        //    if (SSI != null && SSI.ContainsKey("timeNow") && SSI["timeNow"] != null)
        //    {
        //        DateTime.TryParse(SSI["timeNow"].ToString(), out timeNow);
        //    }

        //    var LichBay2 = GetValidLichBay(
        //        SSI["start"]?.ToString(),
        //        SSI["end"]?.ToString(),
        //        timeNow,
        //        tongNguoi,
        //        SSI["ticketLevel"]?.ToString(),
        //        startDate,
        //        idnv
        //    );

        //    return View("chuyenbay", new
        //    {
        //        SSI = SSI,
        //        soluong = tongNguoi,
        //        soluongngl = (int)SSI["adultNum"],
        //        soluongtre = (int)SSI["childrenNum"],
        //        date_range = dateRange,
        //        LichBayArr = LichBay2,
        //        current_day = SSI["timeNow"],
        //        dayStart = SSI["endDate"],
        //        cateFlight = SSI["cateFlight"],
        //        noidi = SSI["end"],
        //        noiden = SSI["start"],
        //        URL = URL
        //    });
        //}
        [HttpGet]
        public ActionResult chuyenbay(string time)
        {
            StoreSearchInformation();
            var SSI = Session["thongtintimkiem"] as Dictionary<string, object>;
            if (SSI == null)
            {
                // Handle the case where SSI is null, e.g., redirect to an error page or initialize it
                return RedirectToAction("Index"); // Example: redirecting to the index action
            }

            DateTime startDate = DateTime.MinValue; // Initialize startDate to a default value

            if (!string.IsNullOrEmpty(time))
            {
                startDate = DateTime.ParseExact(time, "yyyy-MM-dd", null);
            }

            if (SSI.ContainsKey("start") && SSI.ContainsKey("end") && SSI["end"] != null)
            {
                string temp = SSI["end"].ToString();
                SSI["end"] = SSI["start"];
                SSI["start"] = temp;
            }

            SSI["startDate"] = startDate;

            DateTime day = DateTime.Parse(SSI["timeNow"].ToString()).Date;
            var dateRange = new List<DateTime>();
            for (int i = 0; i < 30; i++)
            {
                dateRange.Add(day.AddDays(i));
            }
            if (SSI != null && SSI.ContainsKey("cateFlight") && SSI["cateFlight"] != null && SSI["cateFlight"].ToString() == "round-trip")
            {
                URL = "/chuyenbay";
            }
            if (SSI != null && SSI.ContainsKey("cateFlight") && SSI["cateFlight"] != null && SSI["cateFlight"].ToString() == "one-way")
            {
                URL = "/ThongTinHanhKhach";
            }
            int tongNguoi = (int)SSI["adultNum"] + (int)SSI["childrenNum"];


            if (URL == "/chuyenbay" && SSI.Count > 0)
            {
                URL = "/ThongTinHanhKhach";
                var LichBay = GetValidLichBay(
                    SSI["start"].ToString(),
                    SSI["end"].ToString(),
                    DateTime.Parse(SSI["timeNow"].ToString()),
                    tongNguoi,
                    SSI["ticketLevel"].ToString(),
                    DateTime.Parse(SSI["endDate"].ToString()),
                    "idnv"
                );


                ViewBag.soluong = tongNguoi;
                ViewBag.soluongngl = (int)SSI["adultNum"];
                ViewBag.soluongtre = (int)SSI["childrenNum"];
                ViewBag.date_range = dateRange;
                ViewBag.LichBayArr = LichBay;
                ViewBag.current_day = SSI["timeNow"];
                ViewBag.dayStart = SSI["endDate"];
                ViewBag.cateFlight = SSI["cateFlight"];
                ViewBag.noidi = SSI["end"];
                ViewBag.noiden = SSI["start"];
                ViewBag.URL = URL;

                return View();
            }

            DateTime timeNow = DateTime.Now;
            if (SSI.ContainsKey("timeNow"))
            {
                DateTime.TryParse(SSI["timeNow"].ToString(), out timeNow);
            }

            var LichBay2 = GetValidLichBay(
                SSI["start"]?.ToString(),
                SSI["end"]?.ToString(),
                timeNow,
                tongNguoi,
                SSI["ticketLevel"]?.ToString(),
                startDate,
                "idnv"
            );


            ViewBag.soluong = tongNguoi;
            ViewBag.soluongngl = (int)SSI["adultNum"];
            ViewBag.soluongtre = (int)SSI["childrenNum"];
            ViewBag.date_range = dateRange;
            ViewBag.LichBayArr = LichBay2;
            ViewBag.current_day = SSI["timeNow"];
            ViewBag.dayStart = SSI["endDate"];
            ViewBag.cateFlight = SSI["cateFlight"];
            ViewBag.noidi = SSI["end"];
            ViewBag.noiden = SSI["start"];
            ViewBag.URL = URL;


            return View();
        }
    
        
        [HttpPost]
public ActionResult chuyenbay()
{
            StoreSearchInformation();
    var SSI = new Dictionary<string, object>();

    if (URL == "/chuyenbay")
    {
        SSI = Session["thongtintimkiem"] as Dictionary<string, object>;
    }
    else
    {
        StoreSearchInformation();
        SSI = Session["thongtintimkiem"] as Dictionary<string, object>;
    }
   
    DateTime day = DateTime.Parse(SSI["timeNow"].ToString()).Date;
    List<DateTime> dateRange = new List<DateTime>();
    for (int i = 0; i < 30; i++)
    {
        dateRange.Add(day.AddDays(i));
    }

    int tongNguoi = (int)SSI["adultNum"] + (int)SSI["childrenNum"];

    if (URL == "/chuyenbay" && LenSS > 0)
    {
        URL = "/ThongTinHanhKhach";
        var LichBay = GetValidLichBay(SSI["start"].ToString(), SSI["end"].ToString(),
                                    DateTime.Parse(SSI["timeNow"].ToString())
                                    , tongNguoi, SSI["ticketLevel"].ToString(),
                                    DateTime.Parse(SSI["endDate"].ToString()),idnv);
       
        return View("chuyenbay", new
        {
            SSI = SSI,
            soluong = tongNguoi,
            soluongngl = (int)SSI["adultNum"],
            soluongtre = (int)SSI["childrenNum"],
            date_range = dateRange,
            LichBayArr = LichBay,
            current_day = SSI["timeNow"],
            dayStart = SSI["endDate"],
            cateFlight = SSI["cateFlight"],
            noidi = SSI["end"],
            noiden = SSI["start"],
            URL = URL
        });
    }
   
    var LichBay2 = GetValidLichBay(SSI["start"].ToString(), SSI["end"].ToString(),
                        DateTime.Parse(SSI["timeNow"].ToString())
                        , tongNguoi, SSI["ticketLevel"].ToString(),
                        DateTime.Parse(SSI["startDate"].ToString()), idnv);
          
   
    return View("chuyenbay", new
    {
        SSI = SSI,
        soluong = tongNguoi,
        soluongngl = (int)SSI["adultNum"],
        soluongtre = (int)SSI["childrenNum"],
        date_range = dateRange,
        LichBayArr = LichBay2,
        current_day = SSI["timeNow"],
        dayStart = SSI["endDate"],
        cateFlight = SSI["cateFlight"],
        noidi = SSI["end"],
        noiden = SSI["start"],
        URL = URL
    });
}

    } 
}
