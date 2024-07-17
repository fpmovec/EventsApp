interface BookingDTO {
  eventId: string;
  userId: string;
  fullName: string;
  phone: string;
  personsQuantity: number;
  email: string;
  birthday: string;
}

interface Booking {
  id: number;
  eventName: string;
  createdDate: Date;
  eventId: string;
  userId: string;
  pricePerOne: string;
  fullName: string;
  phone: string;
  personsQuantity: number;
  email: string;
  birthday: Date;
  totalPrice: number;
}

export type {
    BookingDTO,
    Booking
}
