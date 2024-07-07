import { maxEventPrice } from "../../Constants";
import { EventItem, EventItemExtended } from "../../Models/Event";
import { EventsFilterOptions } from "../../Models/FilterOptions";

const baseUrl = "https://localhost:7107";

export const GetEvents = async (
  options: EventsFilterOptions
): Promise<EventItem[]> => {
  let events: EventItem[] = [];
  const dateInYear = new Date();
  dateInYear.setDate(dateInYear.getDate() + 365 * 2);
  const minDate = ({
    v = options.minDate,
  }: { v?: Date | null | undefined } = {}) => {
    if (!isFinite(+(v as Date))) {
      return new Date();
    } else return v as Date;
  };

  const maxDate = ({
    v = options.maxDate,
  }: { v?: Date | null | undefined } = {}) => {
    if (!isFinite(+(v as Date))) {
      return dateInYear;
    } else return v as Date;
  };

  const response = await fetch(
    `${baseUrl}/Events/get-all?` +
      new URLSearchParams({
        sortType: options.sortType.toString(),
        sortOrder: options.sortOrder.toString(),
        category: options.category,
        minDate: minDate().toDateString(),
        maxDate: maxDate().toDateString(),
        minPrice: options.minPrice.toString(),
        maxPrice: (options.maxPrice == 0
          ? maxEventPrice
          : options.maxPrice
        ).toString(),
        place: options.place,
        searchString: options.searchString,
        currentPage: options.currentPage.toString(),
      }),
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
      credentials: "omit",
    }
  );
  events = await response.json();

  return events;
};

export const GetPopularEvents = async (): Promise<EventItem[]> => {
  const response = await fetch(`${baseUrl}/Events/popular`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "omit",
  });

  return await response.json();
};

export const GetPagesCount = async (): Promise<number> => {
  const response = await fetch(`${baseUrl}/Events/pages`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "omit",
  });

  return await response.json();
};

export const GetEventById = async (id: string): Promise<EventItemExtended> => {
  const response = await fetch(`${baseUrl}/Events/get/${id}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "omit",
  });
  console.log(response);
  return await response.json();
};
