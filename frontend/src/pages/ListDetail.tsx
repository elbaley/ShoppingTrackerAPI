import UserProductsTable from "@/components/UserProductsTable";
import { fetcher } from "@/lib/fetcher";
import { useAuth } from "@/lib/useAuth";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

interface ListDetailProps {}
export interface IProduct {
  id: number;
  name: string;
  price: number;
  category: string;
  productImg: string;
}
export interface IUserProduct {
  id: number;
  userId: number;
  userListId: number;
  description: string;
  isBought: boolean;
  status: "Done" | "Pending";
  product: IProduct;
}
interface IUserList {
  id: number;
  userId: number;
  name: string;
  startedShopping: boolean;
  products: IUserProduct[];
}

const ListDetail = ({}: ListDetailProps) => {
  const [userList, setUserList] = useState<IUserList>({} as IUserList);
  const { user } = useAuth();

  const [startedShopping, setStartedShopping] = useState(true);

  function fetchUserLists() {
    fetcher({
      url: `/UserList/${params.id}`,
      method: "GET",
      token: user.token,
    }).then((userList) => setUserList(userList));
  }

  function toggleStartedShopping() {
    fetcher({
      url: `/UserList/?id=${params.id}`,
      method: "PUT",
      token: user.token,
      json: false,
      body: {
        name: userList.name,
        startedShopping: !userList.startedShopping,
      },
    }).then((data) => {
      if (data.statusCode === 200) {
        setStartedShopping(data.data.startedShopping);
        fetchUserLists();
      }
    });
  }

  useEffect(() => {
    fetchUserLists();
  }, []);

  useEffect(() => {
    setStartedShopping(userList.startedShopping ?? false);
  }, [userList]);

  const params = useParams();
  return (
    <main className="px-3 sm:pl-10 ">
      <div className="flex items-center flex-col sm:flex-row justify-between sm:pr-5">
        <h1 className="pb-3">{userList.name}</h1>
      </div>

      {userList.products && userList.products.length > 0 ? (
        <>
          <div className="flex items-center gap-2 py-3">
            <label htmlFor="startedShopping">Started Shopping</label>
            <input
              name="startedShopping"
              type="checkbox"
              checked={startedShopping}
              onChange={toggleStartedShopping}
            />
          </div>
          <UserProductsTable
            startedShopping={startedShopping}
            fetchUserLists={fetchUserLists}
            userProducts={userList.products ?? []}
          />
        </>
      ) : (
        <h2>This lists doesn't have any products.</h2>
      )}
    </main>
  );
};

export default ListDetail;
