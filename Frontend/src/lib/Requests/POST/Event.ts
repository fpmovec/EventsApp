import { baseUrl } from "../../Constants";
import { EventDTO } from "../../Models/Event";

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

  await fetch(`${baseUrl}/api/events/create`, {
    method: "POST",
    headers: {
      Authorization: "Bearer " + `${token}`,
    },
    body: eventFormData,
  });
};

export const UpdateEvent = async (
  id: string,
  data: EventDTO,
  file: File,
  token: string
): Promise<void> => {
  const eventFormData: FormData = new FormData();

  eventFormData.append("Name", data.name);
  eventFormData.append("Description", data.description);
  eventFormData.append("Place", data.place);
  eventFormData.append("Price", data.price.toString());
  eventFormData.append("Date", new Date(data.dateTime).toLocaleString());
  eventFormData.append(
    "MaxParticipantsCount",
    data.maxParticipantsCount.toString()
  );
  eventFormData.append("CategoryName", data.categoryName);
  eventFormData.append("ImageFile", file ?? null);

  await fetch(`${baseUrl}/api/events/edit/${id}`, {
    method: "PUT",
    headers: {
      Authorization: "Bearer " + `${token}`,
    },
    body: eventFormData,
  });
};
