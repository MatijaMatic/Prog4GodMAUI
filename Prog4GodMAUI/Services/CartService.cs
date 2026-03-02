using Prog4GodMAUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prog4GodMAUI.Services
{
    public class CartService : BaseService
    {
        private readonly ProductService _productService;
        private List<CartItemDetail> _cartItems = new List<CartItemDetail>();
        private int? cartId = null;

        public CartService(ProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        public async Task<Cart> GetCartAsync(int cartId)
        {
            return await GetAsync<Cart>($"carts/{cartId}");
        }

        public List<CartItemDetail> GetCartItems()
        {
            return _cartItems; 
        }

        public async Task RefreshCartItemsByUserIdAsync(int userId)
        {
            var carts = await GetAsync<List<Cart>>($"carts/user/{userId}");
            var cart = carts?.FirstOrDefault();

            cartId = cart?.Id;

            if (cart != null)
            {
                _cartItems.Clear();

                foreach (var cartProduct in cart.Products)
                {
                    var productDetails = await _productService.GetProductByIdAsync(cartProduct.ProductId);
                    _cartItems.Add(new CartItemDetail
                    {
                        Product = productDetails,
                        Quantity = cartProduct.Quantity,
                    });
                }
            }
        }

        public async Task<HttpResponseMessage> DeleteCartAsync()
        {
            var response = await DeleteAsync($"carts/{cartId}");
            if (response.IsSuccessStatusCode)
            {
                _cartItems.Clear();
            }
            return response;
        }

        public void AddProductToCart(Product product)
        {
            var existingCartItem = _cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
         
            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                _cartItems.Add(new CartItemDetail
                {
                    Product = product,
                    Quantity = 1,
                });
            }
        }

        public void IncreaseProductQuantity(int? productId)
        {
            if (!productId.HasValue)
            {
                throw new ArgumentNullException(nameof(productId), "Product ID cannot be null!");
            }

            var existingCartItem = _cartItems.FirstOrDefault(item => item.Product.Id == productId.Value);
            if (existingCartItem == null)
            {
                throw new ArgumentException($"Product with ID {productId.Value} is not in the cart.");
            }

            existingCartItem.Quantity++;
        }

        public void DecreaseProductQuantity(int? productId)
        {
            if (!productId.HasValue)
            {
                throw new ArgumentNullException(nameof(productId), "Product ID cannot be null!");
            }

            var existingCartItem = _cartItems.FirstOrDefault(item => item.Product.Id == productId.Value);

            if (existingCartItem == null)
            {
                throw new ArgumentException($"Product with ID {productId.Value} is not in the cart!");
            }

            existingCartItem.Quantity--;

            if (existingCartItem.Quantity <= 0)
            {
                _cartItems.Remove(existingCartItem);
            }
        }
    }
}
