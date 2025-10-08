using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.BaseModel
{
    public class OperationResult
    {
        public int? RecordID { get; private set; }
        public string OperationName { get; private set; }
        public bool Success { get; private set; }
        public DateTime  OperationDate { get; private set; }
        public string Message { get; private set; }
        public HttpStatusCode  StatusCode { get; set; }
        public OperationResult(string OperationName)
        {
            this.OperationName = OperationName;
            this.OperationDate = DateTime.Now;
            this.Success = false;
        }
        public OperationResult ToSuccess(string Message)
        {
            this.Success = true;
            this.Message = Message;
            return this;

        }
        
        public OperationResult ToSuccess(string Message,int RecordID)
        {
            this.Success = true;
            this.Message = Message;
            this.RecordID = RecordID;
            return this;

        }
        public OperationResult ToFail(string Message,HttpStatusCode StatusCode)
        {
            this.Success = false;
            this.Message = Message;
            this.StatusCode = StatusCode;
            return this;

        }
        public OperationResult ToFail(string Message)
        {
            this.Success = false;
            this.Message = Message;
            this.StatusCode = StatusCode;
            return this;

        }
        public OperationResult ToFail(string Message, int RecordID)
        {
            this.Success = false;
            this.Message = Message;
            this.RecordID = RecordID;
            return this;

        }
    }
}
