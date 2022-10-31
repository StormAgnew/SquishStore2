using Microsoft.AspNetCore.Mvc;
using Squish.DATA.EF.Models;
using Microsoft.AspNetCore.Identity;
using Squish.UI.MVC.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using Humanizer;

namespace Squish.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        

        //Properties
        private readonly SQUISHContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        //Constructor
        public ShoppingCartController(SQUISHContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
      
            var sessionCart = HttpContext.Session.GetString("cart");

            
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            if (sessionCart == null || sessionCart.Count() == 0)
            {
                ViewBag.Message = "There are no items in your cart.";

                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                ViewBag.Message = null;

                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

           
            return View(shoppingCart);
        }


        public IActionResult AddToCart(int id)
        {
        
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            var sessionCart = HttpContext.Session.GetString("cart");

           
            if (sessionCart == null)
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }

            else
            {
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            
            }


            SquishInformation squishInformation = _context.SquishInformations.Find(id);

           
            CartItemViewModel civm = new CartItemViewModel(1, squishInformation);

          
            if (shoppingCart.ContainsKey(squishInformation.SquishId))
            {
              
                shoppingCart[squishInformation.SquishId].Qty++;
            }
            else
            {
                shoppingCart.Add(squishInformation.SquishId, civm);
            }


            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {

            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

     
            shoppingCart.Remove(id);

          
            if (shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            
            else
            {
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }

          
            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int productId, int qty)
        {
           
            var sessionCart = HttpContext.Session.GetString("cart");

           
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            shoppingCart[productId].Qty = qty;

            // update session
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        // This method MUST be async in order to invoke the UserManager's async methods in this action.
        public async Task<IActionResult> SubmitOrder()
        {
            string? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            
            UserAccountInfo ud = _context.UserAccountInfo.Find(userId);

            // Create the Order object
            Order o = new Order()
            {
               
               
            };

            // Add the order to _context
            _context.Orders.Add(o);


            // Retrieve the session cart and convert to C#
            var sessionCart = HttpContext.Session.GetString("cart");
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            // Create an OrderProduct record for every Product in our cart
            foreach (var item in shoppingCart)
            {
                Order op = new Order()
                {
                    OrderId = o.OrderId,
                    SquishId = item.Value.CartProd.SquishId,
                    Quantity = (int)(short?)item.Value.Qty
                    
                };

                //ONLY need to add items to an existing entity (here -> the order 'o') if the items are a related record (like the OrderProduct here)
                //o.OrderId.Add(op);

            }

            //Save changes to DB
            _context.SaveChanges();

            // Now that the order has been saved to the database, we can empty the cart.            
            HttpContext.Session.Remove("cart");


            return RedirectToAction("Index", "Orders");

        }


    }
}
