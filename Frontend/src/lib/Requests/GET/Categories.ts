import { baseUrl } from "../../Constants";

export const GetAllCategories = async (): Promise<string[]> => {
  const response = await fetch(`${baseUrl}/api/categories/get-all`, {
    method: "GET",
    credentials: "omit",
    headers: {
      "Content-Type": "application/json",
    },
  });

  return await response.json();
};
