using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToolsCommon;

namespace USBModel
{
    public partial class USBDBHelp
    {
        // IPPrinterSite

        #region + public async Task<List<Tbl_IPPrinterSite>> IPPrinterSite_Get_ALL()
        public async Task<List<Tbl_IPPrinterSite>> IPPrinterSite_Get_ALL()
        {
            try
            {
                var list = await _db.Queryable<Tbl_IPPrinterSite>()
                                    .OrderBy(s => s.UpdateTime, OrderByType.Desc)
                                    .ToListAsync();

                if (list == null || list.Count <= 0)
                {
                    throw new Exception("Tbl_IPPrinterSite is empty.");
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<IPPrinterSiteVM> IPPrinterSiteVM_Get_BySiteId(Guid siteId)
        public async Task<IPPrinterSiteVM> IPPrinterSiteVM_Get_BySiteId(Guid siteId)
        {
            try
            {
                var site = await _db.Queryable<Tbl_IPPrinterSite>().InSingleAsync(siteId);

                if (site == null)
                {
                    throw new Exception("Tbl_IPPrinterSite is empty. Id: " + siteId);
                }

                var printers = await _db.Queryable<Tbl_IPPrinterInfo>()
                                           .Where(p => p.SiteId == site.Id.ToString())
                                           .ToListAsync();

                var siteVM = new IPPrinterSiteVM(site, printers);

                return siteVM;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<Tbl_IPPrinterSite> IPPrinterSite_Get_ById(Guid id)
        public async Task<Tbl_IPPrinterSite> IPPrinterSite_Get_ById(Guid id)
        {
            try
            {
                var query = await _db.Queryable<Tbl_IPPrinterSite>().InSingleAsync(id);
                if (query == null)
                {
                    throw new Exception("Cannot find the Tbl_IPPrinterSite, Id: " + id);
                }

                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterSite_Insert(Tbl_IPPrinterSite site)
        public async Task IPPrinterSite_Insert(Tbl_IPPrinterSite site)
        {
            try
            {
                var existName = await _db.Queryable<Tbl_IPPrinterSite>().FirstAsync(s => s.SiteName == site.SiteName);
                if (existName != null)
                {
                    throw new Exception("IPPrinterSite is exist, name: " + site.SiteName);
                }

                site.Id = Guid.NewGuid();
                site.UpdateTime = DateTime.Now;

                var insert = await _db.Insertable(site).ExecuteCommandIdentityIntoEntityAsync();
                if (!insert)
                {
                    throw new Exception("Tbl_IPPrinterSite insert new fail.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterSite_Update(Tbl_IPPrinterSite site)
        public async Task IPPrinterSite_Update(Tbl_IPPrinterSite site)
        {
            try
            {
                if (site.Id == Guid.Empty)
                {
                    throw new Exception("Id is empty.");
                }

                site.UpdateTime = DateTime.Now;
                await _db.Updateable(site).ExecuteCommandAsync();
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterSite_Delete_ById(Guid siteId)
        public async Task IPPrinterSite_Delete_ById(Guid id)
        {
            try
            {
                await _db.Deleteable<Tbl_IPPrinterSite>().In(t => t.Id, id).ExecuteCommandAsync();

                // delete ipprinter
                await _db.Deleteable<Tbl_IPPrinterInfo>().Where(p => p.SiteId == id.ToString()).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // IPPrinterInfo

        #region + public async Task<Tbl_IPPrinterInfo> IPPrinterInfo_Get_ById(int id)
        public async Task<Tbl_IPPrinterInfo> IPPrinterInfo_Get_ById(int id)
        {
            try
            {
                var printer = await _db.Queryable<Tbl_IPPrinterInfo>().InSingleAsync(id);
                return printer;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<List<Tbl_IPPrinterInfo>> IPPrinterInfo_Get_IList_BySiteSubnetAddr(string subnetAddr)
        public async Task<List<IIPPrinterInfo>> IPPrinterInfo_Get_IList_BySiteSubnetAddr(string subnetAddr)
        {
            try
            {
                var site = await _db.Queryable<Tbl_IPPrinterSite>().FirstAsync(s => s.SubnetAddr == subnetAddr);
                if (site == null)
                {
                    throw new Exception("PrinterSite is empty, Subnet Address: " + subnetAddr);
                }

                var printers = await _db.Queryable<Tbl_IPPrinterInfo>().Where(p => p.SiteId == site.Id.ToString()).ToListAsync();
                if (printers == null || printers.Count <= 0)
                {
                    throw new Exception("Printers is empty, subnet: " + subnetAddr);
                }

                var list = new List<IIPPrinterInfo>();
                foreach (var p in printers)
                {
                    list.Add(p);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<List<Tbl_IPPrinterInfo>> IPPrinterInfo_Get_List()
        public async Task<List<Tbl_IPPrinterInfo>> IPPrinterInfo_Get_List()
        {
            try
            {
                var list = await _db.Queryable<Tbl_IPPrinterInfo>().ToListAsync();
                if (list == null || list.Count <= 0)
                {
                    throw new Exception("Tbl_IPPrinterInfo is empty.");
                }

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region + public async Task<List<Tbl_IPPrinterInfo>> IPPrinterInfo_Get_List_BySiteId(string siteId)
        public async Task<List<Tbl_IPPrinterInfo>> IPPrinterInfo_Get_List_BySiteId(string siteId)
        {
            try
            {
                var list = await _db.Queryable<Tbl_IPPrinterInfo>()
                                    .Where(p => p.SiteId == siteId)
                                    .ToListAsync();

                if (list == null || list.Count <= 0)
                {
                    throw new Exception("Tbl_IPPrinterInfo is empty.");
                }

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterInfo_Insert(Tbl_IPPrinterInfo printer)
        public async Task IPPrinterInfo_Insert(Tbl_IPPrinterInfo printer)
        {
            try
            {
                var insert = await _db.Insertable(printer).ExecuteCommandIdentityIntoEntityAsync();
                if (!insert)
                {
                    throw new Exception("Tbl_IPPrinterInfo insert fail. name: " + printer.PrinterName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterInfo_Update(Tbl_IPPrinterInfo printer)
        public async Task IPPrinterInfo_Update(Tbl_IPPrinterInfo printer)
        {
            try
            {
                if (printer.Id <= 0)
                {
                    throw new Exception("update fail. Id <= 0, name: " + printer.PrinterName);
                }

                var update = await _db.Updateable(printer).ExecuteCommandHasChangeAsync();
                if (!update)
                {
                    throw new Exception("IPPrinterInfo update fail, name:" + printer.PrinterName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task IPPrinterInfo_Delete_ById(int id)
        public async Task IPPrinterInfo_Delete_ById(int id)
        {
            try
            {
                await _db.Deleteable<Tbl_IPPrinterInfo>().In(p => p.Id, id).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        // PerPrintJob

        #region + public async Task PrintJobLog_Insert(Tbl_PerPrintJob printJob)
        public async Task PrintJobLog_Insert(Tbl_PrintJobLog printJob)
        {
            try
            {
                await _db.Insertable(printJob).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task<(int total,List<Tbl_PerPrintJob> list)> PrintJobLog_Get_List_ByComputerIdentity(string comIdentity, int pageIndex, int size)
        public async Task<(int total, List<Tbl_PrintJobLog> list)> PrintJobLog_Get_List_ByComputerIdentity(string comIdentity, int pageIndex, int size)
        {
            try
            {
                RefAsync<int> total = new RefAsync<int>();
                var list = await _db.Queryable<Tbl_PrintJobLog>()
                                    .Where(p => p.ComputerIdentity == comIdentity)
                                    .OrderBy(p => p.PrintingTime, OrderByType.Desc)
                                    .ToPageListAsync(pageIndex, size, total);

                if (list == null || list.Count <= 0)
                {
                    throw new Exception("PrintJobLog is empty.");
                }

                return (total.Value, list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
