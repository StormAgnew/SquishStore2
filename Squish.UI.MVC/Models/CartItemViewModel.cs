using Squish.DATA.EF.Models;
using Squish.UI.MVC.Models;

namespace Squish.UI.MVC.Models
{
    public class CartItemViewModel
    {
        
        public int Qty { get; set; }
        public SquishInformation CartProd { get; set; }

        public CartItemViewModel() { }

        public CartItemViewModel(int qty, SquishInformation squishInformation)
        {
            Qty = qty;
            CartProd = squishInformation;
        }

    }
}


