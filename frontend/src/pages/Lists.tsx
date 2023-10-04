import AddListModal from "@/components/AddListModal";
import { fetcher } from "@/lib/fetcher";
import { useAuth } from "@/lib/useAuth";
import { useEffect, useState } from "react";
import { IoLayers } from "react-icons/io5";
import { Link } from "react-router-dom";
interface ListsProps {}

export interface IUserList {
  id: number;
  name: string;
  startedShopping: boolean;
}
const Lists = ({}: ListsProps) => {
  const [userLists, setUserLists] = useState<IUserList[]>([]);
  const { user } = useAuth();

  function fetchUserLists() {
    fetcher({ url: "/UserList", method: "GET", token: user.token }).then(
      (userLists) => setUserLists(userLists),
    );
  }

  useEffect(() => {
    fetchUserLists();
  }, []);

  return (
    <main className="px-3 sm:pl-10 ">
      <div className="flex items-center flex-col sm:flex-row justify-between sm:pr-5">
        <h1 className="pb-3">Lists</h1>
      </div>
      <AddListModal fetchUserList={fetchUserLists} />

      <div className="flex flex-col gap-2 justify-center md:justify-start">
        {userLists.map((userList) => (
          <ListItem key={userList.id} userList={userList} />
        ))}
      </div>
    </main>
  );
};

const ListItem = ({ userList }: { userList: IUserList }) => {
  return (
    <Link to={`/app/lists/${userList.id}`}>
      <div className="hover:bg-opacity-20 cursor-pointer bg-white dark:bg-darkSecondary rounded-lg text-xl px-4 py-4 flex gap-3 mt-2 items-center shadow-sm">
        <IoLayers />
        <span className="font-bold">{userList.name}</span>
        {userList.startedShopping ? (
          <span className="text-sm bg-actions-success ml-auto text-white px-2 rounded-lg">
            Started
          </span>
        ) : (
          <span className="text-sm bg-actions-warning ml-auto text-white px-2 rounded-lg">
            Pending
          </span>
        )}
      </div>
    </Link>
  );
};

export default Lists;
