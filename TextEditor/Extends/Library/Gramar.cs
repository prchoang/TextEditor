﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextEditor.Extends.Library
{
    public class Gramar
    {
        public bool Check(string am)
        {
            var ctam = GetAmGiua(am.ToLower());
            if (ctam == null)
            {
                return false;
            }
            var am_giua_ok = FindString(ctam.AmGiua, am_giua);
            if (!am_giua_ok)
            {
                return false;
            }
            else
            {
                if(ctam.AmDau == "")
                {
                    if(ctam.AmCuoi == "")
                    {
                        return true;
                    }
                    else
                    {
                        var am_cuoi_ok = FindString(ctam.AmCuoi, am_cuoi);

                        if (!am_cuoi_ok)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    var am_dau_ok = FindString(ctam.AmDau, am_dau);
                    if (!am_dau_ok)
                    {
                        return false;
                    }
                }
            }

            return true;

        }

        public bool FindString(string key, string[] list)
        {
            if (key.Equals(""))
                return true;
            foreach (var l in list)
            {
                if (l.Equals(key.ToLower()))
                    return true;
            }
            return false;
        }

        public class CTAM
        {
            public string AmDau { get; set; }
            public string AmGiua { get; set; }
            public string AmCuoi { get; set; }
        }

        public CTAM GetAmGiua(string am)
        {
            string AmDau = "";
            string AmGiua = "";
            string AmCuoi = "";
            bool ok = false;
            string[] am_tiet = am.ToCharArray().Select(c => c.ToString()).ToArray();
            switch (am_tiet.Length)
            {
                case 1:
                    {
                        AmDau = "";
                        AmGiua = am;
                        AmCuoi = "";
                        ok = true;
                        break;
                    }
                case 2:
                    {
                        foreach (var ad in am_dau)
                        {
                            if (ad == am_tiet[0])
                            {
                                AmDau = ad;
                                AmGiua = am.Replace(ad, "");
                                AmCuoi = "";
                                ok = true;
                            }
                        }
                        if (!ok)
                        {
                            foreach (var ac in am_cuoi)
                            {
                                AmDau = "";
                                AmGiua = am.Replace(ac, "");
                                AmCuoi = ac;
                                ok = true;
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        string tmp_am_dau = am_tiet[0] + am_tiet[1];
                        foreach (var ad in am_dau)
                        {
                            if (ad == tmp_am_dau)
                            {
                                AmDau = ad;
                                AmGiua = am.Replace(tmp_am_dau, "");
                                AmCuoi = "";
                                ok = true;
                            }
                        }
                        if (!ok)
                        {
                            tmp_am_dau = "";
                            string tmp_am_cuoi = am_tiet[2] + am_tiet[1];
                            foreach (var ac in am_cuoi)
                            {
                                if (ac == tmp_am_cuoi)
                                {
                                    AmDau = "";
                                    AmGiua = am.Replace(ac, "");
                                    AmCuoi = ac;
                                }
                            }
                        }
                        if (!ok)
                        {
                            tmp_am_dau = am_tiet[0];
                            foreach (var ad in am_dau)
                            {
                                if (ad == tmp_am_dau)
                                {
                                    AmDau = ad;
                                    AmGiua = am.Replace(ad, "");
                                    am_tiet = AmGiua.ToCharArray().Select(c => c.ToString()).ToArray();
                                    foreach (var ac in am_cuoi)
                                    {
                                        try
                                        {
                                            if (ac == am_tiet[1])
                                            {
                                                AmGiua = AmGiua.Replace(ac, "");
                                                AmCuoi = ac;
                                                ok = true;
                                            }
                                        }
                                        catch
                                        {

                                        }

                                    }
                                    if (!ok)
                                    {

                                        AmGiua = AmGiua;
                                        AmCuoi = "";
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        break;
                    }
                case 5:
                    {
                        break;
                    }
                case 6: { break; }
            }

            if (AmGiua == "")
            {
                return null;
            }

            var ctam = new CTAM();
            ctam.AmDau = AmDau;
            ctam.AmGiua = AmGiua;
            ctam.AmCuoi = AmCuoi;

            return ctam;

        }

        public string[] am_dau = new string[] { "b", "c", "ch", "d", "đ", "g", "gh", "h", "k", "kh", "l", "m", "n", "ng", "ngh", "nh", "p", "ph", "q", "r", "s", "t", "th", "tr", "v", "x", "none" };
        public string[] am_cuoi = new string[] { "c", "ch", "m", "n", "ng", "nh", "p", "t", "none" };
        public string[] am_giua = new string[] { "a", "á", "ã", "à", "ạ", "ấ", "ầ", "ẫ", "ậ", "â", "ắ", "ằ", "ặ", "ẵ", "ă", "ô", "ồ", "ố", "ộ", "ỗ", "u", "ụ", "ú", "ù", "ũ", "u", "i", "ì", "í", "ị", "ĩ", "e", "é", "è", "ẹ", "ẽ", "ê", "ề", "ế", "ệ", "ễ", "y", "ý", "ỳ", "ỵ", "ỹ", "uy", "ùy", "úy", "ũy", "ụy", "ai", "ái", "ài", "ại", "ãi", "ơi", "ới", "ời", "ỡi", "ợi", "o", "ò", "ó", "ọ", "õ", "ơ", "ờ", "ớ", "ợ", "ỡ", "oc", "óc", "ọc", "iê", "iề", "iế", "iệ", "iễ", "iếu", "iều", "iễu", "iệu", "iêu", "an", "ản", "án", "àn", "ạn", "an", "iên", "iện", "iền", "iễn", "iến", "uật", "uất", "uân", "uận", "uần", "uấn", "uận", "uẫn", "ư", "ự", "ừ", "ứ", "ữ", "inh", "ính", "ình", "ĩnh", "ịnh","ây","ầy","ấy","ẫy","ậy","ững","ứng","ừng","ựng" };
    }
}