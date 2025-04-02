using SchoolManagement;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace SchoolManagement
{
    public interface IDbConnectionWrapper : IDisposable
    {
        IDbCommand CreateCommand(); 
        void Open();
        void Close();
        Task OpenAsync();
        Task CloseAsync();
    }
}