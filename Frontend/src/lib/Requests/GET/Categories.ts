const baseUrl = "https://localhost:7107";

export const GetAllCategories = async (): Promise<string[]> => {
  const response = await fetch(`${baseUrl}/categories/get-all`, {
    method: "GET",
    credentials: "omit",
    headers: {
      "Content-Type": "application/json",
    },
  });

  return await response.json();
};
