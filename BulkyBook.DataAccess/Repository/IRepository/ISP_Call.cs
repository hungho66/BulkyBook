using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ISP_Call : IDisposable
    {
        //Trả về một int hoặc bool, VD, cột đầu tiên của hàng đầu tiên trong kho dữ liệu 
        T Single<T>(string procedureName, DynamicParameters param = null);


        void Execute(string procedureName, DynamicParameters param = null);

        //Tạo lại một row hoàn chỉnh, sau đó nếu muốn lấy tất cả các row dùng IEnumrable ở dưới
        T OneRecord<T>(string procedureName, DynamicParameters param = null);

        //Trả về 1 bảng
        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);

        //Thủ tục lưu trữ trả về các bảng
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null);
    }
}
