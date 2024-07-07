import { Category } from "./Category";
import { EventImage } from "./Image";

interface EventItem {
  id: string;
  name: string;
  description: string;
  briefDescription: string;
  place: string;
  date: Date;
  price: number;
  image: EventImage;
  category: Category;
}

interface EventItemExtended {
  id: string;
  name: string;
  description: string;
  briefDescription: string;
  place: string;
  date: Date;
  price: number;
  maxParticipantsCount: number;
  bookedTicketsCount: number;
  remainingTicketsCount: number;
  isSoldOut: boolean;
  image: EventImage;
  category: Category;
}

interface EventDTO {
  name: string;
  description: string;
  place: string;
  price: number;
  dateTime: Date;
  maxParticipantsCount: number;
  categoryName: string;
}

export type { EventItem, EventItemExtended, EventDTO };
