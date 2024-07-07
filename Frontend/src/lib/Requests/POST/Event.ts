import { EventDTO } from "../../Models/Event";

const baseUrl = "https://localhost:7107";

export const CreateEvent = async (
  data: EventDTO,
  file: File,
  token: string
): Promise<void> => {
  const eventFormData: FormData = new FormData();

  eventFormData.append("Name", data.name);
  eventFormData.append("Description", data.description);
  eventFormData.append("Place", data.place);
  eventFormData.append("Price", data.price.toString());
  eventFormData.append("Date", data.dateTime.toLocaleDateString());
  eventFormData.append(
    "MaxParticipantsCount",
    data.maxParticipantsCount.toString()
  );
  eventFormData.append("CategoryName", data.categoryName);
  eventFormData.append("ImageFile", file);

  await fetch(`${baseUrl}/events/create`, {
    method: "POST",
    headers: {
      Authorization: "Bearer " + `${token}`,
    },
    body: eventFormData,
  });
};
