import { IUserList } from "@/pages/Lists";
import { IProduct } from "@/pages/Products";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Modal from "./Modal";
import { fetcher } from "@/lib/fetcher";
import { useAuth } from "@/lib/useAuth";
import { toast } from "react-toastify";

interface ProductCardProps {
  product: IProduct;
  userLists: IUserList[];
}

const ProductCard = ({ product, userLists }: ProductCardProps) => {
  const [selectedList, setSelectedList] = useState("choose");
  const { user } = useAuth();

  const selectedListName = userLists.find(
    (list) => list.id === parseInt(selectedList),
  )?.name;

  const [isOpen, setIsOpen] = useState(false);
  const [formState, setFormState] = useState({
    productId: product.id,
    userListId: userLists.find((list) => list.id === parseInt(selectedList))
      ?.id,
    description: "",
    isBought: false,
  });

  useEffect(() => {
    setFormState((oldState) => {
      const newState = {
        ...oldState,
        userListId: userLists.find((list) => list.id === parseInt(selectedList))
          ?.id,
      };
      return newState;
    });
  }, [selectedList]);

  function closeModal() {
    setIsOpen(false);
  }

  function openModal() {
    setIsOpen(true);
  }

  function addUserProduct() {
    fetcher({
      url: "/UserProduct",
      method: "POST",
      body: formState,
      token: user.token,
      json: false,
    })
      .then((data) => {
        console.log(data);
        if (data.statusCode !== 201) {
          toast.error(data.message);
        } else {
          toast.success("Product added to list!");
          closeModal();
        }
      })
      .catch((err) => {
        console.log(err);
      });
  }

  return (
    <>
      <div className="relative flex w-full max-w-xs  flex-col overflow-hidden rounded-lg border border-gray-100 bg-white shadow-md">
        <a
          className="relative mx-3 mt-3 flex h-60 overflow-hidden rounded-xl"
          href="#"
        >
          <img
            className="object-cover w-full"
            src={product.productImg}
            alt="product image"
          />
          <span className="absolute top-0 left-0 m-2 rounded-full bg-black px-2 text-center text-sm font-medium text-white">
            {product.category}
          </span>
        </a>
        <div className="px-5 pb-5">
          <a href="#">
            <h5 className="py-1 text-xl tracking-tight text-slate-900">
              {product.name}
            </h5>
          </a>
          <div className=" mb-5 flex items-center justify-between">
            <p>
              <span className="text-3xl font-bold text-slate-900">
                ${product.price}
              </span>
            </p>
          </div>
          <div className="flex items-center gap-1">
            {userLists.length === 0 ? (
              <p>
                To add the product{" "}
                <Link to="/lists" className="font-bold">
                  create a list
                </Link>
              </p>
            ) : (
              <>
                <select
                  id="small"
                  value={selectedList}
                  onChange={(e) => setSelectedList(e.target.value)}
                  className="block w-full p-2 text-sm text-gray-900 border border-gray-300 rounded-lg bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                >
                  <option value="choose" disabled>
                    Choose a list
                  </option>
                  {userLists.map((list) => {
                    return (
                      <option key={list.id} value={list.id}>
                        {list.name}
                      </option>
                    );
                  })}
                </select>

                <button
                  disabled={selectedList === "choose"}
                  onClick={openModal}
                  className="flex items-center justify-center rounded-md bg-slate-900 p-2 text-center text-sm font-medium text-white hover:bg-gray-700 focus:outline-none focus:ring-4 focus:ring-blue-300"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    className="h-6 w-6"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                    strokeWidth="2"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"
                    />
                  </svg>
                </button>
              </>
            )}
          </div>
        </div>
      </div>
      <Modal
        closeModal={closeModal}
        isOpen={isOpen}
        title={`Add to: ${selectedListName}`}
      >
        <div className="mb-3">
          <label
            className="block uppercase tracking-wide text-gray-700 dark:text-darkTextSecondary text-xs font-bold mb-2"
            htmlFor="name"
          >
            Description
          </label>
          <input
            className="block appearance-none w-full bg-gray-200 dark:bg-darkSecondary border border-gray-200 dark:border-black text-gray-700 dark:text-white py-3 px-4 pr-8 rounded leading-tight focus:outline-none focus:border-gray-500"
            onChange={(e) => {
              setFormState((oldState) => {
                const newState = { ...oldState, description: e.target.value };
                return newState;
              });
            }}
            value={formState.description}
          />
        </div>
        <button
          onClick={addUserProduct}
          className="hover:bg-opacity-70 p-2 w-full bg-black text-white rounded-lg"
        >
          Add
        </button>
      </Modal>
    </>
  );
};

export default ProductCard;
