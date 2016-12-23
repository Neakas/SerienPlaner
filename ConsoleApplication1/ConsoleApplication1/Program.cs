using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using amxDatenImport;
using Excel;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> Artikelmatchdict = new Dictionary<int, string>()
            {
                { 1,amxImportHelper.KHKArtikelPropEnum.Artikelnummer.ToString()},
                { 2,amxImportHelper.KHKArtikelPropEnum.Bezeichnung1.ToString()},
                { 3,amxImportHelper.KHKArtikelPropEnum.Bezeichnung2.ToString()},
                { 4,amxImportHelper.KHKArtikelPropEnum.Matchcode.ToString()},
                { 8,amxImportHelper.KHKArtikelPropEnum.HArtikelnummer.ToString()},
                { 9,amxImportHelper.KHKArtikelPropEnum.Basismengeneinheit.ToString()},
                { 10,"User_Laengecm"},
                { 11,"User_Breitecm"},
                { 12,"User_Hoehecm"},
                { 13,"User_Gewichtkg"},
                { 17,amxImportHelper.KHKArtikelPropEnum.Memo.ToString()},
                { 18,amxImportHelper.KHKArtikelPropEnum.Artikelgruppe.ToString()},
                { 22,amxImportHelper.KHKArtikelPropEnum.Steuerklasse.ToString()},
                { 24,"User_Produktfamilie"},
                { 25,"User_Produktgruppe"},
                { 26,"User_HDB"},
                { 27,"User_FlagshipOnly"},
                { 28,amxImportHelper.KHKArtikelPropEnum.IstRabattfaehig.ToString()},
                { 29,"User_Lizenzabrechnung"},
            };

            Dictionary<int, string> ArtikelVariantenmatchdict = new Dictionary<int, string>()
            {
                { 16,amxImportHelper.KHKArtikelVariantenPropEnum.EANNummer.ToString()},
                { 21,amxImportHelper.KHKArtikelVariantenPropEnum.MittlererEK.ToString()},
            };

            Dictionary<int, string> Preislistenartikelmatchdicht = new Dictionary<int, string>()
            {
                { 19,"HNP"},
                { 20,"Empf.Vk"},
            };

            string filePath = @"C:\Test\test.xlsx";
            using (var context = new OLDemoReweAbfDEntities())
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                    {
                        excelReader.IsFirstRowAsColumnNames = true;
                        List<KHKArtikel> adresslist = AmxDatenImport.Transform<KHKArtikel>(excelReader.AsDataSet(), 0, Artikelmatchdict, 2, 1);
                        KHKArtikel artikel = new KHKArtikel();


                        artikel.PlatzID = 1423;
                    }
                }
            }
            KHKArtikelVarianten var = new KHKArtikelVarianten();
            foreach (var prop in typeof(KHKArtikelVarianten).GetProperties())
            {
                Debug.WriteLine(prop.Name + ",");
            }
        }
    }
}
