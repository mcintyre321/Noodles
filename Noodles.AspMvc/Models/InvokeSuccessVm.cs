namespace Noodles.AspMvc.Models
{
    public class InvokeSuccessVm
    {
        public IInvokeable Method { get; set; }
        public object Result { get; set; }

        public InvokeSuccessVm(IInvokeable method, object result)
        {
            Method = method;
            Result = result;
        }
    }
}