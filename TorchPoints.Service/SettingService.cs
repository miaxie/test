using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TorchPoints.Core.DataAccess;
using TorchPoints.Core.Domain;

namespace TorchPoints.Service
{
    public class SettingService : ISetting
    {
        private readonly DapperClient _SqlDB;
        public SettingService(IDapperFactory dapperFactory)
        {
            _SqlDB = dapperFactory.CreateClient("SqlServer");
        }
        public virtual Setting GetSettingByName(string name)
        {
            var sql = new StringBuilder(@"select * from [Setting](nolock)");

            if (!string.IsNullOrEmpty(name))
            {
                sql = sql.AppendFormat(" where name='{0}'", name);
            }
            var result = _SqlDB.Query<Setting>(sql.ToString());
            return result.FirstOrDefault();
        }
    }
}
