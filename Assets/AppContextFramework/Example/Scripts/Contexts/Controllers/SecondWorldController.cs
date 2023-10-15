using ACFW.Controllers;
using ACFW.Example.Views;
using System.Threading.Tasks;
using UnityEngine;

namespace ACFW.Example.Controllers
{
    public class SecondWorldController : ContextController
    {
        private SecondWorldView SecondView => (SecondWorldView)view;
        public SecondWorldController(SecondWorldView view) : base(view)
        {
        }
        public override Task Open()
        {
            return base.Open();
        }
    }
}
