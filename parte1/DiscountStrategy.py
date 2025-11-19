from abc import ABC, abstractmethod


class DiscountStrategy(ABC):
    @abstractmethod
    def calculate_discount(self, amount: float) -> float:
        pass


class PercentageDiscount(DiscountStrategy):
    def __init__(self, percentage: float):
        self.percentage = percentage

    def calculate_discount(self, amount: float) -> float:
        return amount * (self.percentage / 100)


class FixedDiscount(DiscountStrategy):
    def __init__(self, fixed_amount: float):
        self.fixed_amount = fixed_amount

    def calculate_discount(self, amount: float) -> float:
        return min(self.fixed_amount, amount)


class ProgressiveDiscount(DiscountStrategy):
    def calculate_discount(self, amount: float) -> float:
        if amount > 500:
            return PercentageDiscount(15).calculate_discount(amount)
        elif amount > 100:
            return PercentageDiscount(10).calculate_discount(amount)
        return PercentageDiscount(5).calculate_discount(amount)


class CouponDiscount(DiscountStrategy):
    def __init__(self, coupon_code: str):
        self.coupon_code = coupon_code
        self.coupons = {
            "SAVE10%": PercentageDiscount(10),
            "SAVE20%": PercentageDiscount(20),
            "SAVE30": FixedDiscount(30),
        }

    def calculate_discount(self, amount: float) -> float:
        discount_strategy = self.coupons.get(self.coupon_code)
        if discount_strategy:
            return discount_strategy.calculate_discount(amount)
        return 0.0
