from DiscountStrategy import DiscountStrategy
from typing import List


class ShoppingCart:
    def __init__(self, discounts: List[DiscountStrategy] = []):
        self.items = []
        self.discounts = discounts

    def add_item(self, price):
        self.items.append({'price': price})

    def get_total(self) -> float:
        total = sum(i['price'] for i in self.items)
        for discount in self.discounts:
            total -= discount.calculate_discount(total)
        return max(total, 0.0)
