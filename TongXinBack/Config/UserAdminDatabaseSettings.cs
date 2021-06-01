using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TongXinBack.Config
{
    public interface UserAdminDatabaseSettings
    {
        string UserCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class UserAdminDatabaseSettingsImpl : UserAdminDatabaseSettings
    {
        public string UserCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
