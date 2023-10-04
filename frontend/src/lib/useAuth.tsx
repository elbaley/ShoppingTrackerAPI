import { PropsWithChildren, createContext, useContext, useMemo } from "react";
import { useNavigate } from "react-router-dom";
import { useLocalStorage } from "./useLocalStorage";
import { baseURL, fetcher } from "./fetcher";
import { toast } from "react-toastify";

type RegisterData = {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
};
interface IUserList {
  id: number;
  name: string;
  startedShopping: boolean;
}

interface IUser {
  name: string;
  token: string;
  role: string;
  userLists: IUserList[];
}

type UserContextValues = {
  user: IUser;
  logout: () => void;
  register: (data: RegisterData) => Promise<void>;
  login: (data: any) => Promise<void>;
  fetchCategories: () => Promise<any>;
};

const AuthContext = createContext({} as UserContextValues);
export const AuthProvider = ({ children }: PropsWithChildren) => {
  const [user, setUser] = useLocalStorage("user", null);
  const navigate = useNavigate();

  // call this for registration
  const register = async (data: RegisterData) => {
    // send api request
    fetch(baseURL + "/api/Auth/register", {
      headers: new Headers({
        Accept: "application/json",
        "Content-Type": "application/json",
      }),
      method: "POST",
      body: JSON.stringify(data),
    })
      .then((res) => res.json())
      .then((data) => {
        if (data.statusCode !== 201) {
          toast.error(data.message);
        } else {
          toast.success("Registered successfully, now you can log in.");
          setTimeout(() => {
            navigate("/login");
          }, 2000);
        }
      })
      .catch((e) => console.log(e));
  };

  // call this function when you want to authenticate the user
  type LoginData = {
    email: string;
    password: string;
  };
  const login = async (data: LoginData) => {
    // send api request
    fetch(baseURL + "/api/Auth/login", {
      headers: new Headers({
        Accept: "application/json",
        "Content-Type": "application/json",
      }),
      method: "POST",
      body: JSON.stringify(data),
    })
      .then((res) => res.json())
      .then((data) => {
        if (data.statusCode !== 200) {
          toast.error(data.message);
        } else {
          toast.success("Logged in");
          const token = data.data.token;
          const name = data.data.name;
          const role = data.data.role;
          setUser({
            token: token,
            name: name,
            role: role,
          });
          if (role === "User") {
            navigate("/app/products");
          }
          if (role === "Admin") {
            navigate("/app/admin/products");
          }
        }
      })
      .catch((e) => console.log(e));
  };

  // call this function to sign out logged in user
  const logout = () => {
    setUser(null);
    navigate("/login", { replace: true });
  };

  const fetchCategories = async () => {
    const categories = await fetcher({
      url: "/Category",
      method: "GET",
      token: user.token,
    });
    return categories;
  };

  const value = useMemo(
    () => ({
      user,
      login,
      logout,
      register,
      fetchCategories,
    }),
    [user],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  return useContext(AuthContext);
};
