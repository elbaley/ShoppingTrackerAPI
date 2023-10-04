import { toast } from "react-toastify";

export const baseURL = "http://localhost:5209";
interface IFetcherProps {
  url: string;
  method: string;
  token: string;
  body?: object;
  json?: boolean;
}
export const fetcher = async ({
  url,
  method,
  body,
  json = true,
  token,
}: IFetcherProps) => {
  const res = await fetch(baseURL + url, {
    mode: "cors",
    method,
    // add a body and set it to body if body is truthy otherwise it wont add
    ...(body && { body: JSON.stringify(body) }),
    headers: new Headers({
      Authorization: `Bearer ${token}`,
      Accept: "application/json",
      "Content-Type": "application/json",
    }),
  });

  const data = await res.json();

  if (!res.ok) {
    // handle error
    toast.error("An error occured");
    return {
      ...data,
    };
  }

  if (json) {
    return data.data;
  } else {
    return data;
  }
};
