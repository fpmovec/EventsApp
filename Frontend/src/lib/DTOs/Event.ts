import { Category } from "./Category";
import { EventImage } from "./Image";

interface EventItem {
    id: string;
    name: string;
    description: string;
    briefDescription: string;
    place: string;
    dateTime: Date;
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
    dateTime: Date;
    price: number;
    maxParticipantsCount: number;
    bookedTicketsCount: number;
    remainingTicketsCount: number;
    isSoldOut: number;
    image: EventImage;
    category: Category;
}

export type {
    EventItem,
    EventItemExtended
}