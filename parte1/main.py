from ShoppingCart import ShoppingCart
from DiscountStrategy import PercentageDiscount, FixedDiscount, ProgressiveDiscount, CouponDiscount

cart = ShoppingCart()
cart.add_item(100)
cart.add_item(50)
print("Total without discounts:", cart.get_total())
assert cart.get_total() == 150

cart_with_percentage = ShoppingCart(discounts=[PercentageDiscount(10)])
cart_with_percentage.add_item(100)
cart_with_percentage.add_item(50)
print("Total with 10% discount:", cart_with_percentage.get_total())
assert cart_with_percentage.get_total() == 135.0

cart_with_fixed = ShoppingCart(discounts=[FixedDiscount(20)])
cart_with_fixed.add_item(100)
cart_with_fixed.add_item(50)
print("Total with $20 fixed discount:", cart_with_fixed.get_total())
assert cart_with_fixed.get_total() == 130.0

cart_with_progressive_high = ShoppingCart(discounts=[ProgressiveDiscount()])
cart_with_progressive_high.add_item(600)
print("Total with progressive discount on $600:",
      cart_with_progressive_high.get_total())
assert cart_with_progressive_high.get_total() == 510.0
cart_with_progressive_low = ShoppingCart(discounts=[ProgressiveDiscount()])
cart_with_progressive_low.add_item(80)
print("Total with progressive discount on $80:",
      cart_with_progressive_low.get_total())
assert cart_with_progressive_low.get_total() == 76.0
cart_with_progressive_mid = ShoppingCart(discounts=[ProgressiveDiscount()])
cart_with_progressive_mid.add_item(150)
print("Total with progressive discount on $150:",
      cart_with_progressive_mid.get_total())
assert cart_with_progressive_mid.get_total() == 135.0

cart_with_coupon = ShoppingCart(discounts=[CouponDiscount("SAVE10%")])
cart_with_coupon.add_item(200)
print("Total with SAVE10% coupon on $200:", cart_with_coupon.get_total())
assert cart_with_coupon.get_total() == 180.0

cart_with_multiple = ShoppingCart(
    discounts=[PercentageDiscount(10), FixedDiscount(15)])
cart_with_multiple.add_item(300)
print("Total with 10% and $15 fixed discount on $300:",
      cart_with_multiple.get_total())
assert cart_with_multiple.get_total() == 255.0

cart_with_multiple_coupons = ShoppingCart(
    discounts=[CouponDiscount("SAVE20%"), CouponDiscount("SAVE30")])
cart_with_multiple_coupons.add_item(400)
print("Total with SAVE20% and SAVE30 coupons on $400:",
      cart_with_multiple_coupons.get_total())
assert cart_with_multiple_coupons.get_total() == 290.0
